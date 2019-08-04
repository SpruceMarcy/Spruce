using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;///MB: Imports dictionaries
using System;//MB: Allows use of [Serializable]
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    [Serializable]//MB: This means that an instance of this class can be written to file
    public class SaveGame
    {
        // - - - - Variables Global to this Save
        readonly string saveName;//MB: No use yet
        byte[] seed; //MB: The seed for all randomly determined elements
        Level loadedLevel; //MB: The labyrinth that is being played
        string playerTextureKey; //MB: A key for the texture dictionary to retrieve the player texture
        Coord playerPos; //MB: The position of the player in the level. May want to move this to Level
        // - - - - - - - - - - - - - - - - - - -

        public SaveGame(byte[] seed, Dictionary<string, Texture2D> textureDict, string dataPackKey) //MB: on instanciation
        {
            this.seed = seed;
            loadedLevel = new Level(5,5,dataPackKey,seed,15); //MB: Create a new placeholder level
            playerPos = new Coord(300, 300); //MB: Sets the player to (300,300) just as a placeholder
            playerTextureKey = "Player";
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState) //MB: Game Logic (single frame)
        {
            Coord movementVector = new Coord(0,0);//MB: This variable records where the player is moving next
            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (!IsSolid(playerPos + new Coord(0,-1)))
                {
                    movementVector +=new Coord(0,-1); //MB: If "W" is pressed, move up (y values go up as you go down the screen)
                }
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                if (!IsSolid(playerPos + new Coord(0, 1)))
                {
                    movementVector += new Coord(0, 1);  //MB: If "S" is pressed, move down
                }
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (!IsSolid(playerPos + new Coord(-1, 0))) //MB: If "A" is pressed, move left
                {
                    movementVector += new Coord(-1, 0); //MB: If "A" is pressed, move left
                }
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (!IsSolid(playerPos + new Coord(1,0))) //MB: If "D" is pressed, move right
                {
                    movementVector += new Coord(1, 0); //MB: If "D" is pressed, move right
                }
            }
            if (!movementVector.Equals(new Coord(0, 0)))//MB: This avoids div by 0 errors
            {
                movementVector /= movementVector.ToVector2().Length();//MB: makes it a unit vector so that diagonal movement is not faster
            }
            for (int i = 1; i <= 4; i++)
            {
                if (!IsSolid(playerPos+movementVector*i))
                {
                    playerPos += movementVector;//MB: Moves to the next location if the new location is not in a wall or something
                }
                else
                {
                    break;
                }
            }

            loadedLevel.Update(mouseState, new Coord(960, 540) - playerPos);//MB: run the level logic
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict, Dictionary<string, MapDataPack> dataPacks) //MB: frontend stuff
        {
            loadedLevel.Draw(spriteBatch,new Coord(960,540)-playerPos,graphicsDevice,textureDict,dataPacks); //MB: Draw the level
            {
                //MB: This is a bodge-together approach to drawing the player at an angle since the center of rotation is in the top left of a texture by default
                Texture2D PlayerTexture = textureDict[playerTextureKey];
                float angle = (float)Math.Atan2(Mouse.GetState().Position.Y - 540, Mouse.GetState().Position.X - 960) + MathHelper.PiOver2;
                Point SpritePosition = new Point(960,540);
                spriteBatch.Draw(PlayerTexture, new Rectangle(SpritePosition, new Point(PlayerTexture.Width, PlayerTexture.Height)), null, Color.Black, angle, new Vector2(PlayerTexture.Width/2f,PlayerTexture.Height/2f), SpriteEffects.None, 0);
            }
        }
        /// <summary>
        /// Returns whether or not the specified position is occupied, preventing movement.
        /// This could also potentially be moved to Level
        /// </summary>
        /// <param name="position">Coordinates in the level to test</param>
        /// <returns></returns>
        public bool IsSolid(Coord position)
        {
            Room room;
            room = loadedLevel.getRoom((int)Math.Floor(position.x/(16*32)),(int)Math.Floor(position.y/ (16 * 32)));//MB: get the room at those coordinates
            
            Tile tile;
            tile = room.tiles[(int)Math.Floor((position.x % (16 * 32)) / 32), (int)Math.Floor((position.y % (16 * 32)) / 32)];//MB: get the tile at those coordinates
            return tile.isSolid;
        }
    }
}
#pragma warning restore CS0618