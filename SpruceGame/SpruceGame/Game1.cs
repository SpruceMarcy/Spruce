using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;///MB: Imports dictionaries
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    /// <summary>
    /// MB: This is a way of keeping track of what the game needs to do when it next updates.
    /// </summary>
    enum GameState
    {
        MainMenu = 0,
        NewGame = 1,
        InGame = 2
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;//Monogame
        SpriteBatch spriteBatch;//Monogame

        //--------MB: Declare variables here that are global to the game--------
        Dictionary<string, Texture2D> Textures;//MB: This variable stores all textures, accessible with an identifier
        GameState GameState;//MB: This variable keeps track of whether the game is live or not etc.

        //---------------------------------------------------------------------

        public Game1()
        {
            //MB: don't know when this is called; best not touch it
            graphics = new GraphicsDeviceManager(this);//Monogame
            Content.RootDirectory = "Content";//Monogame
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();//Monogame

            //--------MB: Do anything that happens at startup here that doesn't involve loading--------
            graphics.PreferredBackBufferHeight = 1080;//MB: Window Height
            graphics.PreferredBackBufferWidth = 1920;//MB: Window Width
            //graphics.ToggleFullScreen();//MB: IF YOU CRASH IN FULL SCREEN YOU DIE IN REAL LIFE
            graphics.ApplyChanges();//MB: Updates the screen size
            GameState = GameState.MainMenu;//MB: This means that the game will start at the main menu

            //----------------------------------------------------------------------------------------
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);//Monogame: Create a new SpriteBatch, which can be used to draw textures.
            Textures = new Dictionary<string, Texture2D>();//MB: Initializes the texture dictionary

            //--------MB: Load all the textures here--------
            Textures.Add("Cursor", Content.Load<Texture2D>("Cursor"));
            
            //---------------------------------------------
            //--------MB: Load anything else here--------

            //------------------------------------------
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();//Monogame MB: This means you can press escape to exit
                       //MB: Potentially we could store the keyboard.GetState() in a variable so we don't have to keep calling it
            switch (GameState)//MB: This is where State-Dependent game logic goes
	        {
	        	case GameState.MainMenu:    
                    break;//MB: This stops the thread running into the next case. Came with the switch.
                case GameState.NewGame:
                    break;
                case GameState.InGame:
                    break;
                default:
                    throw new System.Exception("Invalid GameState");//MB: This should never run, which is why it'd throw an error
        	}
            base.Update(gameTime);//Monogame
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);//MB: Clears the frame with blue

            spriteBatch.Begin();//MB: Allows drawing
            switch (GameState)//MB: This is where State-Dependent screen updating goes
	        {
	        	case GameState.MainMenu:
                    Window.Title=(gameTime.TotalGameTime.ToString() + " - " + 1/(gameTime.ElapsedGameTime.TotalSeconds) + "FPS");//MB: for debugging; shows game duration and fps in the title
                    break;//MB: This stops the thread running into the next case. Came with the switch.
                case GameState.NewGame:
                    break;
                case GameState.InGame:
                    break;
                default:
                    throw new System.Exception("Invalid GameState");//MB: This should never run, which is why it'd throw an error
        	}
            spriteBatch.Draw(Textures["Cursor"],new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));//MB: Draws the cursor at the mouse

            spriteBatch.End();//MB: Drawing not allowed after this
            base.Draw(gameTime);//Monogame
        }
    }
}
#pragma warning restore CS0618
