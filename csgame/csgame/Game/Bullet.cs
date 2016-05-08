using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FieldObject
{
    class Bullet :DrawableGameComponent
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position = Vector2.Zero;
        public Vector2 Speed = Vector2.Zero;
        public Vector2 Acceleration = Vector2.Zero;
        private SpriteBatch spriteBatch;
        public int Width { get { return this.Texture.Width; } }
        public int Height { get { return this.Texture.Height; } }
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
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.Speed += this.Acceleration;
            this.Position += this.Speed;
            if (this.isOutRange())
            {
                this.Game.Components.Remove(this);
                this.Enabled = false;
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
            if (this.Texture != null)
            {
                spriteBatch.Draw(
                    this.Texture,
                    this.Position,
                    null,
                    Color.White,
                    this.getDirectionRotate(),
                    new Vector2(this.Texture.Width / 2.0f, this.Texture.Height / 2.0f),
                    1f,
                    SpriteEffects.None,
                    0f
                 );
            }
            base.Draw(gameTime);
            this.spriteBatch.End();
        }

        private float getDirectionRotate()
        {
            return (float)Math.Atan2(this.Speed.Y, this.Speed.X);
        }
    }
}
