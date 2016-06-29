using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Med_a
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region variables
        ObservableCollection<MP3> Music = new ObservableCollection<MP3>();
        ObservableCollection<MP4> Videos = new ObservableCollection<MP4>();
        ObservableCollection<Playlist> Playlists = new ObservableCollection<Playlist>();

        List<int> order = new List<int>();
        Random rand = new Random();

        MediaPlayer player = new MediaPlayer();

        MP3 currentMP3 = null;
        MP4 currentMP4 = null;
        Playlist currentPlaylist = null;

        bool random = false;
        bool playing = false;
        bool userDragging = false;
        bool MP3 = false;
        bool MP4 = false;
        bool Edit = false;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            populateMP3();
            populateMP4();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            currentPlaylist = new Playlist("Library");
            SQLHandler.selectQuery("SELECT * FROM MP3");
            if (SQLHandler.read.HasRows)
            {
                MP3 song = new MP3();
                while (SQLHandler.read.Read())
                {
                    song = new MP3();
                    song.MP3_ID = Convert.ToInt16(SQLHandler.read[0]);
                    song.Title = SQLHandler.read[1].ToString();
                    song.Artist = SQLHandler.read[2].ToString();
                    song.Album = SQLHandler.read[3].ToString();
                    song.Location = SQLHandler.read[6].ToString();
                    song.Genre = SQLHandler.read[7].ToString();
                    try
                    {
                        string[] results = SQLHandler.read[4].ToString().Split(':');
                        int[] result = new int[] { Convert.ToInt16(results[0]), Convert.ToInt16(results[1]), Convert.ToInt16(results[2]) };
                        song.Duration = new TimeSpan(result[0], result[1], result[2]);
                    }
                    catch (Exception) { }
                    try
                    {
                        song.Plays = Convert.ToInt16(SQLHandler.read[5]);
                    }
                    catch (Exception) { }
                    currentPlaylist.add(song);
                }
            }
            Playlists.Add(currentPlaylist);
            PlaylistListBox.ItemsSource = Playlists;
        }

        #region Populating Listbox Methods

        /// <summary>
        /// Populates the library listbox from the current playlist
        /// </summary>
        private void populateMP3()
        {
            Music.Clear();
            SQLHandler.selectQuery("SELECT * FROM MP3 ORDER BY artist ASC");
            if (SQLHandler.read.HasRows)
            {
                MP3 song = new MP3();
                while (SQLHandler.read.Read())
                {
                    song = new MP3();
                    song.MP3_ID = Convert.ToInt16(SQLHandler.read[0]);
                    song.Title = SQLHandler.read[1].ToString();
                    song.Artist = SQLHandler.read[2].ToString();
                    song.Album = SQLHandler.read[3].ToString();
                    song.Location = SQLHandler.read[6].ToString();
                    song.Genre = SQLHandler.read[7].ToString();
                    try
                    {
                        string[] results = SQLHandler.read[4].ToString().Split(':');
                        int[] result = new int[] { Convert.ToInt16(results[0]), Convert.ToInt16(results[1]), Convert.ToInt16(results[2]) };
                        song.Duration = new TimeSpan(result[0], result[1], result[2]);
                    }
                    catch (Exception) { }
                    try
                    {
                        song.Plays = Convert.ToInt16(SQLHandler.read[5]);
                    }
                    catch (Exception) { }
                    Music.Add(song);
                }
            }
            MP3_Listbox.ItemsSource = Music;
        }

        private void populateMusic()
        {
           
        }

        private void populateMP4()
        {
            Videos.Clear();
            SQLHandler.selectQuery("SELECT * FROM MP4 ORDER BY title ASC");
            if (SQLHandler.read.HasRows)
            {
                MP4 video = new MP4();
                while (SQLHandler.read.Read())
                {
                    video = new MP4();
                    video.MP4_ID = Convert.ToInt16(SQLHandler.read[0]);
                    video.Title = SQLHandler.read[1].ToString();
                    video.Location = SQLHandler.read[5].ToString();
                    try
                    {
                        string[] results = SQLHandler.read[3].ToString().Split(':');
                        int[] result = new int[] { Convert.ToInt16(results[0]), Convert.ToInt16(results[1]), Convert.ToInt16(results[2]) };
                        video.Duration = new TimeSpan(result[0], result[1], result[2]);
                    }
                    catch (Exception) { }
                    try
                    {
                        video.Plays = Convert.ToInt16(SQLHandler.read[4]);
                    }
                    catch (Exception) { }
                    Videos.Add(video);
                }
            }
            VideoListBox.ItemsSource = Videos;
        }

        private void populatePlaylists()
        {

        }
        #endregion

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(player.Source != null && player.NaturalDuration.HasTimeSpan)
            {
                progressSlider.Minimum = 0;
                progressSlider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
                if (!userDragging)
                {
                    progressSlider.Value = player.Position.TotalSeconds;
                }
                currentTimeLabel.Content = player.Position.ToString().Split(':')[1] + ":" + player.Position.ToString().Split(':')[2].Split('.')[0];

                if(player.Position.TotalSeconds >= currentMP3.Duration.TotalSeconds)
                {
                    next();
                }
            }
        }

        private void New_Playlist_Click(object sender, RoutedEventArgs e)
        {
            SQLHandler.selectQuery("INSERT INTO Playlist (name) VALUES ('" + PlaylistNameTextBox.Text + "')");
        }

        private void MP3_Listbox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Edit && MP3)
                {
                    currentMP3 = (MP3)MP3_Listbox.SelectedItem;
                    maxTimeLabel.Content = currentMP3.Duration.ToString().Split(':')[1] + ":" + currentMP3.Duration.ToString().Split(':')[2].Split('.')[0];
                    DisplayArtistLabel.Content = currentMP3.Artist;
                    DisplayTitleLabel.Content = currentMP3.Title;
                    int index = currentMP3.MP3_ID;
                    SQLHandler.selectQuery("SELECT location FROM MP3 WHERE MP3_id='" + index + "'");
                    if (SQLHandler.read.HasRows)
                    {
                        while (SQLHandler.read.Read())
                        {
                            player.Open(new Uri(SQLHandler.read[0].ToString()));
                            player.Play();
                            playing = true;
                        }
                    }
                }
                else if (Edit)
                {
                    MP3 song = (MP3)MP3_Listbox.SelectedItem;
                    MessageBox.Show(song.ToString());
                    Edit_MP3 next = new Edit_MP3();
                    next.ID = song;
                    next.ShowDialog();
                }
                else
                {
                    MP3 = true;
                }
            }
            catch (Exception)
            { }
        }

        private void MP3_Listbox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    TagLib.File current = TagLib.File.Create(file);
                    int minutes = current.Properties.Duration.Minutes;
                    int seconds = current.Properties.Duration.Seconds;
                    SQLHandler.executeQuery("INSERT INTO MP3(title, artist, album, duration, plays, location) VALUES ('" + current.Tag.Title + "','" + current.Tag.FirstAlbumArtist + "','" + current.Tag.Album + "', '00:" + minutes + ":" + seconds + "', 0 ,'" + file + "')");
                    SQLHandler.selectQuery("SELECT MAX(MP3_id) FROM MP3");
                    int num = Convert.ToInt32(SQLHandler.read.Read());
                    SQLHandler.executeQuery("INSERT INTO hasMP3(Playlist_id, MP3_id) VALUES ('1', '" + num + "'");
                    Music.Clear();
                    populateMP3();
                }
            }
        }

        private void next()
        {
            if (random)
            {
                if (order.Count == 1)
                {
                    generateOrder();
                }
                int next = rand.Next(1, order.Count);
                int val = order[next];
                order.Remove(val);
                MP3_Listbox.SelectedIndex = val;
                return;
            }
            else if (MP3_Listbox.SelectedIndex == Music.Count - 1)
            {
                MP3_Listbox.SelectedIndex = 0;
                return;
            }
            MP3_Listbox.SelectedIndex++;
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            next();
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if(MP3_Listbox.SelectedIndex == 0)
            {
                MP3_Listbox.SelectedIndex = Music.Count - 1;
                return;
            }
            MP3_Listbox.SelectedIndex--;
        }

        private void randomButton_Click(object sender, RoutedEventArgs e)
        {
            random = !random;
            if (random)
            {
                generateOrder();
            }
        }

        private void generateOrder()
        {
            for (int i = 0; i < Music.Count; i++)
            {
                order.Add(i);
            }
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if(player.Source != null)
            {
                if (playing)
                {
                    playing = false;
                    player.Pause();
                }
                else
                {
                    playing = true;
                    player.Play();
                }
            }
        }

        private void progressSlider_DragStarted(object sender, RoutedEventArgs e)
        {
            userDragging = true;
        }

        private void progressSlider_DragCompleted(object sender, RoutedEventArgs e)
        {
            userDragging = false;
            player.Position = TimeSpan.FromSeconds(progressSlider.Value);
        }

        private void progressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<Double> e)
        {
            currentTimeLabel.Content = player.Position.ToString().Split(':')[1] + ":" + player.Position.ToString().Split(':')[2].Split('.')[0];
        }

        private void VideoListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    TagLib.File current = TagLib.File.Create(file);
                    int minutes = current.Properties.Duration.Minutes;
                    int seconds = current.Properties.Duration.Seconds;
                    SQLHandler.executeQuery("INSERT INTO MP4(title, duration, plays, location) VALUES ('" + current.Tag.Title + "', '00:" + minutes + ":" + seconds + "', 0 ,'" + file + "')");
                    Videos.Clear();
                    populateMP4();
                }
            }
        }

        private void VideoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MP4)
                {
                    MP4 video = (MP4)VideoListBox.SelectedItem;
                    VideoPlayer.Source = new Uri(video.Location);
                    VideoPlayer.Play();
                }
                else
                {
                    MP4 = true;
                }
            }
            catch (Exception)
            { }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            Edit = !Edit;
        }

        public void updateMain()
        {
            MP3_Listbox.ItemsSource = null;
            VideoListBox.ItemsSource = null;
            PlaylistListBox.ItemsSource = null;
            populateMP3();
            populateMP4();
        }

        private void PlaylistListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
