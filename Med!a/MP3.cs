using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med_a
{
    public class MP3
    {
        private string title;
        private string artist;
        private string album;
        private string location;
        private string genre;
        private TimeSpan duration;
        private int plays;
        private int MP3_id;

        public MP3()
        {}

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Artist
        {
            get { return artist; }
            set { artist = value; }
        }

        public string Album
        {
            get { return album; }
            set { album = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public int Plays
        {
            get { return plays; }
            set { plays = value; }
        }

        public int MP3_ID
        {
            get { return MP3_id; }
            set { MP3_id = value; }
        }

        public override string ToString()
        {
            return "Title: " + title + " Location: " + location;
        }
    }
}
