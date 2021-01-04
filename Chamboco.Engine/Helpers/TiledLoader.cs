using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Globalization;
using TiledSharp;

namespace Chamboco.Engine.Helpers
{
    public record TiledObject
    (
        Vector2 Position,
        int Width,
        int Height,
        float MoveAmount
    );

    public record TileLayer
    (
        Vector2 Position,
        Vector2 Source,
        TileSetAditionalInfo Info,
        int Index,
        int Amount,
        float Speed,
        string TextureName,
        string Name,
        int Width,
        int Height
    );

    public class TiledData
    {
        public Dictionary<string, TileLayer[]> Layers { get; }
        public Dictionary<string, TiledObject[]> Objects { get; }

        public TiledData(Dictionary<string, TiledObject[]> objects, Dictionary<string, TileLayer[]> layers) =>
            (Layers, Objects) = (layers, objects);
    }


    public static class TiledLoader
    {
        public static TiledData Load(string tmxFileName)
        {
            var map = new TmxMap($"Content/Maps/{tmxFileName}.tmx");
            var layers = LoadTiledLayers(map);
            var objects = LoadTiledObjects(map);

            return new TiledData(objects, layers);
        }

        static Dictionary<string, TileLayer[]> LoadTiledLayers(TmxMap map)
        {
            Dictionary<string, TileLayer[]> ret = new();

            foreach (var (index, layer) in map.TileLayers.WithIndex())
            {
                List<TileLayer> tiles = new();

                foreach (var tile in layer.Tiles)
                {
                    if (tile.Gid == 0)
                        continue;

                    var info = map.GetTileSetAditionalInfo(tile, out var tileSet);
                    var position = info.MapPosition;
                    var source = info.ImageSource;

                    var textureName = tileSet.Properties["TextureName"];

                    var amount = 0;
                    if (layer.Properties.ContainsKey("Amount"))
                        amount = int.Parse(layer.Properties["Amount"]);

                    var speed = 0f;
                    if (layer.Properties.ContainsKey("Speed"))
                        speed = float.Parse(layer.Properties["Speed"], CultureInfo.InvariantCulture);

                    TileLayer layerData = new (
                        position,
                        source,
                        info,
                        index,
                        amount,
                        speed,
                        textureName,
                        tileSet.Name,
                        tileSet.TileWidth,
                        tileSet.TileHeight);
                    tiles.Add(layerData);
                }

                ret.Add(layer.Name, tiles.ToArray());
            }

            return ret;
        }

        static Dictionary<string, TiledObject[]> LoadTiledObjects(TmxMap map)
        {
            Dictionary<string, TiledObject[]> ret = new();

            foreach (var layer in map.ObjectGroups)
            {
                List<TiledObject> objs = new ();

                foreach (var item in layer.Objects)
                {

                    var moveAmount = 0f;
                    if (item.Properties.ContainsKey("MoveAmount"))
                        moveAmount = float.Parse(item.Properties["MoveAmount"], NumberStyles.Float, CultureInfo.InvariantCulture);

                    var obj = new TiledObject (
                        new Vector2((float)item.X, (float)item.Y),
                        (int)item.Width,
                        (int)item.Height,
                        moveAmount
                    );

                    objs.Add(obj);
                }

                ret.Add(layer.Name, objs.ToArray());
            }
            return ret;
        }
    }

}
