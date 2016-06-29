using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med_a
{
    public class MP4
    {
        private string title;
        private string location;
        private int plays;
        private TimeSpan duration;
        private int MP4_id;

        public MP4()
        {}

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
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

        public int MP4_ID
        {
            get { return MP4_id; }
            set { MP4_id = value; }
        }

        public override string ToString()
        {
            return "Title: " + title + " Location: " + location;
        }
    }
}
