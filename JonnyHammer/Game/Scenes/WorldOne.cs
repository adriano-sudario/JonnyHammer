using JonnyHamer.Engine.Inputs;
using JonnyHammer.Engine;
using JonnyHammer.Engine.Helpers;
using JonnyHammer.Engine.Scenes;
using JonnyHammer.Game.Characters;
using JonnyHammer.Game.Environment;
using JonnyHammer.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using JonnyHamer.Engine.Managers;

namespace JonnyHammer.Game.Scenes
{
    public class WorldOne : Scene
    {
        private Jonny player;
        private KeyboardInput keyboard = new();

        public WorldOne()
        {
            var map = TiledLoader.Load("world_one");
            SpawnTiledLayers(map.Layers);
            SpawnTiledObjects(map.Objects);
        }

        void SpawnTiledLayers(Dictionary<string, TileLayer[]> layers)
        {
            foreach (var (layer, tiles) in layers)
                foreach (var (index, tile) in tiles.WithIndex())
                    switch (tile.Name)
                    {
                        case "bg":
                            Spawn(new MainBackground(tile.TextureName),  tile.Position, $"{layer}_bg_{index}");
                            break;

                        case "cloud":
                            SpawnClouds(layer, tile);
                            break;

                        default:
                            var scenery = new Scenery();
                            scenery.TextureName = tile.TextureName;
                            scenery.Source = tile.Source;
                            scenery.Width = tile.Width;
                            scenery.Height = tile.Height;
                            Spawn(scenery, tile.Position, $"{layer}_sc_{index}");
                            break;

                    }
        }

        void SpawnClouds(string layer, TileLayer tile)
        {
            var multiplier = tile.Amount - 1;
            var cloudRespawn = new Vector2((tile.Width * multiplier) - multiplier, tile.Position.Y);

            for (var i = 0; i < tile.Amount; i++)
                Spawn(new Cloud(tile.Speed, cloudRespawn),
                    (tile.Position + new Vector2((i * tile.Width) - 1, 0)),
                    $"{layer}_cloud_{i}");
        }

        private void SpawnTiledObjects(Dictionary<string, TiledObject[]> objects)
        {
            foreach (var (layer, tiles) in objects)
                foreach (var (index, tile) in tiles.WithIndex())
                    switch (layer)
                    {
                        case "one_way_blocks":
                        case "blocks":
                            Spawn(new Block($"floor_{layer}_{index}",tile.Width, tile.Height), 
                                new Vector2(tile.Position.X, tile.Position.Y + tile.Height));
                            break;

                        case "player_spawn":
                            player = new Jonny(respawnPosition: tile.Position);
                            Spawn(player, tile.Position, "Jonny");
                            break;

                        case "big_narutos":
                            Spawn(new BigNaruto((int)tile.MoveAmount), tile.Position,"NarutoRed");
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
                    Camera2D.Move(new Vector2(-5, 0));
                else if (keyboard.IsPressing(Keys.Right))
                    Camera2D.Move(new Vector2(5, 0));

                if (keyboard.IsPressing(Keys.Up))
                    Camera2D.Move(new Vector2(0, -5));
                else if (keyboard.IsPressing(Keys.Down))
                    Camera2D.Move(new Vector2(0, 5));

                return;
            }
            
            if (keyboard.HasPressed(Keys.F11))
            {
                Screen.ToggleFullScreen();
                return;
            }
            
            if (keyboard.IsPressing(Keys.OemPlus))
                Camera2D.ZoomUp(.02f);
            else if (keyboard.IsPressing(Keys.OemMinus))
                Camera2D.ZoomDown(.02f);

            if (keyboard.IsPressing(Keys.F12))
            {
                Camera2D.Center();
            }
            else
                Camera2D.Follow(player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera2D.GetViewTransformationMatrix());
            base.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}