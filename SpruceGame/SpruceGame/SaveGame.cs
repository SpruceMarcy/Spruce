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
        public byte[] seed; //MB: The seed for all randomly determined elements
        public Level loadedLevel; //MB: The labyrinth that is being played
        Player player;
        // - - - - - - - - - - - - - - - - - - -

        public SaveGame(byte[] seed,Texture2D roomData) //MB: on instanciation
        {
            this.seed = seed;
            player = new Player();
            player.pos = new Coord(300, 300); //MB: Sets the player to (300,300) just as a placeholder
            player.textureKey = "Player";
            player.legsKey = "PlayerLegs";
            player.primaryWeapon = new Weapon("CandyGun","CandyBullet");
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, Dictionary<string, Texture2D> textureDict) //MB: Game Logic (single frame)
        {
            Coord movementVector = new Coord(0,0);//MB: This variable records where the player is moving next
            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (!loadedLevel.IsSolid(player.hitbox,player.pos + new Coord(0,-1)))
                {
                    movementVector +=new Coord(0,-1); //MB: If "W" is pressed, move up (y values go up as you go down the screen)
                }
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                if (!loadedLevel.IsSolid(player.hitbox, player.pos + new Coord(0, 1)))
                {
                    movementVector += new Coord(0, 1);  //MB: If "S" is pressed, move down
                }
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (!loadedLevel.IsSolid(player.hitbox, player.pos + new Coord(-1, 0))) //MB: If "A" is pressed, move left
                {
                    movementVector += new Coord(-1, 0); //MB: If "A" is pressed, move left
                }
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (!loadedLevel.IsSolid(player.hitbox, player.pos + new Coord(1,0))) //MB: If "D" is pressed, move right
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
                if (!loadedLevel.IsSolid(player.hitbox, player.pos+movementVector*i))
                {
                    player.pos += movementVector;//MB: Moves to the next location if the new location is not in a wall or something
                }
                else
                {
                    break;
                }
            }
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 muzzle = new Vector2(0, textureDict[player.primaryWeapon.textureKey].Bounds.Height);
                loadedLevel.projectiles.Add(player.primaryWeapon.Fire(player.pos+ Vector2.Transform(new Vector2(8, 0), Matrix.CreateRotationZ(player.angle)).toCoord()-Vector2.Transform(muzzle,Matrix.CreateRotationZ(player.primaryWeapon.angle)).toCoord(),movementVector));
            }
            player.Update(keyboardState,mouseState,movementVector);
            loadedLevel.Update(mouseState, new Coord(960, 540) - player.pos);//MB: run the level logic
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Dictionary<string, Texture2D> textureDict, Dictionary<string, MapDataPack> dataPacks) //MB: frontend stuff
        {
            loadedLevel.Draw(spriteBatch,new Coord(960,540)-player.pos,graphicsDevice,textureDict,dataPacks); //MB: Draw the level
            player.Draw(spriteBatch,graphicsDevice,textureDict);
        }

    }
}
#pragma warning restore CS0618