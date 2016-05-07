using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GameService
{
    class Menu : DrawableGameComponent
    {
        public enum MenuItemType : byte
        {
            Start = 0,
            Exit
        }
        public delegate void MenuEventHandler(MenuItemType item);
        public event MenuEventHandler OnItemSelected;
        KeyboardState previousKeyState;
        KeyboardState currentKeyState;
        int currentIndex;
        MenuItemType[] items;
        SpriteBatch spriteBatch;
        SpriteFont font;

        public Menu(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            this.currentIndex = 0;
            this.items = Enum.GetValues(typeof(MenuItemType)) as MenuItemType[];
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            this.font = Game.Content.Load<SpriteFont>("Fonts\\SampleFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(previousKeyState == null)
            {
                previousKeyState = Keyboard.GetState();
            }
            this.currentKeyState = Keyboard.GetState();
            if (this.IsPressedJustNow(Keys.Up))
            {
                this.currentIndex = MathHelper.Clamp(
                    this.currentIndex - 1,
                    0,
                    this.items.Count() - 1
                );
            }
            if (this.IsPressedJustNow(Keys.Down))
            {
                this.currentIndex = MathHelper.Clamp(
                    this.currentIndex + 1,
                    0,
                    this.items.Count() - 1
                );
            }
            if (this.IsPressedJustNow(Keys.Z))
            {
                this.SelectItem();
            }
            this.previousKeyState = currentKeyState;
            base.Update(gameTime);
        }

        private bool IsPressedJustNow(Keys key)
        {
            return this.previousKeyState.IsKeyUp(key)
                && this.currentKeyState.IsKeyDown(key);
        }

        private void SelectItem()
        {
            this.OnItemSelected(this.items[this.currentIndex]);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                this.font,
                this.items[this.currentIndex].ToString(),
                new Vector2(100, 100),
                Color.White
            );
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
