using JonnyHamer.Engine.Helpers;
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
using System.Collections.Generic;

namespace JonnyHammer.Game.Scenes
{
    public class WorldOne : Scene
    {
        private Jonny player;
        private KeyboardInput keyboard = new KeyboardInput();

        public WorldOne()
        {
            var map = TiledLoader.Load("world_one");
            SpawnTiledLayers(map.Layers);
            SpawnTiledObjects(map.Objects);
        }

        void SpawnTiledLayers(Dictionary<string, TileLayer[]> layers)
        {
            foreach (var (layer, tiles) in layers)
                foreach (var (index, tile) in tiles.AsIndexed())
                    switch (tile.Name)
                    {
                        case "bg":
                            Spawn<MainBackground>($"{layer}_bg_{index}", tile.Position * Screen.Scale,
                                s => s.TextureName = tile.TextureName);
                            break;

                        case "cloud":
                            SpawnClouds(layer, tile);
                            break;

                        default:
                            Spawn<Scenery>($"{layer}_sc_{index}", tile.Position * Screen.Scale,
                                s =>
                                {
                                    s.TextureName = tile.TextureName;
                                    s.Source = tile.Source;
                                    s.Width = (int)tile.Width;
                                    s.Height = (int)tile.Height;
                                });
                            break;

                    }
        }

        void SpawnClouds(string layer, TileLayer tile)
        {
            var multiplier = tile.Amount - 1;
            var cloudRespawn = new Vector2((tile.Width * multiplier) - multiplier, tile.Position.Y);
            Cloud lastCloud = null;

            for (var i = 0; i < tile.Amount; i++)
            {

                lastCloud = Spawn<Cloud>($"{layer}_cloud_{i}",
                    (tile.Position + new Vector2((i * tile.Width) - 1, 0)) * Screen.Scale,
                    c =>
                    {
                        c.Speed = tile.Speed;
                        c.Cloudrespawn = cloudRespawn;
                    });
            }
        }

        private void SpawnTiledObjects(Dictionary<string, TiledObject[]> objects)
        {
            foreach (var (layer, tiles) in objects)
                foreach (var (index, tile) in tiles.AsIndexed())
                    switch (layer)
                    {
                        case "one_way_blocks":
                        case "blocks":
                            Spawn<Block>(
                               $"floor_{layer}_{index}",
                               new Vector2(tile.Position.X, tile.Position.Y + tile.Height) * Screen.Scale,
                               f =>
                               {
                                   f.Width = (int)(tile.Width * Screen.Scale);
                                   f.Height = (int)(tile.Height * Screen.Scale);
                               });
                            break;

                        case "player_spawn":
                            player = Spawn<Jonny>(
                                "Jonny",
                                tile.Position * Screen.Scale,
                                j => j.RespawnPosition = tile.Position);
                            break;

                        case "big_narutos":
                            Spawn<BigNaruto>("NarutoRed", tile.Position * Screen.Scale,
                                bg => bg.MoveAmount = tile.MoveAmount);
                            break;
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
            else if (keyboard.HasPressed(Keys.F11))
            {
                Screen.ToggleFullScreen();
                return;
            }
            else if (keyboard.HasPressed(Keys.OemPlus))
                Screen.Scale += 0.1f;
            else if (keyboard.HasPressed(Keys.OemMinus))
                Screen.Scale -= 0.1f;

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