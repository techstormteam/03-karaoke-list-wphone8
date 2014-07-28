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
using KaraokeList.Models;
using SQLite;
using System.Windows.Media;

namespace KaraokeList
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<ModelFavorite> chosenEntries = new List<ModelFavorite>();
        private bool selectMode = false;
        private ApplicationBarIconButton btnSearch;
        private ApplicationBarIconButton btnSelect;
        private ApplicationBarIconButton btnDelete;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            InitializeAppBar();
            updateList();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void updateList()
        {
            App.ViewModelFavoriteProperty.CreateGroup();
            llsWordList.ItemsSource = App.ViewModelFavoriteProperty.EntriesGroup;
        }

        private void llsSongFavoriteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (llsSongFavoriteList.SelectedItem == null)
            {
                return;
            }
            ModelFavorite entry = (ModelFavorite)llsSongFavoriteList.SelectedItem;

            if (selectMode)
            {
                entry.IsChecked = !entry.IsChecked;
                updatePageAfterCheckOrUncheck();
            }
            else
            {
                App.ViewModelFavoriteProperty.SelectedEntry = entry;
                //gotoMeanings("FavouritesMeaningPage.xaml");
            }

            llsSongFavoriteList.SelectedItem = null;
        }

        private void updatePageAfterCheckOrUncheck()
        {
            ModelFavorite entry = (ModelFavorite)llsWordList.SelectedItem;
            if (entry.IsChecked)
            {
                chosenEntries.Add(entry);
            }
            else
            {
                chosenEntries.Remove(entry);
            }

            if (chosenEntries.Count > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
        }

        private void InitializeAppBar()
        {
            //ApplicationBar = new ApplicationBar();
            
            btnSearch = new ApplicationBarIconButton();
            btnSearch.IconUri = new Uri("/Assets/ApplicationBar/appbar.search.png", UriKind.Relative);
            btnSearch.Text = "tìm kiếm";
            btnSearch.Click += new EventHandler(btnSearch_Click);

            btnSelect = new ApplicationBarIconButton();
            btnSelect.IconUri = new Uri("/Assets/ApplicationBar/appbar.select.png", UriKind.Relative);
            btnSelect.Text = "chọn";
            btnSelect.Click += new EventHandler(btnSelect_Click);

            btnDelete = new ApplicationBarIconButton();
            btnDelete.IconUri = new Uri("/Assets/ApplicationBar/appbar.delete.rest.png", UriKind.Relative);
            btnDelete.Text = "xóa";
            btnDelete.Click += new EventHandler(btnDelete_Click);

            showAppBarSongList();

            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.IsVisible = true;
            ApplicationBar.BackgroundColor = Colors.Black;
        }

        private void llsWordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (llsWordList.SelectedItem == null)
            {
                return;
            }
            App.ViewModelSongProperty.SelectedEntry = (ModelSong)llsWordList.SelectedItem;

            gotoPage("/Views/PageSongDetail.xaml");
            llsWordList.SelectedItem = null;
        }

        private void showAppBarSongList()
        {
            ApplicationBar.Buttons.Clear();
            ApplicationBar.Buttons.Add(btnSearch);
            //ApplicationBar.Buttons.Remove(btnSelect);
            //ApplicationBar.Buttons.Remove(btnDelete);
        }


        private void showAppBarAbout()
        {
            ApplicationBar.Buttons.Clear();
        }

        private void showAppBarFavoriteList()
        {
            ApplicationBar.Buttons.Clear();
            //ApplicationBar.Buttons.Remove(btnSearch);
            ApplicationBar.Buttons.Add(btnSelect);
            ApplicationBar.Buttons.Add(btnDelete);
        }

        private void gotoPage(string pageName)
        {
            this.NavigationService.Navigate(new Uri("/" + pageName, UriKind.RelativeOrAbsolute));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            gotoPage("/Views/PageSongSearch.xaml");
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            selectMode = true;
            btnDelete.IsEnabled = false;
            btnSelect.IsEnabled = true;
            showAllCheckBoxes();
        }

        private void showAllCheckBoxes()
        {
            setVisibilityAllCheckBoxes(llsSongFavoriteList, Visibility.Visible);
        }

        private void setVisibilityAllCheckBoxes(DependencyObject targeted_control, Visibility visibility)
        {
            var count = VisualTreeHelper.GetChildrenCount(targeted_control);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(targeted_control, i);
                    if (child is CheckBox) // Only search for ChecBoxes
                    {
                        CheckBox checkBox = (CheckBox)child;
                        checkBox.Visibility = visibility;
                    }
                    else
                    {
                        setVisibilityAllCheckBoxes(child, visibility);
                    }
                }
            }
            else
            {
                return;
            }
        }

        private void deleteAllEntries()
        {
            string caption = "Bạn có chắc không?";
            string message = "Sau thao tác này, toàn bộ lịch sử tra từ của bạn sẽ bị xóa hết.";
            MessageBoxResult result = MessageBox.Show(message, caption,
            MessageBoxButton.OKCancel);
            if (MessageBoxResult.OK == result)
            {
                App.ViewModelFavoriteProperty.Entries.Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (chosenEntries.Count == 0)
            {
                deleteAllEntries();
            }
            else
            {
                deleteEntries(chosenEntries);
                OnBackKeyPress(new System.ComponentModel.CancelEventArgs());
            }
            chosenEntries.Clear();
            updateList();
        }

        private void deleteEntries(List<ModelFavorite> chosenEntries)
        {
            foreach (ModelFavorite entry in chosenEntries)
            {
                App.ViewModelFavoriteProperty.Entries.Remove(entry);
            }
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
            llsSongFavoriteList.ItemsSource = App.ViewModelFavoriteProperty.Entries;
        }

        private void pviFavoriteList_GotFocus(object sender, RoutedEventArgs e)
        {
            showAppBarFavoriteList();
        }

        private void pviSongList_GotFocus(object sender, RoutedEventArgs e)
        {
            showAppBarSongList();
        }

        private void Pivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            if (pviFavoriteList == e.Item)
            {
                showAppBarFavoriteList();
            }
            else if (pviSongList == e.Item)
            {
                showAppBarSongList();
            }
            else if (pviAbout == e.Item)
            {
                showAppBarAbout();
            }
            
        }
    }
}