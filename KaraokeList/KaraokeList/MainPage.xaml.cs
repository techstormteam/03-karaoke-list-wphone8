using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using KaraokeList.Resources;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;

namespace KaraokeList
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //gotoPage("HistoryPage.xaml");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var askforReview = (bool)IsolatedStorageSettings.ApplicationSettings["askforreview"];
            if (askforReview)
            {
                //make sure we only ask once! 
                IsolatedStorageSettings.ApplicationSettings["askforreview"] = false;
                var ok = MessageBox.Show("Cảm ơn bạn đã sử dụng KaraokeList, Hãy đánh giá 5 sao cho ứng dụng và comment chức năng mà bạn muốn ở phiên bản kế tiếp.", "Hãy đánh giá 5 sao!", MessageBoxButton.OKCancel);
                if (ok == MessageBoxResult.OK)
                {
                    var marketplaceReviewTask = new MarketplaceReviewTask();
                    marketplaceReviewTask.Show();
                }
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //if (_recoEnabled)
            //{
            //    stopVoiceRecognition();
            //    e.Cancel = true;
            //    base.OnBackKeyPress(e);
            //    return;
            //}

            string caption = "Thoát";
            string message = "Bạn muốn thoát khỏi KaraokeList?";
            e.Cancel = MessageBoxResult.Cancel == MessageBox.Show(message, caption,
            MessageBoxButton.OKCancel);

            base.OnBackKeyPress(e);

        }

        private void PageMain_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the data context of the listbox control to the sample data
            llsWordList.ItemsSource = App.ViewModelSongProperty.EntriesGroup;
        }
    }
}