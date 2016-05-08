using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameService
{
    class ResultScene : DrawableGameComponent
    {
        SpriteBatch spritebatch;
        SpriteFont font;
        private Vector2 Position { get; set; }
        private string resultString = "";
        public string ResultString
        {
            get { return this.resultString; }
            set
            {
                this.resultString = value;
                this.Position = (
                    new Vector2(this.Game.GraphicsDevice.Viewport.Width, this.Game.GraphicsDevice.Viewport.Height) -
                    this.font.MeasureString(resultString)
                ) * 0.5f;
            }
        }
        public ResultScene(Game game):
            base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spritebatch = new SpriteBatch(Game.GraphicsDevice);
            this.font = Game.Content.Load<SpriteFont>("Fonts/SampleFont");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();
            spritebatch.DrawString(
                this.font,
                this.ResultString,
                this.Position,
                Color.White
            );
            spritebatch.End();
            base.Draw(gameTime);
        }
    }
}
