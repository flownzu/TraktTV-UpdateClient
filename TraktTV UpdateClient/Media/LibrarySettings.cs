using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TraktTVUpdateClient.Media
{
    public class LibrarySettings
    {
        [JsonProperty(PropertyName = "libraries")]
        public List<MediaLibrary> Libraries { get; set; }

        public LibrarySettings()
        {
            Libraries = new List<MediaLibrary>();
        }

        public void Save()
        {
            using (FileStream fs = File.Open("mediaLibraries.json", FileMode.Create, FileAccess.ReadWrite))
            {
                var settings = System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(this));
                fs.Write(settings, 0, settings.Length);
            }
        }

        public static LibrarySettings Load(string fileName = "mediaLibraries.json")
        {
            if (File.Exists(fileName))
            {
                try
                {
                    var settings = JsonConvert.DeserializeObject<LibrarySettings>(File.ReadAllText(fileName));
                    return settings;
                }
                catch { return new LibrarySettings(); }
            }
            else return new LibrarySettings();
        }

        public async Task LoadFiles()
        {
            List<Task> taskList = new List<Task>();
            foreach (MediaLibrary lib in Libraries)
            {
                taskList.Add(Task.Run(() => lib.GetAllFiles()));
            }
            await Task.WhenAll(taskList);
        }
    }
}
