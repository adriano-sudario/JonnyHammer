using JonnyHamer.Engine.Managers;
using JonnyHammer.Engine;
using JonnyHammer.Game.Scenes;
using Microsoft.Xna.Framework;

namespace JonnyHammer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Core
    {
        public Main() : base(startOnFullScreen: false)
        {

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            //Screen.ChangeResolution(1920, 1080);
            //Screen.ChangeResolution(1280, 720);
            //Screen.Scale = 3;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // SceneManager.AddScene("world_one", new WorldOne());
            SceneManager.AddScene("nujustu_scene", new Nujutsu());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
