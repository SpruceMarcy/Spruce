using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System.Text.RegularExpressions;
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
        LoadGame = 3,
        Options = 4
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
        SaveGame LoadedGame;
        MouseState PreviousMouseState;
        Dictionary<string, Button> MenuButtons;//MB: The array of buttons on the main menu.
        Textbox SeedBox;

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
            MenuButtons = GenerateMenuButtons(new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2));//MB: Instanciates all the menu buttons
            SeedBox = new Textbox("", 6, new Point(910, 500), GraphicsDevice, Color.Green, InputFont);
            PreviousMouseState = Mouse.GetState();
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
            Textures = new Dictionary<string, Texture2D>
            {
                //--------MB: Load all the textures here--------
                { "Cursor", Content.Load<Texture2D>("Cursor") },
                { "ButtonUnpressed", Content.Load<Texture2D>("ButtonUnpressed") },
                { "ButtonPressed", Content.Load<Texture2D>("ButtonPressed") },
                { "ButtonHover", Content.Load<Texture2D>("ButtonHover") },
                { "ButtonDisabled", Content.Load<Texture2D>("ButtonDisabled") },
                { "ButtonSelected", Content.Load<Texture2D>("ButtonSelected") },
                { "Background", Content.Load<Texture2D>("Background") },
                { "WallTopLeft", Content.Load<Texture2D>("WallTopLeft") },
                { "WallTop", Content.Load<Texture2D>("WallTop") },
                { "WallTopRight", Content.Load<Texture2D>("WallTopRight") },
                { "WallRight", Content.Load<Texture2D>("WallRight") },
                { "WallBottomRight", Content.Load<Texture2D>("WallBottomRight") },
                { "WallBottom", Content.Load<Texture2D>("WallBottom") },
                { "WallBottomLeft", Content.Load<Texture2D>("WallBottomLeft") },
                { "WallLeft", Content.Load<Texture2D>("WallLeft") },
                { "WallMiddle", Content.Load<Texture2D>("WallMiddle") },
                { "Player",Content.Load<Texture2D>("PlayerTemp")}
            };//MB: Initializes the texture dictionary


            MainFont = Content.Load<SpriteFont>("MainFont");
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
                    if (PreviousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released) //MB: if mousedown
                    {
                        if (MenuButtons["MainMenuExit"].ClickCheck(mouseState.Position))//MB: If exit button clicked
                            Exit();
                        if (MenuButtons["MainMenuNewGame"].ClickCheck(mouseState.Position))//MB: If new game button clicked
                            GameState = GameState.NewGame;
                        if (MenuButtons["MainMenuLoadGame"].ClickCheck(mouseState.Position))//MB: If load game button clicked
                            GameState = GameState.LoadGame;
                        if (MenuButtons["MainMenuOptions"].ClickCheck(mouseState.Position))//MB: If options button clicked
                            GameState = GameState.Options;
                    }
                    break;//MB: This stops the thread running into the next case. Came with the switch.
                case GameState.NewGame:
                    if (PreviousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released) //MB: if mousedown
                    {
                        if (MenuButtons["NewGameStart"].ClickCheck(mouseState.Position))//MB: If start button clicked
                        {
                            while (SeedBox.Text.Length<6)
                            {
                                SeedBox.Text = SeedBox.Text + "0";
                            }
                            byte[] bytearray = new byte[SeedBox.Text.Length / 2];
                            for (int i = 0; i < bytearray.Length; i++)
                            {
                                bytearray[i] = (byte)((HexCharToByte(SeedBox.Text[2 * i]) * 16) + HexCharToByte(SeedBox.Text[(2 * i) + 1]));
                            }
                            LoadedGame = new SaveGame(bytearray, Textures);
                            Window.Title = bytearray[0].ToString() + ", " + bytearray[1].ToString() + ", " + bytearray[2].ToString();
                            GameState = GameState.InGame;
                        }
                        if (MenuButtons["NewGameBack"].ClickCheck(mouseState.Position))//MB: If back button clicked
                            GameState = GameState.MainMenu;
                        if (MenuButtons["NewGameRandom"].ClickCheck(mouseState.Position))
                            SeedBox.Text="000000";
                    }
                    SeedBox.Update(keyboardState, mouseState);
                    MenuButtons["NewGameStart"].Enabled = new Regex("^[0123456789ABCDEF]+$").IsMatch(SeedBox.Text, 0);
                    break;
                case GameState.InGame:
                    LoadedGame.Update(keyboardState);
                    break;
                case GameState.LoadGame:
                    break;
                case GameState.Options:
                    break;
                default:
                    throw new System.NotImplementedException("Invalid GameState");//MB: This should never run, which is why it'd throw an error
            }
            base.Update(gameTime);//Monogame
            PreviousMouseState = mouseState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);//MB: Clears the frame with blue

            spriteBatch.Begin();//MB: Allows drawing
            switch (GameState)//MB: This is where State-Dependent screen updating goes
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Textures["Background"], new Vector2(0, 0));//MB: Draws the background
                    foreach (string ButtonName in new string[] { "MainMenuNewGame", "MainMenuLoadGame", "MainMenuOptions", "MainMenuExit" }) //MB: Draws the buttons
                    {
                        MenuButtons[ButtonName].Draw(spriteBatch, Mouse.GetState());
                    }
                    Window.Title = (gameTime.TotalGameTime.ToString() + " - " + 1 / (gameTime.ElapsedGameTime.TotalSeconds) + "FPS");//MB: for debugging; shows game duration and fps in the title
                    break;//MB: This stops the thread running into the next case. Came with the switch.
                case GameState.NewGame:
                    spriteBatch.Draw(Textures["Background"], new Vector2(0, 0));//MB: Draws the background
                    foreach (string ButtonName in new string[] { "NewGameStart", "NewGameBack","NewGameRandom" }) //MB: Draws the buttons
                    {
                        MenuButtons[ButtonName].Draw(spriteBatch, Mouse.GetState());
                    }
                    SeedBox.Draw(spriteBatch);
                    break;
                case GameState.InGame:
                    LoadedGame.Draw(spriteBatch);
                    break;
                case GameState.LoadGame:
                    break;
                case GameState.Options:
                    break;
                default:
                    throw new System.NotImplementedException("Invalid GameState");//MB: This should never run, which is why it'd throw an error
            }
            spriteBatch.Draw(Textures["Cursor"], new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));//MB: Draws the cursor at the mouse

            spriteBatch.End();//MB: Drawing not allowed after this
            base.Draw(gameTime);//Monogame
        }

        /// <summary>
        /// Returns an array of the buttons on the main menu. Hardcoded content.
        /// </summary>
        /// <param name="CentreScreen">The coordinates of the middle of the screen.</param>
        /// <returns>Array of buttons</returns>
        private Dictionary<string, Button> GenerateMenuButtons(Vector2 CentreScreen)
        {
            Dictionary<string, Button> MenuButtons = new Dictionary<string, Button>
            {
                { "MainMenuNewGame", new Button(new Rectangle((int)CentreScreen.X - 128, (int)CentreScreen.Y - 191, 256, 82), "New Game", GraphicsDevice, Textures, MainFont) },
                { "MainMenuLoadGame", new Button(new Rectangle((int)CentreScreen.X - 128, (int)CentreScreen.Y - 91, 256, 82), "Load Game", GraphicsDevice, Textures, MainFont) },
                { "MainMenuOptions", new Button(new Rectangle((int)CentreScreen.X - 128, (int)CentreScreen.Y + 9, 256, 82), "Options", GraphicsDevice, Textures, MainFont) },
                { "MainMenuExit", new Button(new Rectangle((int)CentreScreen.X - 128, (int)CentreScreen.Y + 109, 256, 82), "Exit", GraphicsDevice, Textures, MainFont) },
                { "NewGameStart", new Button(new Rectangle((int)CentreScreen.X - 128, (int)CentreScreen.Y + 9, 256, 82), "Start", GraphicsDevice, Textures, MainFont) },
                { "NewGameBack", new Button(new Rectangle((int)CentreScreen.X - 128, (int)CentreScreen.Y + 109, 256, 82), "Back", GraphicsDevice, Textures, MainFont) },
                { "NewGameRandom", new Button(new Rectangle((int)CentreScreen.X - 128, (int)CentreScreen.Y -141, 256, 82), "Random Seed", GraphicsDevice, Textures, MainFont) }
            };
            return MenuButtons;
        }
        private byte HexCharToByte(char HexChar)
        {
            switch (HexChar)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case 'A':
                    return 10;
                case 'B':
                    return 11;
                case 'C':
                    return 12;
                case 'D':
                    return 13;
                case 'E':
                    return 14;
                case 'F':
                    return 15;
                default:
                    break;
            }
            return 0;
        }
    }
}
#pragma warning restore CS0618
