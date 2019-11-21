using JonnyHamer.Engine.Inputs;
using JonnyHamer.Engine.Manipulators;
using JonnyHammer.Engine.Helpers;
using JonnyHammer.Engine.Scenes;
using JonnyHammer.Game.Characters;
using JonnyHammer.Game.Environment;
using JonnyHammer.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Globalization;
using TiledSharp;

namespace JonnyHammer.Game.Scenes
{
    public class WorldOne : Scene
    {
        private TmxMap map;
        private Jonny player;
        private KeyboardInput keyboard = new KeyboardInput();

        public WorldOne()
        {
            map = new TmxMap("Content/Maps/world_one.tmx");

            SpawnTiledLayers(map);
            SpawnTiledObjects(map);
        }

        private void SpawnTiledLayers(TmxMap map)
        {
            foreach (var layer in map.TileLayers)
            {
                int index = 0;

                foreach (var tile in layer.Tiles)
                {
                    if (tile.Gid == 0)
                        continue;

                    TmxTileset tileSet = null;
                    var info = map.GetTileSetAditionalInfo(tile, out tileSet);
                    var position = info.MapPosition;
                    var source = info.ImageSource;

                    switch (tileSet.Name)
                    {
                        case "bg":
                            Spawn<MainBackground>($"{layer.Name}_{++index}", position,
                                s => s.TextureName = tileSet.Properties["TextureName"]);
                            break;

                        case "cloud":
                            int amount = int.Parse(layer.Properties["Amount"]);
                            int multiplier = amount - 1;
                            Vector2 cloudRespawn =
                                new Vector2((tileSet.TileWidth * multiplier) - multiplier, position.Y);
                            Cloud lastCloud = null;

                            for (int i = 0; i < amount; i++)
                            {
                                Vector2 cloudPosition = i == 0 ? position :
                                    new Vector2(lastCloud.Transform.Position.X + tileSet.TileWidth - 1, position.Y);

                                lastCloud = Spawn<Cloud>($"{layer.Name}_{++index}",
                                    position + new Vector2((i * tileSet.TileWidth) - 1, 0),
                                    c =>
                                    {
                                        c.OnDisappear += () => c.Transform.MoveTo(cloudRespawn);
                                        c.Speed = float.Parse(layer.Properties["Speed"], CultureInfo.InvariantCulture);
                                    });
                            }
                            break;

                        default:
                            Spawn<Scenery>($"{layer.Name}_{++index}", position,
                                s =>
                                {
                                    s.TextureName = tileSet.Properties["TextureName"];
                                    s.Source = source;
                                    s.Width = tileSet.TileWidth;
                                    s.Height = tileSet.TileHeight;
                                });
                            break;
                    }
                }
            }
        }

        private void SpawnTiledObjects(TmxMap map)
        {
            foreach (var layer in map.ObjectGroups)
            {
                int index = 0;

                foreach (var item in layer.Objects)
                {
                    switch (layer.Name)
                    {
                        case "one_way_blocks":
                        case "blocks":
                            Spawn<Block>($"floor_{++index}", new Vector2((float)item.X, (float)(item.Y + item.Height)),
                                f =>
                                {
                                    f.Width = (int)item.Width;
                                    f.Height = (int)item.Height;
                                });
                            break;

                        case "player_spawn":
                            player = Spawn<Jonny>("Narutao", new Vector2((float)item.X, (float)item.Y),
                                j => j.RespawnPosition = new Vector2((float)item.X, (float)item.Y));
                            break;

                        case "big_narutos":
                            Spawn<BigNaruto>("NarutoRed", new Vector2((float)item.X, (float)item.Y),
                                bg => bg.MoveAmount = float.Parse(item.Properties["MoveAmount"], CultureInfo.InvariantCulture));
                            break;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            keyboard.Update();

            if (keyboard.IsPressing(Keys.LeftControl))
            {
                if (keyboard.IsPressing(Keys.Left))
                    Camera.GoTo(new Vector2(Camera.Position.X + 3, Camera.Position.Y));
                else if (keyboard.IsPressing(Keys.Right))
                    Camera.GoTo(new Vector2(Camera.Position.X - 3, Camera.Position.Y));

                if (keyboard.IsPressing(Keys.Up))
                    Camera.GoTo(new Vector2(Camera.Position.X, Camera.Position.Y + 3));
                else if (keyboard.IsPressing(Keys.Down))
                    Camera.GoTo(new Vector2(Camera.Position.X, Camera.Position.Y - 3));

                return;
            }
            else if (keyboard.HasPressed(Keys.Back))
            {
                player.Respawn();
                return;
            }
            
            Camera.Follow(player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.ViewMatrix);
            base.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}