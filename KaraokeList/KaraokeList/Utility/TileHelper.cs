using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class TileHelper
    {
        public static void ResetIconicTile()
        {
            IconicTileData oIcontile = new IconicTileData();
            oIcontile.Count = 0;
            oIcontile.IconImage = new Uri("/Assets/Tiles/GiftIconImage.png", UriKind.Relative);
            oIcontile.SmallIconImage = new Uri("/Assets/Tiles/GiftSmallIconImage.png", UriKind.Relative);

            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault();
            tile.Update(oIcontile);
        }
    }
}
