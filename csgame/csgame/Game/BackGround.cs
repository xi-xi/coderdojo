using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameService
{
    class BackGround : DrawableGameComponent
    {
        class SpeedTexture
        {
            public Texture2D Texture{ get; set; }
            public float Speed { get; set; }
            public Color Color { get; set; }
        }
        private List<SpeedTexture> textures;
        private TimeSpan initialziedTime;
        private SpriteBatch spritebatch;

        public BackGround(Game game)
            :base(game)
        {

        }

        public override void Initialize()
        {
            this.textures = new List<SpeedTexture>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.textures.Add(
                new SpeedTexture()
                {
                    Texture = Game.Content.Load<Texture2D>("Graphics\\cloud.png"),
                    Speed = 0.01f,
                    Color = Color.DarkGray
                }
            );
            this.textures.Add(
                new SpeedTexture()
                {
                    Texture = Game.Content.Load<Texture2D>("Graphics\\buildings.png"),
                    Speed = 0.2f,
                    Color = Color.DarkGray
                }
            );
            this.spritebatch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if(this.initialziedTime == null)
            {
                this.initialziedTime = gameTime.TotalGameTime;
            }
            double timespan = (this.initialziedTime - gameTime.TotalGameTime).TotalMilliseconds;
            spritebatch.Begin();
            foreach(var item in this.textures)
            {
                Vector2 position = new Vector2(
                    (float)((timespan * item.Speed) % item.Texture.Width),
                    .0f
                );
                spritebatch.Draw(
                    item.Texture,
                    position,
                    item.Color
                );
                spritebatch.Draw(
                    item.Texture,
                    position + new Vector2(item.Texture.Width, .0f),
                    item.Color
                );
                
            }
            spritebatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
