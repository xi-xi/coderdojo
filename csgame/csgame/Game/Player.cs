using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Player : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private KeyboardState currentKeyboardState;
        const float PLAYER_MOVE_SPEED = 8f;
        public Vector2 Position;
        public int Health;
        public int Width
        {
            get { return this.texture.Width; }
        }
        public int Height
        {
            get { return this.texture.Height; }
        }

        public Player(GameService.MainGame game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            this.Health = 100;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.texture = this.Game.Content.Load<Texture2D>("Graphics\\player");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.currentKeyboardState = Keyboard.GetState();
            float playerMoveSpeed = Player.PLAYER_MOVE_SPEED;
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                this.Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                this.Position.X += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                this.Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                this.Position.Y += playerMoveSpeed;
            }
            this.Position.X = MathHelper.Clamp(
                this.Position.X,
                0,
                GraphicsDevice.Viewport.Width - this.Width
            );
            this.Position.Y = MathHelper.Clamp(
                this.Position.Y,
                0,
                GraphicsDevice.Viewport.Height - this.Height
            );
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            spriteBatch.Draw(
                this.texture,
                this.Position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
             );
            base.Draw(gameTime);
            this.spriteBatch.End();
        }
    }
}
