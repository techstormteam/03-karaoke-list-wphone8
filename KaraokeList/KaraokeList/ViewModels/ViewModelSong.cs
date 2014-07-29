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
        public int Vol { get; set; }
        public string Language { get; set; }
        private ObservableCollection<ModelSong> entriesVol;
        private ObservableCollection<ModelSong> entriesLanguage;
        private ObservableCollection<ModelSong> entriesSearch;
        private ObservableCollection<ModelSong> entries;
        private ObservableCollection<AlphaKeyGroup<ModelSong>> entriesGroup;
        private ModelSong selectedEntry;
        private SpeechRecognizer recognizer = getRecognizer();

        private bool dataLoaded = false;

        public bool NeedApplyChanges { get; set; }

        public ViewModelSong()
        {
            NeedApplyChanges = true;
        }

        public void LoadData()
        {
            
            if (!dataLoaded)
            {
                IEnumerable<ModelSong> list = ModelSong.QueryVol(App.DbConnectionProperty);
                entriesVol = new ObservableCollection<ModelSong>(list);
                entriesVol.RemoveAt(entriesVol.Count - 1);
                if (entriesVol != null && entriesVol.Count > 0) {
                    Vol = entriesVol.First<ModelSong>().Vol;
                }

                list = ModelSong.QueryLanguage(App.DbConnectionProperty);
                entriesLanguage = new ObservableCollection<ModelSong>(list);
                //entriesLanguage.RemoveAt(entriesLanguage.Count - 1);
                if (entriesLanguage != null && entriesLanguage.Count > 0)
                {
                    Language = entriesLanguage.First<ModelSong>().Language;
                }

                
                

                dataLoaded = true;
            }

            if (NeedApplyChanges)
            {
                reloadSongList();
                NeedApplyChanges = false;
            }

        }

        public ObservableCollection<AlphaKeyGroup<ModelSong>> reloadSongList()
        {
            IEnumerable<ModelSong> list = ModelSong.QuerySongByVol(App.DbConnectionProperty, Vol, Language);
            entries = new ObservableCollection<ModelSong>(list);
            EntriesGroup = CreateGroup(0, entries.Count - 1);
            EntriesSearch = new ObservableCollection<ModelSong>(list);
            return EntriesGroup;
        }

        public void searchSong(string keys)
        {
            IEnumerable<ModelSong> list = ModelSong.QuerySongByCri(App.DbConnectionProperty, Vol, Language, keys);
            
            EntriesSearch = new ObservableCollection<ModelSong>(list);
        }

        public ObservableCollection<AlphaKeyGroup<ModelSong>> CreateGroup(int fromPosition, int toPosition)
        {
            List<ModelSong> lookedEntries = entries.ToList().GetRange(fromPosition, toPosition - fromPosition + 1);
            return AlphaKeyGroup<ModelSong>.Create2Groups(lookedEntries,
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                (ModelSong s) => { return s.Name; }, false, Vol);
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

        public ObservableCollection<ModelSong> EntriesData
        {
            get { return entries; }
            set
            {
                NotifyPropertyChanging("EntriesData");
                entries = value;
                NotifyPropertyChanged("EntriesData");
            }
        }

        public ObservableCollection<ModelSong> EntriesVol
        {
            get { return entriesVol; }
            set
            {
                NotifyPropertyChanging("EntriesVol");
                entriesVol = value;
                NotifyPropertyChanged("EntriesVol");
            }
        }

        public ObservableCollection<ModelSong> EntriesLanguage
        {
            get { return entriesLanguage; }
            set
            {
                NotifyPropertyChanging("EntriesLanguage");
                entriesLanguage = value;
                NotifyPropertyChanged("EntriesLanguage");
            }
        }

        public ObservableCollection<ModelSong> EntriesSearch
        {
            get { return entriesSearch; }
            set
            {
                NotifyPropertyChanging("EntriesSearch");
                entriesSearch = value;
                NotifyPropertyChanged("EntriesSearch");
            }
        }

        public ModelSong SelectedEntry
        {
            get { return selectedEntry; }
            set
            {
                NotifyPropertyChanging("SelectedEntry");
                selectedEntry = value;
                NotifyPropertyChanged("SelectedEntry");
            }
        }

        private static SpeechRecognizer getRecognizer()
        {
            SpeechRecognizer speechRecognizer = new SpeechRecognizer();
            speechRecognizer.Grammars.AddGrammarFromPredefinedType("Words", SpeechPredefinedGrammar.WebSearch);
            return speechRecognizer;
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

        public SpeechRecognizer Recognizer
        {
            get { return recognizer; }
            set
            {
                NotifyPropertyChanging("Recognizer");
                recognizer = value;
                NotifyPropertyChanged("Recognizer");
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
