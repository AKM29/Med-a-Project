using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med_a
{
    public class Playlist
    {
        private ObservableCollection<MP3> music = new ObservableCollection<MP3>();
        private string name;
        private int numitems;
        private int playlist_id;
        private TimeSpan duration;

        public Playlist(String Name)
        {
            name = Name;
        }

        public ObservableCollection<MP3> Music
        {
            get { return music; }
            set { music = value; }
        }

        public string Name
        {
            get { return name; }
        }

        public int Count
        {
            get { return numitems; }
        }

        public TimeSpan Duration
        {
            get { return duration; }
        }

        public void add(MP3 Song)
        {
            music.Add(Song);
        }

        public int Playlist_id
        {
            get { return playlist_id; }
            set { playlist_id = value; }
        }
    }
}
