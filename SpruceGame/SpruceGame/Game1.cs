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
    public class Button
	{
        private Texture2D ButtonTexture;
        Rectangle rectangle;
        bool Enabled = true;
        public Button(Rectangle rectangle, string Text, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> TextureDict)
        {
            this.rectangle=rectangle;
            ButtonTexture=new Texture2D(graphicsDevice,rectangle.Width,rectangle.Height);
            Color[] data = new Color[rectangle.Width*rectangle.Height];
            Color[] tempData;
            Color temp;
            for(int y = 0; y < rectangle.Height; y++)
            {
                for (int x = 0; x < rectangle.Width; x++)
			    {
                    temp=Color.Green;                    
                    if (x<TextureDict["ButtonTopLeft"].Width && y<TextureDict["ButtonTopLeft"].Height)
                    {
                        tempData=new Color[TextureDict["ButtonTopLeft"].Width*TextureDict["ButtonTopLeft"].Height];
                        TextureDict["ButtonTopLeft"].GetData<Color>(tempData);
                        temp=tempData[y*TextureDict["ButtonTopLeft"].Width+x];
                    }
                    else if (x>rectangle.Width-TextureDict["ButtonTopRight"].Width-1 && y<TextureDict["ButtonTopRight"].Height)
	                {
                        tempData=new Color[TextureDict["ButtonTopRight"].Width*TextureDict["ButtonTopRight"].Height];
                        TextureDict["ButtonTopRight"].GetData<Color>(tempData);
                        temp=tempData[(y+1)*TextureDict["ButtonTopRight"].Width+x-rectangle.Width];
                	}
                    else if (x<TextureDict["ButtonBottomLeft"].Width && y>rectangle.Height-TextureDict["ButtonBottomLeft"].Height-1)
	                {
                        tempData=new Color[TextureDict["ButtonBottomLeft"].Width*TextureDict["ButtonBottomLeft"].Height];
                        TextureDict["ButtonBottomLeft"].GetData<Color>(tempData);
                        temp=tempData[(y-rectangle.Height+TextureDict["ButtonBottomLeft"].Height)*TextureDict["ButtonBottomLeft"].Width+x];
                	}
                    else if (x>rectangle.Width-TextureDict["ButtonBottomRight"].Width-1 && y>rectangle.Height-TextureDict["ButtonBottomRight"].Height-1)
	                {
                        tempData=new Color[TextureDict["ButtonBottomRight"].Width*TextureDict["ButtonBottomRight"].Height];
                        TextureDict["ButtonBottomRight"].GetData<Color>(tempData);
                        temp=tempData[(y-rectangle.Height+TextureDict["ButtonBottomRight"].Height+1)*TextureDict["ButtonBottomRight"].Width+x-rectangle.Width];
                	}
                    else if (y<TextureDict["ButtonTop"].Height)
	                {
                        tempData=new Color[TextureDict["ButtonTop"].Height];
                        TextureDict["ButtonTop"].GetData<Color>(tempData);
                        temp=tempData[y];
	                }
                    else if (x<TextureDict["ButtonLeft"].Width)
	                {
                        tempData=new Color[TextureDict["ButtonLeft"].Width];
                        TextureDict["ButtonLeft"].GetData<Color>(tempData);
                        temp=tempData[x];
	                }
                    else if (y>rectangle.Height-TextureDict["ButtonBottom"].Height-1)
	                {
                        tempData=new Color[TextureDict["ButtonBottom"].Height];
                        TextureDict["ButtonBottom"].GetData<Color>(tempData);
                        temp=tempData[y-rectangle.Height+TextureDict["ButtonBottom"].Height];
	                }
                    else if (x>rectangle.Width-TextureDict["ButtonRight"].Width-1)
	                {
                        tempData=new Color[TextureDict["ButtonRight"].Width];
                        TextureDict["ButtonRight"].GetData<Color>(tempData);
                        temp=tempData[x-rectangle.Width+TextureDict["ButtonRight"].Width];
	                }
                    else
                	{
                        tempData=new Color[1];
                        TextureDict["ButtonMiddle"].GetData<Color>(tempData);
                        temp=tempData[0];
                	}
                    data[y*rectangle.Width+x]=temp;
			    }
            }
            ButtonTexture.SetData(data);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ButtonTexture,rectangle.Location.ToVector2());
        }
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
        Button[] MenuButtons;
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
            
            Textures.Add("ButtonTopLeft",Content.Load<Texture2D>("ButtonTopLeft"));
            Textures.Add("ButtonTop",Content.Load<Texture2D>("ButtonTop"));
            Textures.Add("ButtonTopRight",Content.Load<Texture2D>("ButtonTopRight"));
            Textures.Add("ButtonLeft",Content.Load<Texture2D>("ButtonLeft"));
            Textures.Add("ButtonMiddle",Content.Load<Texture2D>("ButtonMiddle"));
            Textures.Add("ButtonRight",Content.Load<Texture2D>("ButtonRight"));
            Textures.Add("ButtonBottomLeft",Content.Load<Texture2D>("ButtonBottomLeft"));
            Textures.Add("ButtonBottom",Content.Load<Texture2D>("ButtonBottom"));
            Textures.Add("ButtonBottomRight",Content.Load<Texture2D>("ButtonBottomRight"));       

            //---------------------------------------------
            //--------MB: Load anything else here--------
            MenuButtons=new Button[4];
            MenuButtons[0]=new Button(new Rectangle(500,200,128,50),"",GraphicsDevice,Textures);

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
                    MenuButtons[0].Draw(spriteBatch);
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
