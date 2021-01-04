using Microsoft.Xna.Framework;
using System;
using TiledSharp;

namespace Chamboco.Engine.Helpers
{
    public record TileSetAditionalInfo
    (
        Vector2 MapPosition,
        Vector2 ImageSource,
        int Column,
        int Row,
        int Widt,
        int Heigh
    );

    public static class Tiled
    {
        public static TileSetAditionalInfo GetTileSetAditionalInfo(this TmxMap map, TmxLayerTile tile,
            out TmxTileset tileSet)
        {
            tileSet = map.GetTileSet(tile);

            var tileSetColumns = tileSet.Columns.Value == 0 ? 1 : tileSet.Columns.Value;
            var tileSetWidth = tileSetColumns > 0 ?
                tileSetColumns * map.TileWidth :
                map.TileWidth;
            var tileSetHeight = tileSetColumns > 0 ?
                (tileSet.TileCount.Value / tileSetColumns) * map.TileWidth :
                map.TileWidth;
            var tileSetRows = tileSetHeight / tileSet.TileHeight;

            var position = new Vector2(tile.X * map.TileWidth,
                ((tile.Y + 1) * map.TileHeight) - tileSet.TileHeight);

            var frame = tile.Gid - tileSet.FirstGid;
            var column = frame <= tileSetColumns ? frame : frame % (float)tileSetColumns;
            var row = (int)Math.Floor((float)frame / tileSetColumns);

            var source = new Vector2(column * tileSet.TileWidth,
                row * tileSet.TileHeight);

            return new TileSetAditionalInfo(position, source, tileSetColumns, tileSetRows, tileSetWidth, tileSetHeight);
        }

        public static TmxTileset GetTileSet(this TmxMap map, TmxLayerTile tile)
        {
            TmxTileset tileSet = null;

            for (var i = 0; i < map.Tilesets.Count; i++)
            {
                var isHigher = tile.Gid >= map.Tilesets[i].FirstGid;

                if (isHigher && i >= map.Tilesets.Count - 1)
                    tileSet = map.Tilesets[i];
                else if (isHigher && tile.Gid < map.Tilesets[i + 1].FirstGid)
                    tileSet = map.Tilesets[i];
            }

            return tileSet;
        }
    }
}
