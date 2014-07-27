using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace KaraokeList.Views
{
    public partial class PageSongSearch : PhoneApplicationPage
    {
        private BitmapImage seachImageSource = new BitmapImage(new Uri(@"Assets/Images/feature.search.png", UriKind.Relative));
        private BitmapImage clearImageSource = new BitmapImage(new Uri(@"Assets/Images/feature.clear.png", UriKind.Relative));

        public PageSongSearch()
        {
            InitializeComponent();
        }

        private void btnActionIcon_Tapped(object sender, EventArgs e)
        {
            tbWordSearching.Text = "";
        }

        private void tbWordSearching_TextChanged(object sender, RoutedEventArgs e)
        {
            PhoneTextBox tbWordSearching = (PhoneTextBox)sender;
            string searchString = tbWordSearching.Text;


            if (String.IsNullOrEmpty(searchString))
            {
                tbWordSearching.ActionIcon = seachImageSource;
                //moveToItem(0);
                return;
            }
            tbWordSearching.ActionIcon = clearImageSource;

            //newHook = searchAPI.getHook(searchString);
            //if (newHook != null)
            //{
            //    moveToItem(newHook.Data.fromPosition);
            //}

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            
        }

        private void btnVoiceSearch_Click(object sender, EventArgs e)
        {
            
        }

        private void tbWordSearching_KeyUp(object sender, KeyEventArgs e)
        {
            //if (isSearching() && e.Key == Key.Enter)
            //{
            //    App.EntryViewModelProperty.SelectedEntry = App.EntryViewModelProperty.Entries[newHook.Data.fromPosition];
            //    gotoMeanings();
            //}
        }

        private void llsWordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (llsWordList.SelectedItem == null)
            {
                return;
            }
            //App.EntryViewModelProperty.SelectedEntry = (DictionaryEntry)llsWordList.SelectedItem;
            //gotoMeanings();

            llsWordList.SelectedItem = null;
        }

    }
}