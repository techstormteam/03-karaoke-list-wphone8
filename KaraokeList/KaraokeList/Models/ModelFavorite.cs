using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaraokeList.Models
{
    [Table("ZFAVORITE")]
    public class ModelFavorite : Model<ModelSong>
    {
        [PrimaryKey, Column("Z_PK"), AutoIncrement]
        public int Id { get; set; }
        [Column("Z_ENT")]
        public string Ent { get; set; }
        [Column("Z_OPT")]
        public string Opt { get; set; }
        [Column("ZROWID")]
        public string SongId { get; set; }
        [Column("ZSVOL")]
        public string Vol { get; set; }
        [Column("ZSABBR")]
        public string abbr { get; set; }
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

        public static IEnumerable<ModelSong> QueryFavorite(SQLiteConnection db, int vol, string language)
        {
            return db.Query<ModelSong>("select * from ZSONG where ZSVOL = ?", vol);
        }
    }

}
