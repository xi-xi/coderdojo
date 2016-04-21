using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FieldObject
{
    class Bullet :DrawableGameComponent
    {
        private Texture2D texture;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Speed = Vector2.Zero;
        public Vector2 Acceleration = Vector2.Zero;
        private SpriteBatch spriteBatch;
        public Bullet(GameService.MainGame game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.texture = this.Game.Content.Load<Texture2D>("Graphics\\bullet");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.Speed += this.Acceleration;
            this.Position += this.Speed;
            if (this.isOutRange())
            {
                this.Game.Components.Remove(this);
                this.Dispose();
            }
            base.Update(gameTime);
        }

        private bool isOutRange()
        {
            return this.Position.X > GraphicsDevice.Viewport.Width + GraphicsDevice.Viewport.X ||
                this.Position.Y > GraphicsDevice.Viewport.Height + GraphicsDevice.Viewport.Y ||
                this.Position.X < GraphicsDevice.Viewport.X ||
                this.Position.Y < GraphicsDevice.Viewport.Y;
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            spriteBatch.Draw(
                this.texture,
                this.Position,
                null,
                Color.White,
                this.getDirectionRotate(),
                new Vector2(this.texture.Width / 2.0f, this.texture.Height / 2.0f),
                1f,
                SpriteEffects.None,
                0f
             );
            base.Draw(gameTime);
            this.spriteBatch.End();
        }

        private float getDirectionRotate()
        {
            return (float)Math.Atan2(this.Speed.Y, this.Speed.X);
        }
    }
}
