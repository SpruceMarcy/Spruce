using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        InGame = 2,
        LoadGame=3,
        Options=4
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
        SpriteFont MainFont;//MB: This variable holds the font to be used. Only applies to buttons as of 16/07/18
        SpriteFont InputFont;//MB: A monospaced font
        GameState GameState;//MB: This variable keeps track of whether the game is live or not etc.
        MouseState PreviousMouseState;
        Button[] MenuButtons;//MB: The array of buttons on the main menu.
        Song song;//MB: this holds the music. will be obsolete once a music manager is implemented
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
        /// MB: Runs after LoadContent
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
            MenuButtons=GenerateMenuButtons(new Vector2(graphics.PreferredBackBufferWidth/2,graphics.PreferredBackBufferHeight/2));//MB: Instanciates all the menu buttons
            PreviousMouseState=Mouse.GetState();
            //----------------------------------------------------------------------------------------
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// MB: Runs before Initialize
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);//Monogame: Create a new SpriteBatch, which can be used to draw textures.
            Textures = new Dictionary<string, Texture2D>();//MB: Initializes the texture dictionary

            //--------MB: Load all the textures here--------
            Textures.Add("Cursor", Content.Load<Texture2D>("Cursor"));
            Textures.Add("ButtonUnpressed",Content.Load<Texture2D>("ButtonUnpressed"));
            Textures.Add("ButtonPressed", Content.Load<Texture2D>("ButtonPressed"));
            Textures.Add("ButtonHover", Content.Load<Texture2D>("ButtonHover"));
            Textures.Add("ButtonDisabled", Content.Load<Texture2D>("ButtonDisabled"));
            Textures.Add("Background", Content.Load<Texture2D>("Background"));

            MainFont=Content.Load<SpriteFont>("MainFont");
            InputFont = Content.Load<SpriteFont>("Monospace");

            //---------------------------------------------
            //--------MB: Load anything else here--------
            song = Content.Load<Song>("PlaceholderMusic");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

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
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            switch (GameState)//MB: This is where State-Dependent game logic goes
	        {
	        	case GameState.MainMenu:
                    if (PreviousMouseState.LeftButton==ButtonState.Pressed && mouseState.LeftButton==ButtonState.Released) //MB: if mousedown
                    {
                        if (MenuButtons[3].ClickCheck(mouseState.Position))//MB: If exit button clicked
                            Exit();
                        if (MenuButtons[0].ClickCheck(mouseState.Position))//MB: If new game button clicked
                            GameState =GameState.NewGame;
                        if (MenuButtons[1].ClickCheck(mouseState.Position))//MB: If load game button clicked
                            GameState = GameState.LoadGame;
                        if (MenuButtons[2].ClickCheck(mouseState.Position))//MB: If options button clicked
                            GameState = GameState.Options;
                    }
                    break;//MB: This stops the thread running into the next case. Came with the switch.
                case GameState.NewGame:
                    break;
                case GameState.InGame:
                    break;
                case GameState.LoadGame:
                    break;
                case GameState.Options:
                    break;
                default:
                    throw new System.NotImplementedException("Invalid GameState");//MB: This should never run, which is why it'd throw an error
        	}
            base.Update(gameTime);//Monogame
            PreviousMouseState=mouseState;
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
                    spriteBatch.Draw(Textures["Background"], new Vector2(0, 0));//MB: Draws the background
                    foreach (Button button in MenuButtons) //MB: Draws the buttons
                   	{
                        button.Draw(spriteBatch, Mouse.GetState());
	                }
                    Window.Title=(gameTime.TotalGameTime.ToString() + " - " + 1/(gameTime.ElapsedGameTime.TotalSeconds) + "FPS");//MB: for debugging; shows game duration and fps in the title
                    break;//MB: This stops the thread running into the next case. Came with the switch.
                case GameState.NewGame:
                    break;
                case GameState.InGame:
                    break;
                case GameState.LoadGame:
                    break;
                case GameState.Options:
                    break;
                default:
                    throw new System.NotImplementedException("Invalid GameState");//MB: This should never run, which is why it'd throw an error
        	}
            spriteBatch.Draw(Textures["Cursor"],new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));//MB: Draws the cursor at the mouse

            spriteBatch.End();//MB: Drawing not allowed after this
            base.Draw(gameTime);//Monogame
        }

        /// <summary>
        /// Returns an array of the buttons on the main menu. Hardcoded content.
        /// </summary>
        /// <param name="CentreScreen">The coordinates of the middle of the screen.</param>
        /// <returns>Array of buttons</returns>
        private Button[] GenerateMenuButtons(Vector2 CentreScreen)
        {
            Button[] MenuButtons = new Button[4];
            MenuButtons[0]=new Button(new Rectangle((int)CentreScreen.X-128,(int)CentreScreen.Y-191,256,82),"New Game",GraphicsDevice,Textures,MainFont);
            MenuButtons[1]=new Button(new Rectangle((int)CentreScreen.X-128,(int)CentreScreen.Y-91,256,82),"Load Game",GraphicsDevice,Textures,MainFont);
            MenuButtons[2]=new Button(new Rectangle((int)CentreScreen.X-128,(int)CentreScreen.Y+9,256,82),"Options",GraphicsDevice,Textures,MainFont);
            MenuButtons[3]=new Button(new Rectangle((int)CentreScreen.X-128,(int)CentreScreen.Y+109,256,82),"Exit",GraphicsDevice,Textures,MainFont);
            return MenuButtons;
        }
    }
}
#pragma warning restore CS0618
