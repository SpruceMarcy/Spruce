using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;//MB: Allows saving objects to files
using System.IO;//MB: Allows reading and writing of files
using static SpruceGame.GlobalMethods;//MB: Allows the calling of the functions in GlobalMethods.cs without an instance or namespace 
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
        PausedInGame = 3,
        LoadGame = 4,
        Options = 5
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;//Monogame
        SpriteBatch spriteBatch;//Monogame

        //--------MB: Declare variables here that are global to the game--------
        Dictionary<string, Texture2D> textures;//MB: This variable stores all textures, accessible with a string identifier
        Dictionary<string, MapDataPack> mapDataPacks;
        SpriteFont mainFont;//MB: This variable holds the font to be used for buttons etc
        SpriteFont inputFont;//MB: A monospaced font for text inputs
        GameState gameState;//MB: This variable keeps track of whether the game is live or not etc.
        Vector2 screenSize;//MB: The preferred width and height of the screen (X & Y respectively)
        float screenTransformScalar;
        Matrix screenTransform;//MB: A transformation applied after the screen is compiled together
        SaveGame loadedGame;//MB: The instance of the game in play
        MouseState previousMouseState; //MB: A variable to record what the mouse was doing last frame; to detect changes in button presses
        KeyboardState previousKeyboardState;//MB: A variable to record what the keyboard
        Dictionary<string, UIButton> menuButtons;//MB: The collection of buttons that have a fixed occurence
        UITextbox seedBox;//MB: The textbox where the user can enter a level seed when starting a new game

        Song song;//MB: this holds the music. will be obsolete once a music manager is implemented
        //---------------------------------------------------------------------

        public Game1()
        {
            //MB: when the game is instanciated. I think it's better to put everything from here in Initialize()
            graphics = new GraphicsDeviceManager(this);//Monogame
            Content.RootDirectory = "Content";//Monogame
            screenSize = new Vector2(1920, 1080);//MB: This determines the size of the screen TODO: read from settings file instead
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
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;//MB: Window Height
            graphics.PreferredBackBufferWidth = (int)screenSize.X;//MB: Window Width
            //graphics.ToggleFullScreen();//MB: IF YOU CRASH IN FULL SCREEN YOU DIE IN REAL LIFE
            graphics.ApplyChanges();//MB: Updates the screen size
            gameState = GameState.MainMenu;//MB: This means that the game will start at the main menu
            menuButtons = GenerateMenuButtons();//MB: Instanciates all the menu buttons
            seedBox = new UITextbox("", 6, new Point(910, 500), GraphicsDevice, Color.Green, inputFont);
            previousMouseState = Mouse.GetState();
            previousKeyboardState = Keyboard.GetState();
            screenTransformScalar = 1.5f;
            calculateTransformation();
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
            textures = new Dictionary<string, Texture2D>//MB: Textures are stored so that they can be accessed with a string
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
                { "WallTopLeftInv", Content.Load<Texture2D>("WallTopLeftInv") },
                { "WallTopRightInv", Content.Load<Texture2D>("WallTopRightInv") },
                { "WallBottomRightInv", Content.Load<Texture2D>("WallBottomRightInv") },
                { "WallBottomLeftInv", Content.Load<Texture2D>("WallBottomLeftInv") },
                { "Door", Content.Load<Texture2D>("Door")},
                { "Container", Content.Load<Texture2D>("ContainerTemp") },
                { "Player", Content.Load<Texture2D>("PlayerTemp")},
                { "PlayerLegs", Content.Load<Texture2D>("Legs")},
                { "MenuTemplate", Content.Load<Texture2D>("MenuTemplate")},
                { "PauseMenu", new Texture2D(GraphicsDevice,PercentToX(52f/3f),PercentToY(767f/27f))}
            };//MB: Initializes the texture dictionary
            mapDataPacks = new Dictionary<string, MapDataPack>
            {
                { "Federation", new MapDataPack(textures) }
            };
            textures["PauseMenu"].SetData<Color>(GetRectangleDataFromTemplate(textures["MenuTemplate"],new Rectangle(0,0, PercentToX(52f / 3f), PercentToY(767f / 27f))));//MB: This makes the pause menu background

            mainFont = Content.Load<SpriteFont>("MainFont");
            inputFont = Content.Load<SpriteFont>("Monospace");

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                Exit();//Monogame MB: This means you can press F1 to exit at any time
            }
            //MB: Please use these following variables instead of calling any .getstate()
            KeyboardState keyboardState = Keyboard.GetState();//MB: This gets the state of all buttons on the keyboard in this frame
            MouseState mouseState = Mouse.GetState();//MB: This gets the postion and button state of the mouse in this frame
            switch (gameState)//MB: This is where State-Dependent game logic goes
            {
                case GameState.MainMenu:
                    if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released) //MB: if mousedown
                    {
                        if (menuButtons["MainMenuExit"].ClickCheck(mouseState.Position))//MB: If exit button clicked
                        {
                            Exit();
                        }

                        if (menuButtons["MainMenuContinue"].ClickCheck(mouseState.Position))//MB: If continue button clicked
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();//MB: This is the thing that deserializes a file
                            Stream stream = File.Open("Save.xml", FileMode.Open);//MB: Opens a file
                            loadedGame= (SaveGame)binaryFormatter.Deserialize(stream);//MB: Reads the SaveGame stored in file
                            stream.Close();//MB: Closes the file
                            gameState = GameState.InGame;//MB: Starts the game
                            MediaPlayer.Stop();
                        }
                        if (menuButtons["MainMenuNewGame"].ClickCheck(mouseState.Position))//MB: If new game button clicked
                        {
                            gameState = GameState.NewGame;
                        }

                        if (menuButtons["MainMenuLoadGame"].ClickCheck(mouseState.Position))//MB: If load game button clicked
                        {
                            gameState = GameState.LoadGame;
                        }

                        if (menuButtons["MainMenuOptions"].ClickCheck(mouseState.Position))//MB: If options button clicked
                        {
                            gameState = GameState.Options;
                        }
                    }
                    break;//MB: This stops the thread running into the next case. Came with the switch.
                case GameState.NewGame:
                    if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released) //MB: if mousedown
                    {
                        if (menuButtons["NewGameStart"].ClickCheck(mouseState.Position))//MB: If start button clicked
                        {
                            while (seedBox.text.Length<6)
                            {
                                seedBox.text = seedBox.text + "0";//MB: This adds 0s to the seed if it isn't 6 characters long
                            }

                            byte[] bytearray = new byte[seedBox.text.Length / 2];//MB: This is used to store the seed
                            for (int i = 0; i < bytearray.Length; i++)//MB: This loop takes each two chars from the textbox and puts it in the seed as a byte
                            {
                                bytearray[i] = (byte)((HexCharToByte(seedBox.text[2 * i]) * 16) + HexCharToByte(seedBox.text[(2 * i) + 1]));
                            }
                            loadedGame = new SaveGame(bytearray, textures,"Federation");//MB: Instanciates a new game
                            Window.Title = bytearray[0].ToString() + ", " + bytearray[1].ToString() + ", " + bytearray[2].ToString();//MB: Puts the seed in the window bar at the top of the screen
                            gameState = GameState.InGame;//MB: Starts the game
                            MediaPlayer.Stop();
                        }
                        if (menuButtons["NewGameBack"].ClickCheck(mouseState.Position))//MB: If back button clicked
                        {
                            gameState = GameState.MainMenu;//MB: Go to the main menu
                        }

                        if (menuButtons["NewGameRandom"].ClickCheck(mouseState.Position))//MB: If Random Seed button clicked
                        {
                            seedBox.text="000000";//MB: Set the seed to 0
                        }
                    }
                    seedBox.Update(keyboardState, mouseState);//MB: Runs the textbox logic, updating the seed textbox with the input from this frame
                    menuButtons["NewGameStart"].enabled = new Regex("^[0123456789ABCDEF]+$").IsMatch(seedBox.text, 0);//MB: only allows a new game to start if the seed is a valid hex string
                    break;
                case GameState.InGame:
                    if (keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))//MB: If esc key is pressed down
                    {
                        gameState = GameState.PausedInGame;//MB: Pause the game
                    }
                    else
                    {
                        if (mouseState.ScrollWheelValue != previousMouseState.ScrollWheelValue)
                        {
                            screenTransformScalar += ( mouseState.ScrollWheelValue-previousMouseState.ScrollWheelValue)/500f;
                            screenTransformScalar = MathHelper.Max(MathHelper.Min(screenTransformScalar,1.5f),1f);
                            calculateTransformation();
                        }
                        loadedGame.Update(keyboardState,mouseState);//MB: Run the save-dependent logic for this frame, like physics
                    }
                    
                    break;
                case GameState.PausedInGame:
                    if (keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))//MB: If esc key is pressed down
                    {
                        gameState = GameState.InGame;//MB: Unpause the game
                    }
                    if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released) //MB: if mousedown
                    {
                        if (menuButtons["PausedContinue"].ClickCheck(mouseState.Position))//MB: If continue button clicked
                        {
                            gameState = GameState.InGame;//MB: Unpause the game
                        }
                        if (menuButtons["PausedSave"].ClickCheck(mouseState.Position))//MB: If save button clicked
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();//MB: this object serializes the save when needed
                            Stream stream = File.Open("Save.xml", FileMode.Create);//MB: Opens a file (blank)
                            binaryFormatter.Serialize(stream, loadedGame);//MB: Stores the current game in the file
                            stream.Close();//MB: Closes the file
                        }
                        if (menuButtons["PausedExit"].ClickCheck(mouseState.Position))//MB: If exit button clicked
                        {
                            gameState = GameState.MainMenu;//MB: Return to menu
                        }
                    }
                    break;
                case GameState.LoadGame:
                    break;
                case GameState.Options:
                    break;
                default:
                    throw new System.NotImplementedException("Invalid GameState");//MB: This should never run, which is why it'd throw an error
            }
            base.Update(gameTime);//Monogame
            previousMouseState = mouseState;//MB:Store the current mouse state as the previous in the next frame
            previousKeyboardState = keyboardState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            SpruceContentManager.Dispose();
            GraphicsDevice.Clear(Color.Black);//MB: Clears the frame with black
            spriteBatch.Begin();//MB: Allows drawing
            switch (gameState)//MB: This is where State-Dependent screen updating goes
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(textures["Background"], new Vector2(0, 0));//MB: Draws the background
                    foreach (string ButtonName in new string[] { "MainMenuContinue", "MainMenuNewGame", "MainMenuLoadGame", "MainMenuOptions", "MainMenuExit" }) //MB: Draws the buttons
                    {
                        menuButtons[ButtonName].Draw(spriteBatch, Mouse.GetState());
                    }
                    Window.Title = (gameTime.TotalGameTime.ToString() + " - " + 1 / (gameTime.ElapsedGameTime.TotalSeconds) + "FPS");//MB: for debugging; shows game duration and fps in the title
                    break;//MB: This stops the thread running into the next case. Came with the switch.
                case GameState.NewGame:
                    spriteBatch.Draw(textures["Background"], new Vector2(0, 0));//MB: Draws the background
                    foreach (string ButtonName in new string[] { "NewGameStart", "NewGameBack","NewGameRandom" }) //MB: Draws the buttons
                    {
                        menuButtons[ButtonName].Draw(spriteBatch, Mouse.GetState());
                    }
                    seedBox.Draw(spriteBatch);//MB: Draws the seed textbox
                    break;
                case GameState.InGame:
                    spriteBatch.End();
                    spriteBatch.Begin(transformMatrix: screenTransform);
                    loadedGame.Draw(spriteBatch,GraphicsDevice,textures,mapDataPacks);//MB: Draws the game to the screen
                    spriteBatch.End();
                    spriteBatch.Begin();
                    break;
                case GameState.PausedInGame:
                    spriteBatch.End();
                    spriteBatch.Begin(transformMatrix: screenTransform);
                    loadedGame.Draw(spriteBatch, GraphicsDevice, textures, mapDataPacks);//MB: Draws the game to the screen
                    spriteBatch.End();
                    spriteBatch.Begin();
                    spriteBatch.Draw(textures["PauseMenu"], new Vector2(PercentToX(124f/3f),PercentToY(1933/54f)));//MB: Draws the background to the pause menu
                    foreach (string ButtonName in new string[] { "PausedContinue", "PausedSave", "PausedExit" }) //MB: Draws the buttons
                    {
                        menuButtons[ButtonName].Draw(spriteBatch, Mouse.GetState());
                    }
                    break;
                case GameState.LoadGame:
                    break;
                case GameState.Options:
                    break;
                default:
                    throw new System.NotImplementedException("Invalid GameState");//MB: This should never run, which is why it'd throw an error
            }

            spriteBatch.Draw(textures["Cursor"], new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y));//MB: Draws the cursor at the mouse position
           
            spriteBatch.End();//MB: Drawing not allowed after this
            base.Draw(gameTime);//Monogame
        }

        /// <summary>
        /// Returns a dictionary of the non-dynamic buttons. Hardcoded content.
        /// </summary>
        /// <returns>Dictionary of buttons</returns>
        private Dictionary<string, UIButton> GenerateMenuButtons()
        {
            Dictionary<string, UIButton> MenuButtons = new Dictionary<string, UIButton>
            {
                { "MainMenuContinue", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(1523f/54f), PercentToX(200f/15f), PercentToY(205f/27f)), "Continue", GraphicsDevice, textures, mainFont) },
                { "MainMenuNewGame", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(2009f/54f), PercentToX(200f/15f), PercentToY(205f/27f)), "New Game", GraphicsDevice, textures, mainFont) },
                { "MainMenuLoadGame", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(2495f/54f), PercentToX(200f/15f), PercentToY(205f/27f)), "Load Game", GraphicsDevice, textures, mainFont) },
                { "MainMenuOptions", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(2981f/54f), PercentToX(200f/15f), PercentToY(205f/27f)), "Options", GraphicsDevice, textures, mainFont) },
                { "MainMenuExit", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(3467f/54f), PercentToX(200f/15f), PercentToY(205f/27f)), "Exit", GraphicsDevice, textures, mainFont) },
                { "NewGameStart", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(51), PercentToX(200f/15f), PercentToY(205f/27f)), "Start", GraphicsDevice, textures, mainFont) },
                { "NewGameBack", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(60), PercentToX(200f/15f), PercentToY(205f/27f)), "Back", GraphicsDevice, textures, mainFont) },
                { "NewGameRandom", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(37), PercentToX(200f/15f), PercentToY(205f/27f)), "Random Seed", GraphicsDevice, textures, mainFont) },
                { "PausedContinue", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(2009f/54f), PercentToX(200f/15f), PercentToY(205f/27f)), "Continue", GraphicsDevice, textures, mainFont) },
                { "PausedSave", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(2495f/54f), PercentToX(200f/15f), PercentToY(205f/27f)), "Save", GraphicsDevice, textures, mainFont) },
                { "PausedExit", new UIButton(new Rectangle(PercentToX(130f/3f), PercentToY(2981f/54f), PercentToX(200f/15f), PercentToY(205f/27f)), "Exit to menu", GraphicsDevice, textures, mainFont) }
            };
            return MenuButtons;
        }
        /// <summary>
        /// Returns the position on the screen based on proportion rather than absolute value
        /// </summary>
        /// <param name="percentage">X position out of 100</param>
        /// <returns></returns>
        private int PercentToX(float percentage) => (int)(screenSize.X * percentage / 100);
        /// <summary>
        /// Returns the position on the screen based on proportion rather than absolute value
        /// </summary>
        /// <param name="percentage">Y position out of 100</param>
        /// <returns></returns>
        private int PercentToY(float percentage) => (int)(screenSize.Y * percentage / 100);

        private void calculateTransformation()
        {
            screenTransform = Matrix.CreateScale(screenTransformScalar) * Matrix.CreateTranslation(new Vector3(-960, -540, 0) * (screenTransformScalar - 1.0f));
        }
    }
}
#pragma warning restore CS0618
