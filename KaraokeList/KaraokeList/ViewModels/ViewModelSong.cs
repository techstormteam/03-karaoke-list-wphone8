using KaraokeList.Models;
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
    public class ViewModelSong : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int nextIndex = 0;
        private ObservableCollection<ModelSong> entries;
        private ObservableCollection<AlphaKeyGroup<ModelSong>> entriesGroup;

        private bool dataLoaded = false;

        public ViewModelSong()
        {
        }

        public void LoadData()
        {
            if (!dataLoaded)
            {
                IEnumerable<ModelSong> list = ModelSong.QuerySongByVol(App.DbConnectionProperty, 1, "en");
                entries = new ObservableCollection<ModelSong>(list);

                //ModelSong song = new ModelSong();
                //entries = new ObservableCollection<ModelSong>();
                //song.SongId = 111111;
                //song.Name = "Tinh ta lo xa";
                //song.Lyric = "";
                //entries.Add(song);

                //entries = new ObservableCollection<ModelSong>();
                //song.SongId = 111111;
                //song.Name = "Tinh ta lo xa";
                //song.Lyric = "";
                //entries.Add(song);

                EntriesGroup = CreateGroup(0, entries.Count - 1);
            }


        }

        public ObservableCollection<AlphaKeyGroup<ModelSong>> CreateGroup(int fromPosition, int toPosition)
        {
            List<ModelSong> lookedEntries = entries.ToList().GetRange(fromPosition, toPosition - fromPosition + 1);
            return AlphaKeyGroup<ModelSong>.CreateGroups(lookedEntries,
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                (ModelSong s) => { return s.Name; }, false);
        }

        public void addMoreEntries(int entryNumber)
        {
            List<ModelSong> dictionaryEntries = new List<ModelSong>();
            for (int index = 0; index < entryNumber; index++)
            {
                dictionaryEntries.Add(entries.ElementAt(nextIndex));
                nextIndex++;
            }
            AlphaKeyGroup<ModelSong>.AddMoreItemsSorted(
                entriesGroup, dictionaryEntries,
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                (ModelSong s) => { return s.Name; });

        }

        public bool IsDataLoaded
        {
            get { return dataLoaded; }
        }

        public ObservableCollection<ModelSong> Entries
        {
            get { return entries; }
            set
            {
                NotifyPropertyChanging("Entries");
                entries = value;
                NotifyPropertyChanged("Entries");
            }
        }


        public ObservableCollection<AlphaKeyGroup<ModelSong>> EntriesGroup
        {
            get { return entriesGroup; }
            set
            {
                NotifyPropertyChanging("EntriesGroup");
                entriesGroup = value;
                NotifyPropertyChanged("EntriesGroup");
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
