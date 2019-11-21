using Microsoft.Xna.Framework;
using System;
using TiledSharp;

namespace JonnyHammer.Engine.Helpers
{
    public class TileSetAditionalInfo
    {
        public Vector2 MapPosition { get; set; }
        public Vector2 ImageSource{ get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public static class Tiled
    {
        public static TileSetAditionalInfo GetTileSetAditionalInfo(this TmxMap map, TmxLayerTile tile, 
            out TmxTileset tileSet)
        {
            tileSet = map.GetTileSet(tile);

            int tileSetColumns = tileSet.Columns.Value == 0 ? 1 : tileSet.Columns.Value;
            int tileSetWidth = tileSetColumns > 0 ?
                tileSetColumns * map.TileWidth :
                map.TileWidth;
            int tileSetHeight = tileSetColumns > 0 ?
                (tileSet.TileCount.Value / tileSetColumns) * map.TileWidth :
                map.TileWidth;
            int tileSetRows = tileSetHeight / tileSet.TileHeight;

            Vector2 position = new Vector2(tile.X * map.TileWidth,
                ((tile.Y + 1) * map.TileHeight) - tileSet.TileHeight);

            int frame = tile.Gid - tileSet.FirstGid;
            int column = frame <= tileSetColumns ? frame : frame % tileSetColumns;
            int row = (int)Math.Floor((float)frame / tileSetColumns);

            Vector2 source = new Vector2(column * tileSet.TileWidth,
                row * tileSet.TileHeight);

            return new TileSetAditionalInfo()
            {
                MapPosition = position,
                ImageSource = source,
                Columns = tileSetColumns,
                Rows = tileSetRows,
                Width = tileSetWidth,
                Height = tileSetHeight
            };
        }

        public static TmxTileset GetTileSet(this TmxMap map, TmxLayerTile tile)
        {
            TmxTileset tileSet = null;

            for (int i = 0; i < map.Tilesets.Count; i++)
            {
                bool isHigher = tile.Gid >= map.Tilesets[i].FirstGid;

                if (isHigher && i >= map.Tilesets.Count - 1)
                    tileSet = map.Tilesets[i];
                else if (isHigher && tile.Gid < map.Tilesets[i + 1].FirstGid)
                    tileSet = map.Tilesets[i];
            }

            return tileSet;
        }
    }
}
