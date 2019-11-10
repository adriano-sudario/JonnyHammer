using JonnyHamer.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JonnyHammer.Engine
{
    public class Core : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static bool isFullScreen;
        private static Texture2D debugTexture;
        public static Texture2D GetDebugTexture(Color color)
        {
            var texture = new Texture2D(debugTexture.GraphicsDevice, 1, 1);
            if (color.A > 150) color.A = 150;
            texture.SetData(new[] { color });
            return texture;
        }


        public static Color Color { get; set; }
        public static Action Quit { get; private set; }
        public static bool IsFullScreen
        {
            get => isFullScreen;
            set
            {
                isFullScreen = value;
                Screen.Adjust(isFullScreen);
            }
        }

        public Core(bool startOnFullScreen = false, Color? color = null)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            isFullScreen = startOnFullScreen;

            if (color == null)
                color = Color.LightGreen;

            Color = color.Value;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Quit = Exit;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Loader.Initialize(Content);
            Screen.Initialize(graphics, GraphicsDevice);
            IsFullScreen = isFullScreen;
            debugTexture = new Texture2D(GraphicsDevice, 1, 1);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color);
            SceneManager.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}