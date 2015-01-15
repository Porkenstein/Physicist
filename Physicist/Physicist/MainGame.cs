﻿namespace Physicist
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using FarseerPhysics;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Actors;
    using Physicist.Controls;
    using Physicist.Extensions;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        private GraphicsDeviceManager graphics;
        private FCCSpritebatch spriteBatch;

        public MainGame()
            : base()
        {
            this.graphics = new GraphicsDeviceManager(this);
        }

        public static GraphicsDevice GraphicsDev 
        { 
            get; 
            private set; 
        }

        public static ContentManager ContentManager
        {
            get;
            private set;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ContentController.Instance.Initialize(this.Content, "Content");
            ContentController.Instance.LoadContent<SpriteFont>("MenuFont", "Pericles6");

            MainGame.GraphicsDev = this.GraphicsDevice;
            MainGame.ContentManager = this.Content;
            this.spriteBatch = new FCCSpritebatch(this.GraphicsDevice);
            AssetCreator.Instance.Initialize(this.GraphicsDevice);
            ScreenManager.Initialize(this.GraphicsDevice);
            ScreenManager.Quit += this.RequestQuit;
           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            ScreenManager.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
/*      if (gameTime != null)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }

                MainGame.World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

                foreach (var actor in MainGame.actors)
                {
                    Player player = actor as Player;
                    player.PlayerCamera = this.camera;
                    if (player != null)
                    {
                        this.camera.Following = player;
                        player.RotateWorld(.001f);
                        player.Update(gameTime, Keyboard.GetState());
                    }
                    else
                    {
                        actor.Update(gameTime);
                    }
                }

                // TODO: Add your update logic here
                this.camera.CenterOnFollowing();
                MainGame.map.Update(gameTime);
            }
*/
            ScreenManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            ScreenManager.Draw(this.spriteBatch);
            base.Draw(gameTime);
        }

        protected virtual void RequestQuit(object sender, EventArgs e)
        {
            this.Exit();
        }
    }
}
