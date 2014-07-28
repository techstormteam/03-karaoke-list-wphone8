using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using KaraokeList.Models;
using Coding4Fun.Toolkit.Controls;

namespace KaraokeList.Views
{
    public partial class PageSongDetail : PhoneApplicationPage
    {
        ToastPrompt toast = new ToastPrompt();

        public PageSongDetail()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ModelSong song = App.ViewModelSongProperty.SelectedEntry;
            lblSongId.Text = song.SongId.ToString();
            lblSongName.Text = song.Name;
            lblLyric.Text = song.Lyric;
            lblAuthor.Text = song.Meta;
            lblQuickSearch.Text = song.Abbr;
        }

        private void btnAddToFav_Click(object sender, EventArgs e)
        {
            ModelSong song = App.ViewModelSongProperty.SelectedEntry;
            if (song.Favourite != 0)
            {
                toast.Message = "Đã có sẵn trong danh sách!";
            }
            else
            {
                App.ViewModelFavoriteProperty.addFavoriteSong(song);
                toast.Message = "Bài hát \"" + song.Name + "\" đã được thêm vào danh sách yêu thích";
            }
            toast.Show();
        }

        private ToastPrompt createToastPrompt()
        {
            ToastPrompt toast = new ToastPrompt();
            toast.TextWrapping = TextWrapping.Wrap;
            toast.IsTimerEnabled = true;
            toast.Stretch = System.Windows.Media.Stretch.UniformToFill;
            //toast.Completed += toast_Completed;
            return toast;
        }
    }
}