﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;///MB: Imports dictionaries
#pragma warning disable CS0618//MB: This disables the depreciated method warning

namespace SpruceGame
{
    /// <summary>
    /// MB: This class is to encapsulate all features of a box for user text input
    /// </summary>
    public class UITextbox
    {
        private Texture2D texture;//MB: Holds the texture for the sprite without text
        readonly Rectangle rectangle;
        readonly int charLimit;
        private bool selected;
        public string text;
        SpriteFont textFont;
        KeyboardState prevKeyState;
        MouseState prevMouseState;

        public UITextbox(string text, int charLimit, Point position, GraphicsDevice graphicsDevice, Color borderColour, SpriteFont textFont)
        {
            {
                Vector2 Size = textFont.MeasureString(new string(' ', charLimit));
                Size.X += 2;
                this.rectangle = new Rectangle(position, Size.ToPoint());
            }
            this.text = text;
            this.textFont = textFont;
            this.charLimit = charLimit;
            this.selected = false;
            prevKeyState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
            texture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);
            {
                Color[] TempData = new Color[rectangle.Width * rectangle.Height];
                for (int y = 0; y < rectangle.Height; y++)
                {
                    TempData[y * rectangle.Width] = borderColour;
                    TempData[(y + 1) * rectangle.Width - 1] = borderColour;
                    for (int x = 1; x < rectangle.Width - 1; x++)
                    {
                        TempData[y * rectangle.Width + x] = y == 0 || y == rectangle.Height - 1 ? borderColour : Color.White;
                    }
                }
                texture.SetData(TempData);
            }
        }
        public void Update(KeyboardState keyboardState, MouseState mouseState)
        {
            if (selected)
            {
                Keys[] pressedKeys = keyboardState.GetPressedKeys();
                foreach (Keys key in prevKeyState.GetPressedKeys())
                {
                    if (Array.IndexOf(pressedKeys, key) == -1)
                    {
                        String Newstring = text;
                        //MB: I know this place is a mess. we could use a dictionary? it'd still be quite large...
                        Keys[] ValidCharacters = { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z };
                        if (Array.IndexOf(ValidCharacters, key) > -1)
                        {
                            Newstring = Newstring + key.ToString();
                        }
                        else
                        {
                            switch (key)
                            {
                                case Keys.None:
                                    break;
                                case Keys.Back:
                                    if (Newstring.Length > 0)
                                    {
                                        Newstring = Newstring.Substring(0, Newstring.Length - 1);
                                    }
                                    break;
                                case Keys.Tab:
                                    break;
                                case Keys.Enter:
                                    break;
                                case Keys.CapsLock:
                                    break;
                                case Keys.Escape:
                                    break;
                                case Keys.Space:
                                    Newstring = Newstring + " ";
                                    break;
                                case Keys.PageUp:
                                    break;
                                case Keys.PageDown:
                                    break;
                                case Keys.End:
                                    break;
                                case Keys.Home:
                                    break;
                                case Keys.Left:
                                    break;
                                case Keys.Up:
                                    break;
                                case Keys.Right:
                                    break;
                                case Keys.Down:
                                    break;
                                case Keys.Select:
                                    break;
                                case Keys.Print:
                                    break;
                                case Keys.Execute:
                                    break;
                                case Keys.PrintScreen:
                                    break;
                                case Keys.Insert:
                                    break;
                                case Keys.Delete:
                                    break;
                                case Keys.Help:
                                    break;
                                case Keys.D0:
                                    Newstring = Newstring + "0";
                                    break;
                                case Keys.D1:
                                    Newstring = Newstring + "1";
                                    break;
                                case Keys.D2:
                                    Newstring = Newstring + "2";
                                    break;
                                case Keys.D3:
                                    Newstring = Newstring + "3";
                                    break;
                                case Keys.D4:
                                    Newstring = Newstring + "4";
                                    break;
                                case Keys.D5:
                                    Newstring = Newstring + "5";
                                    break;
                                case Keys.D6:
                                    Newstring = Newstring + "6";
                                    break;
                                case Keys.D7:
                                    Newstring = Newstring + "7";
                                    break;
                                case Keys.D8:
                                    Newstring = Newstring + "8";
                                    break;
                                case Keys.D9:
                                    Newstring = Newstring + "9";
                                    break;
                                case Keys.LeftWindows:
                                    break;
                                case Keys.RightWindows:
                                    break;
                                case Keys.Apps:
                                    break;
                                case Keys.Sleep:
                                    break;
                                case Keys.NumPad0:
                                    break;
                                case Keys.NumPad1:
                                    break;
                                case Keys.NumPad2:
                                    break;
                                case Keys.NumPad3:
                                    break;
                                case Keys.NumPad4:
                                    break;
                                case Keys.NumPad5:
                                    break;
                                case Keys.NumPad6:
                                    break;
                                case Keys.NumPad7:
                                    break;
                                case Keys.NumPad8:
                                    break;
                                case Keys.NumPad9:
                                    break;
                                case Keys.Multiply:
                                    break;
                                case Keys.Add:
                                    break;
                                case Keys.Separator:
                                    break;
                                case Keys.Subtract:
                                    break;
                                case Keys.Decimal:
                                    break;
                                case Keys.Divide:
                                    break;
                                case Keys.F1:
                                    break;
                                case Keys.F2:
                                    break;
                                case Keys.F3:
                                    break;
                                case Keys.F4:
                                    break;
                                case Keys.F5:
                                    break;
                                case Keys.F6:
                                    break;
                                case Keys.F7:
                                    break;
                                case Keys.F8:
                                    break;
                                case Keys.F9:
                                    break;
                                case Keys.F10:
                                    break;
                                case Keys.F11:
                                    break;
                                case Keys.F12:
                                    break;
                                case Keys.F13:
                                    break;
                                case Keys.F14:
                                    break;
                                case Keys.F15:
                                    break;
                                case Keys.F16:
                                    break;
                                case Keys.F17:
                                    break;
                                case Keys.F18:
                                    break;
                                case Keys.F19:
                                    break;
                                case Keys.F20:
                                    break;
                                case Keys.F21:
                                    break;
                                case Keys.F22:
                                    break;
                                case Keys.F23:
                                    break;
                                case Keys.F24:
                                    break;
                                case Keys.NumLock:
                                    break;
                                case Keys.Scroll:
                                    break;
                                case Keys.LeftShift:
                                    break;
                                case Keys.RightShift:
                                    break;
                                case Keys.LeftControl:
                                    break;
                                case Keys.RightControl:
                                    break;
                                case Keys.LeftAlt:
                                    break;
                                case Keys.RightAlt:
                                    break;
                                case Keys.BrowserBack:
                                    break;
                                case Keys.BrowserForward:
                                    break;
                                case Keys.BrowserRefresh:
                                    break;
                                case Keys.BrowserStop:
                                    break;
                                case Keys.BrowserSearch:
                                    break;
                                case Keys.BrowserFavorites:
                                    break;
                                case Keys.BrowserHome:
                                    break;
                                case Keys.VolumeMute:
                                    break;
                                case Keys.VolumeDown:
                                    break;
                                case Keys.VolumeUp:
                                    break;
                                case Keys.MediaNextTrack:
                                    break;
                                case Keys.MediaPreviousTrack:
                                    break;
                                case Keys.MediaStop:
                                    break;
                                case Keys.MediaPlayPause:
                                    break;
                                case Keys.LaunchMail:
                                    break;
                                case Keys.SelectMedia:
                                    break;
                                case Keys.LaunchApplication1:
                                    break;
                                case Keys.LaunchApplication2:
                                    break;
                                case Keys.OemSemicolon:
                                    break;
                                case Keys.OemPlus:
                                    break;
                                case Keys.OemComma:
                                    break;
                                case Keys.OemMinus:
                                    break;
                                case Keys.OemPeriod:
                                    break;
                                case Keys.OemQuestion:
                                    break;
                                case Keys.OemTilde:
                                    break;
                                case Keys.OemOpenBrackets:
                                    break;
                                case Keys.OemPipe:
                                    break;
                                case Keys.OemCloseBrackets:
                                    break;
                                case Keys.OemQuotes:
                                    break;
                                case Keys.Oem8:
                                    break;
                                case Keys.OemBackslash:
                                    break;
                                case Keys.ProcessKey:
                                    break;
                                case Keys.Attn:
                                    break;
                                case Keys.Crsel:
                                    break;
                                case Keys.Exsel:
                                    break;
                                case Keys.EraseEof:
                                    break;
                                case Keys.Play:
                                    break;
                                case Keys.Zoom:
                                    break;
                                case Keys.Pa1:
                                    break;
                                case Keys.OemClear:
                                    break;
                                case Keys.ChatPadGreen:
                                    break;
                                case Keys.ChatPadOrange:
                                    break;
                                case Keys.Pause:
                                    break;
                                case Keys.ImeConvert:
                                    break;
                                case Keys.ImeNoConvert:
                                    break;
                                case Keys.Kana:
                                    break;
                                case Keys.Kanji:
                                    break;
                                case Keys.OemAuto:
                                    break;
                                case Keys.OemCopy:
                                    break;
                                case Keys.OemEnlW:
                                    break;
                                default:
                                    break;
                            }

                        }
                        if (Newstring.Length <= charLimit)
                        {
                            text = Newstring;
                        }
                    }
                }
            }
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                selected = rectangle.Contains(mouseState.Position);

            }
            prevKeyState = keyboardState;
            prevMouseState = mouseState;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw the button to.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle.Location.ToVector2());
            string DisplayText = text;
            if (text.Length<charLimit && selected)
            {
                DisplayText += '_';
            }
            spriteBatch.DrawString(textFont, DisplayText, new Vector2(rectangle.X + 1, rectangle.Y + 1), Color.Black);
        }
    }
}
#pragma warning restore CS0618