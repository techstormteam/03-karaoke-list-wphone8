using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaraokeList.Models
{
    [Table("ZFAVORITE")]
    public class ModelFavorite : INotifyPropertyChanged, INotifyPropertyChanging
    {
        [PrimaryKey, Column("Z_PK"), AutoIncrement]
        public int Id { get; set; }
        [Column("Z_ENT")]
        public int Ent { get; set; }
        [Column("Z_OPT")]
        public int Opt { get; set; }
        [Column("ZROWID")]
        public int SongId { get; set; }
        [Column("ZSVOL")]
        public int Vol { get; set; }
        [Column("ZSABBR")]
        public string Abbr { get; set; }
        [Column("ZSLANGUAGE")]
        public string Language { get; set; }
        [Column("ZSLYRIC")]
        public string Lyric { get; set; }
        [Column("ZSLYRICCLEAN")]
        public string LyricClean { get; set; }
        [Column("ZSMANUFACTURE")]
        public string Manufacture { get; set; }
        [Column("ZSMETA")]
        public string Meta { get; set; }
        [Column("ZSMETACLEAN")]
        public string MetaClean { get; set; }
        [Column("ZSNAME")]
        public string Name { get; set; }
        [Column("ZSNAMECLEAN")]
        public string NameClean { get; set; }

        [SQLite.Ignore]
        public int SID { get; set; }

        private bool isChecked;

        [SQLite.Ignore]
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    NotifyPropertyChanging("IsChecked");
                    isChecked = value;
                    NotifyPropertyChanged("IsChecked");
                }
            }
        }

        public static IEnumerable<ModelFavorite> QueryFavorite(SQLiteConnection db)
        {
            return db.Query<ModelFavorite>("select * from ZFAVORITE");
        }

        public static void Delete(SQLiteConnection db, ModelFavorite fav)
        {
            db.Delete(fav);
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
