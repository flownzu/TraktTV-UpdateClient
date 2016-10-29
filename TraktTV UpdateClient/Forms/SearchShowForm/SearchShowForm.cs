﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktApiSharp;
using TraktApiSharp.Enums;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Shows.Episodes;
using TraktApiSharp.Objects.Get.Shows.Seasons;
using TraktApiSharp.Objects.Post.Syncs.History;
using TraktApiSharp.Requests.Params;
using TraktTVUpdateClient.Extension;

namespace TraktTVUpdateClient.Forms
{
    public partial class SearchShowForm : Form
    {
        Cache TraktCache;
        List<TraktShow> lastSearch = new List<TraktShow>();

        public SearchShowForm(Cache cache)
        {
            InitializeComponent();
            TraktCache = cache;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
            {
                foreach (ToolStripMenuItem item in this.contextMenuStrip1.Items)
                {
                    item.Enabled = false;
                }
            }
            else
            {
                addSpecificSeasonToolStripMenuItem.DropDownItems.Clear();
                addSpecificEpisodeToolStripMenuItem.DropDownItems.Clear();
                TraktShow selectedShow = lastSearch.Find(x => x.Title.Equals(listView1.SelectedItems[0].SubItems[0].Text));
                if (selectedShow.Seasons != null)
                {
                    var seasons = selectedShow.Seasons.Where(x => x.Number > 0 && x.Episodes != null);
                    if (seasons != null)
                    {
                        ToolStripMenuItem[] episodeMenu = new ToolStripMenuItem[seasons.Count()];
                        ToolStripMenuItem[] seasonMenu = new ToolStripMenuItem[seasons.Count()];
                        int index = 0;
                        foreach (TraktSeason season in seasons)
                        {
                            episodeMenu[index] = new ToolStripMenuItem();
                            episodeMenu[index].Text = "Season " + season.Number.Value;
                            episodeMenu[index].Name = "s" + season.Number.Value;
                            episodeMenu[index].Click += new EventHandler(MenuItemClickHandler);
                            seasonMenu[index] = new ToolStripMenuItem();
                            seasonMenu[index].Text = "Season " + season.Number.Value;
                            seasonMenu[index].Name = "s" + season.Number.Value;
                            seasonMenu[index].Click += new EventHandler(MenuItemClickHandler);
                            seasonMenu[index].Tag = "season" + season.Number.Value;
                            ToolStripMenuItem[] episodeList = new ToolStripMenuItem[season.Episodes.Count()];
                            for (int a = 0; a < season.Episodes.Count(); a++)
                            {
                                episodeList[a] = new ToolStripMenuItem();
                                episodeList[a].Text = "Episode " + (a + 1);
                                episodeList[a].Tag = "s" + season.Number.Value + "e" + (a + 1);
                                episodeList[a].Click += new EventHandler(MenuItemClickHandler);
                            }
                            episodeMenu[index].DropDownItems.AddRange(episodeList);
                            index++;
                        }
                        addSpecificSeasonToolStripMenuItem.DropDownItems.AddRange(seasonMenu);
                        addSpecificEpisodeToolStripMenuItem.DropDownItems.AddRange(episodeMenu);
                    }
                }
            }
        }

        private async void MenuItemClickHandler(object sender, EventArgs e)
        {
            if (TraktCache.TraktClient.IsValidForUseWithAuthorization)
            {
                TraktShow selectedShow = lastSearch.Find(x => x.Title.Equals(listView1.SelectedItems[0].SubItems[0].Text));
                ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
                String tag = (string)clickedItem.Tag;
                TraktSyncHistoryPostBuilder historyPostBuilder = new TraktSyncHistoryPostBuilder();
                if (tag.Contains("season"))
                {
                    int seasonnumber = Int32.Parse(tag.Replace("season", ""));
                    historyPostBuilder.AddShow(selectedShow, seasonnumber);
                    var addHistoryResponse = await TraktCache.TraktClient.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                    if((addHistoryResponse.Added.Seasons.HasValue && addHistoryResponse.Added.Seasons.Value >= 1) || (addHistoryResponse.Added.Episodes.HasValue && addHistoryResponse.Added.Episodes.Value >= 1))
                    {
                        await TraktCache.SyncShowProgress(selectedShow.Ids.Slug);
                        Task.Run(() => TraktCache.Sync()).Forget();
                    }
                    MessageBox.Show("The selected season was added to the watched list.");
                }
                else if (Regex.Match(tag, @"s\d+e\d+").Success)
                {
                    int seasonnumber = Int32.Parse(Regex.Replace(tag, @"s(\d+)e\d+", "$1"));
                    int episodenumber = Int32.Parse(Regex.Replace(tag, @"s\d+e(\d+)", "$1"));
                    TraktEpisode ep = selectedShow.Seasons.Where(x => x.Number.Equals(seasonnumber)).First().Episodes.Where(x => x.Number.Equals(episodenumber)).First();
                    historyPostBuilder.AddEpisode(ep);
                    var addHistoryResponse = await TraktCache.TraktClient.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                    if(addHistoryResponse.Added.Episodes.HasValue && addHistoryResponse.Added.Episodes.Value >= 1)
                    {
                        await TraktCache.SyncShowProgress(selectedShow.Ids.Slug);
                        Task.Run(() => TraktCache.Sync()).Forget();
                    }
                    MessageBox.Show("The selected episode was added to the watched list.");
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != String.Empty)
            {
                this.listView1.Items.Clear();
                int maxResults = 5;
                if (this.textBox2.Text != String.Empty) Int32.TryParse(this.textBox2.Text, out maxResults);
                lastSearch = await searchShows(this.textBox1.Text, maxResults + 1);
                List<Task> taskList = new List<Task>();
                foreach(TraktShow show in lastSearch)
                {
                    taskList.Add(Task.Run(() => SyncSeasonOverview(show)));
                    ListViewItem lvi = listView1.Items.Add(new ListViewItem(new string[] { show.Title, "" }));
                    if (show.Year.HasValue) lvi.SubItems[1].Text = show.Year.ToString();
                    String genreString = "";
                    foreach (String s in show.Genres)
                    {
                        genreString += s.UpperCase() + ", ";
                    }
                    if (genreString.Equals(String.Empty)) genreString = "None, ";
                }
                await Task.WhenAll(taskList);
            }
        }

        private async Task SyncSeasonOverview(TraktShow show)
        {
            show.Seasons = await TraktCache.TraktClient.Seasons.GetAllSeasonsAsync(show.Ids.Slug);
            List<Task> taskList = new List<Task>();
            foreach(TraktSeason season in show.Seasons)
            {
               if(season.Number > 0)
                {
                    taskList.Add(Task.Run(() => SyncSeasonEpisodes(show.Ids.Slug, season)));
                }
            }
            await Task.WhenAll(taskList);
        }

        private async Task SyncSeasonEpisodes(string showIdOrSlug, TraktSeason season)
        {
            season.Episodes = await TraktCache.TraktClient.Seasons.GetSeasonAsync(showIdOrSlug, season.Number.Value);
        }

        private String UpperCase(String s)
        {
            char[] array = s.ToCharArray();
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ' || array[i - 1] == '-')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new String(array);
        }

        private async Task<List<TraktShow>> searchShows(String title, int maxSearchResults = 5)
        {
            List<TraktShow> showList = new List<TraktShow>();
            var searchResult = await TraktCache.TraktClient.Search.GetTextQueryResultsAsync(TraktSearchResultType.Show, title, TraktSearchField.Title, limitPerPage: maxSearchResults, extendedInfo: new TraktExtendedInfo().SetFull());
            if (searchResult != null)
            {
                foreach(TraktSearchResult searchItem in searchResult.Items)
                {
                    showList.Add(searchItem.Show);
                }
            }
            return showList;
        }

        private async void add1stEpisodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TraktCache.TraktClient.IsValidForUseWithAuthorization)
            {
                TraktShow selectedShow = lastSearch.Find(x => x.Title.Equals(this.listView1.SelectedItems[0].SubItems[0].Text));
                TraktSeason firstSeason = selectedShow.Seasons.Where(x => x.Number.Value.Equals(1)).First();
                TraktEpisode firstEpisode = firstSeason.Episodes.Where(x => x.Number.Value.Equals(1)).First();
                TraktSyncHistoryPostBuilder historyPostBuilder = new TraktSyncHistoryPostBuilder();
                historyPostBuilder.AddEpisode(firstEpisode);
                var addEpisodeResponse = await TraktCache.TraktClient.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                if(addEpisodeResponse.Added.Episodes.HasValue && addEpisodeResponse.Added.Episodes.Value >= 1)
                {
                    await TraktCache.SyncShowProgress(selectedShow);
                    Task.Run(() => TraktCache.Sync()).Forget();
                }
            }
        }

        private async void addAllEpisodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TraktCache.TraktClient.IsValidForUseWithAuthorization)
            {
                TraktShow selectedShow = lastSearch.Find(x => x.Title.Equals(this.listView1.SelectedItems[0].SubItems[0].Text));
                TraktSyncHistoryPostBuilder historyPostBuilder = new TraktSyncHistoryPostBuilder();
                historyPostBuilder.AddShow(selectedShow);
                var addShowResponse = await TraktCache.TraktClient.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                if((addShowResponse.Added.Shows.HasValue && addShowResponse.Added.Shows.Value >= 1) || (addShowResponse.Added.Seasons.HasValue && addShowResponse.Added.Seasons.Value >= 1) || (addShowResponse.Added.Episodes.HasValue && addShowResponse.Added.Episodes.Value >= 1))
                {
                    await TraktCache.SyncShowProgress(selectedShow);
                    Task.Run(() => TraktCache.Sync()).Forget();
                }
            }
        }
    }
}
