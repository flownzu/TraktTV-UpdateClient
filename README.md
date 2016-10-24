# TraktTV-UpdateClient

A simple trakt.tv Desktop Client that enables you to update your watched shows more efficiently.

Working List:

Add/Remove episodes from your watched list.

Add ratings to watched shows.

Search for new shows to add to your watched list.

Basic functionality:

Connecting and re-connecting to VLC. Upon playing a new video file in VLC the program will recognize the file name that is played and will try to recognize the show name and search for it on trakt.tv (searches for up to 15 entries, I'm still debating with myself whether this value will be customizable for the user) and pick the one that is the most similar to the recognized show title. After that it will wait until a certain play percent is reached (defaults to 90% but fully customizable down to 50%) and add the recognized episode to your watched list.

This will currently only work if the file you are watching is following the naming standards of most file sharing sites (eg. The.Night.Of.S01E01.720p.WEB-DL.DD5.1.H.264-NTb.mkv). 

**NOTE: This program will not help you obtain illegal copies of TV-Show episodes. I strongly recommend getting Netflix/Amazon Prime/whatever streaming platform is available in your country.**
