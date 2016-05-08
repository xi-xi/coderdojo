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
        const string MANUAL = "MANUAL\n" + 
            "    Z : Shot, OK\n" +
            "    Arrow Key : Move\n" +
            "    ESC : Exit";
        public delegate void MenuEventHandler(MenuItemType item);
        public event MenuEventHandler OnItemSelected;
        KeyboardState previousKeyState;
        KeyboardState currentKeyState;
        int currentIndex;
        MenuItemType[] items;
        Vector2[] itemPositions;
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
            this.itemPositions = new Vector2[this.items.Count()];
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            this.font = Game.Content.Load<SpriteFont>("Fonts\\SampleFont");
            var center = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
            for (int i = 0; i < this.items.Count(); ++i)
            {
                this.itemPositions[i] = center -
                    0.5f * font.MeasureString(items[i].ToString()) +
                    new Vector2(0, (i - items.Count() * 0.5f) * 100 + 200);
            }
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
            for(int i = 0; i < items.Count(); ++i)
            {
                spriteBatch.DrawString(
                    this.font,
                    this.items[i].ToString(),
                    this.itemPositions[i],
                    i == this.currentIndex ? Color.OrangeRed : Color.White
                );
            }
            this.DrawManual();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawManual()
        {
            spriteBatch.DrawString(
                this.font,
                Menu.MANUAL,
                new Vector2(50, 50),
                Color.Gray
            );
        }
    }
}
