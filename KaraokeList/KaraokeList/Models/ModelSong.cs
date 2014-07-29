using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaraokeList.Models
{
    [Table("ZSONG")]
    public sealed class ModelSong
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
        [Column("favourite")]
        public int Favourite { get; set; }

        public static IEnumerable<ModelSong> QuerySongByVol(SQLiteConnection db, int vol, string language)
        {
            return db.Query<ModelSong>("select * from ZSONG where ZSVOL <= ? and ZSLANGUAGE = ?", vol, language);
        }

        public static IEnumerable<ModelSong> QuerySongByCri(SQLiteConnection db, int vol, string language , string search)
        {
            return db.Query<ModelSong>("select * from ZSONG where ZSVOL <= ? and ZSLANGUAGE = ? and (ZSABBR like ? or ZSLYRIC like ? or ZSLYRICCLEAN like ? or ZSMETA like ? or ZSMETACLEAN like ? or ZSNAME like ? or ZSNAMECLEAN like ?)", vol, language, search, search, search, search, search, search, search);
            //return db.Query<ModelSong>("select * from ZSONG where ZSVOL <= ? and ZSLANGUAGE = ?", vol, language);
        }

        public static IEnumerable<ModelSong> QueryVol(SQLiteConnection db)
        {
            return db.Query<ModelSong>("select ZSVOL from ZSONG group by ZSVOL order by ZSVOL desc");
        }

        public static IEnumerable<ModelSong> QueryLanguage(SQLiteConnection db)
        {
            return db.Query<ModelSong>("select ZSLANGUAGE from ZSONG group by ZSLANGUAGE order by ZSLANGUAGE desc");
        }

        public static IEnumerable<ModelSong> RemoveFavorite(SQLiteConnection db, int songId)
        {
            return db.Query<ModelSong>("update ZSONG set favourite = 0 where ZROWID = ?", songId);
        }
    }

}
