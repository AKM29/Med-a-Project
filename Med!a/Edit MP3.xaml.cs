using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Med_a
{
    /// <summary>
    /// Interaction logic for Edit_MP3.xaml
    /// </summary>
    public partial class Edit_MP3
    {
        private MP3 song = null;

        public MP3 ID
        {
            get { return song; }
            set { song = value; }
        }

        public Edit_MP3()
        {
            InitializeComponent();
        }

        private void SubmitDetails_Click(object sender, RoutedEventArgs e)
        {
            SQLHandler.executeQuery("UPDATE MP3 SET title='" + titleTextBox.Text + "', album='" + albumTextBox.Text + "', artist='" + artistTextBox.Text + "', genre='" + genreTextBox.Text + "' WHERE MP3_id='" + song.MP3_ID + "'");

            MainWindow parent = (MainWindow)Application.Current.MainWindow;
            parent.updateMain();
            Close();
        }

        private void setupInfo()
        {
            titleTextBox.Text = song.Title;
            albumTextBox.Text = song.Album;
            artistTextBox.Text = song.Artist;
            genreTextBox.Text = song.Genre;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            setupInfo();
        }
    }
}
