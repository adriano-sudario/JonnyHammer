using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Globalization;
using TiledSharp;

namespace JonnyHammer.Engine.Helpers
{

    public class TiledObject
    {

        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float MoveAmount { get; set; }
        //public Vector2 LocalPosition { get; set; }
        //public int LocalWidth { get; set; }
        //public int LocalHeight { get; set; }
        //public float MoveAmount { get; set; }

        //public Vector2 Position => LocalPosition * Screen.Scale;
        //public float Width => LocalWidth * Screen.Scale;
        //public float Height => LocalHeight * Screen.Scale;

    }
    public class TileLayer
    {
        public float Speed { get; set; }
        public int Amount { get; set; }
        public int Index { get; set; }
        public TileSetAditionalInfo Info { get; set; }
        public Vector2 Source { get; set; }
        public string TextureName { get; set; }
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //public Vector2 LocalPosition { get; set; }
        //public int LocalWidth { get; set; }
        //public int LocalHeight { get; set; }

        //public Vector2 Position => LocalPosition * Screen.Scale;
        //public float Width => LocalWidth * Screen.Scale;
        //public float Height => LocalHeight * Screen.Scale;
    }

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
            var ret = new Dictionary<string, TileLayer[]>();

            foreach (var (index, layer) in map.TileLayers.AsIndexed())
            {
                var tiles = new List<TileLayer>();

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

                    var layerData = new TileLayer
                    {
                        Position = position,
                        Source = source,
                        Info = info,
                        Index = index,
                        Amount = amount,
                        Speed = speed,
                        TextureName = textureName,
                        Name = tileSet.Name,
                        Width = tileSet.TileWidth,
                        Height = tileSet.TileWidth,
                    };
                    tiles.Add(layerData);
                }

                ret.Add(layer.Name, tiles.ToArray());
            }

            return ret;
        }

        static Dictionary<string, TiledObject[]> LoadTiledObjects(TmxMap map)
        {
            var ret = new Dictionary<string, TiledObject[]>();

            foreach (var layer in map.ObjectGroups)
            {
                var objs = new List<TiledObject>();

                foreach (var item in layer.Objects)
                {

                    var moveAmount = 0f;
                    if (item.Properties.ContainsKey("MoveAmount"))
                        moveAmount = float.Parse(item.Properties["MoveAmount"], NumberStyles.Float, CultureInfo.InvariantCulture);

                    var obj = new TiledObject
                    {
                        Position = new Vector2((float)item.X, (float)item.Y),
                        Width = (int)(item.Width),
                        Height = (int)(item.Height),
                        MoveAmount = moveAmount,
                    };

                    objs.Add(obj);
                }

                ret.Add(layer.Name, objs.ToArray());
            }
            return ret;
        }
    }

    public static class Extensions
    {
        public static void Deconstruct<K, V>(this KeyValuePair<K, V> pair, out K key, out V value) =>
            (key, value) = (pair.Key, pair.Value);


        public static IEnumerable<(int, T)> AsIndexed<T>(this IEnumerable<T> @this)
        {
            var index = 0;
            foreach (var item in @this)
                yield return (index++, item);
        }
    }
}
