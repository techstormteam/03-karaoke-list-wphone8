using KaraokeList.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;

namespace KaraokeList.ViewModels
{
    public class ViewModelFavorite : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private ModelFavorite selectedEntry;
        private ModelFavorite lastSelectedEntry;

        private ObservableCollection<ModelFavorite> entries = new ObservableCollection<ModelFavorite>();
        private ObservableCollection<AlphaKeyGroup<ModelFavorite>> entriesGroup;

        public ViewModelFavorite()
        {
        }

        public void CreateGroup()
        {
            EntriesGroup = createGroup();
        }

        public void LoadData()
        {
            IEnumerable<ModelFavorite> list = ModelFavorite.QueryFavorite(App.DbConnectionProperty);
            entries = new ObservableCollection<ModelFavorite>(list);
        }

        private ObservableCollection<AlphaKeyGroup<ModelFavorite>> createGroup()
        {
            return AlphaKeyGroup<ModelFavorite>.CreateGroups(entries,
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                (ModelFavorite s) => { return s.Name; }, true);
        }

        public void addFavoriteSong(ModelSong song)
        {
            ModelFavorite fav = new ModelFavorite();
            fav.Language = song.Language;
            fav.Lyric = song.Lyric;
            fav.LyricClean = song.LyricClean;
            fav.Manufacture = song.Manufacture;
            fav.Meta = song.Meta;
            fav.MetaClean = song.MetaClean;
            fav.Name = song.Name;
            fav.NameClean = song.NameClean;
            fav.Opt = song.Opt;
            fav.SongId = song.SongId;
            fav.Vol = song.Vol;

            App.DbConnectionProperty.Insert(fav);
            song.Favourite = 1;
            App.DbConnectionProperty.Update(song);
        }

        public void RandomFavouriteList()
        {
            Random random = new Random();
            int halfOfHeadwords = entries.Count / 2 - 1;
            for (int index = 0; index < halfOfHeadwords; index++)
			{
                int index2Swap = random.Next(halfOfHeadwords);
                swap(index, index2Swap);
			}
            
        }

        private void swap(int firstPosition, int secondPosition)
        {
            ModelFavorite temp = entries[firstPosition];
            entries[firstPosition] = entries[secondPosition];
            entries[secondPosition] = temp;
        }

        public void AddHeadwordToFavouritesList(ModelFavorite entry)
        {
            if (!entries.Contains(entry))
            {
                entries.Add(entry);
            }
        }

        public ObservableCollection<AlphaKeyGroup<ModelFavorite>> EntriesGroup
        {
            get { return entriesGroup; }
            set
            {
                NotifyPropertyChanging("EntriesGroup");
                entriesGroup = value;
                NotifyPropertyChanged("EntriesGroup");
            }
        }

        public ObservableCollection<ModelFavorite> Entries
        {
            get { return entries; }
            set
            {
                NotifyPropertyChanging("Entries");
                entries = value;
                NotifyPropertyChanged("Entries");
            }
        }

        public ModelFavorite LastSelectedEntry
        {
            get { return lastSelectedEntry; }
            set
            {
                NotifyPropertyChanging("LastSelectedEntry");
                lastSelectedEntry = value;
                NotifyPropertyChanged("LastSelectedEntry");
            }
        }

        public ModelFavorite SelectedEntry
        {
            get { return selectedEntry; }
            set
            {
                NotifyPropertyChanging("SelectedEntry");
                selectedEntry = value;
                NotifyPropertyChanged("SelectedEntry");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}
