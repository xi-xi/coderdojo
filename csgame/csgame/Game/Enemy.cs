using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldObject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Enemy : DrawableGameComponent, IShooter
    {
        private const float MAX_HEALTH = 1.0f;
        private SpriteBatch spritebatch;
        private Texture2D texture;
        private List<Bullet> bullets;
        private Texture2D bulletTexture;
        public Player Player { get; set; }
        public List<Bullet> Bullets
        {
            get
            {
                return this.bullets;
            }
        }

        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }

            set
            {
                this.texture = value;
            }
        }

        public int Width
        {
            get { return this.Texture.Width; }
        }

        public int Height
        {
            get { return this.Texture.Height; }
        }

        public float Health { get; set; }

        public Vector2 Position;
        private Random random = new Random();
        private GameTime lastShootTime;

        public Enemy(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            this.bullets = new List<Bullet>();
            this.Health = MAX_HEALTH;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spritebatch = new SpriteBatch(GraphicsDevice);
            this.texture = Game.Content.Load<Texture2D>("Graphics\\enemy.png");
            this.bulletTexture = Game.Content.Load<Texture2D>("Graphics\\bullet.png");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.lastShootTime == null)
            {
                this.lastShootTime = gameTime;
            }
            var elapsed = gameTime.TotalGameTime - this.lastShootTime.TotalGameTime;
            var playerDirection = this.Player.Position - this.Position;
            playerDirection.Normalize();
            if(this.random.Next(0, 6) == 0)
            {
                this.ShootBullet(
                    playerDirection * 10f,
                    null
                );
            }
            this.CheckHit();
            if(this.Health <= 0)
            {
                this.Enabled = false;
                this.Visible = false;
            }
            base.Update(gameTime);
        }

        private void ShootBullet(Vector2 speed, Vector2? acc)
        {
            var bullet = new Bullet(this.Game as GameService.MainGame)
            {
                Texture = this.bulletTexture,
                Position = new Vector2(
                        this.Position.X,
                        this.Position.Y + this.Height / 2
                    ),
                Speed = speed,
                Acceleration = acc ?? Vector2.Zero
            };
            this.Game.Components.Add(bullet);
            this.bullets.Add(bullet);
            bullet.EnabledChanged += new EventHandler<EventArgs>(
                (object sender, EventArgs e) => {
                    if (!(sender as Bullet).Enabled)
                    {
                        bullets.Remove(sender as Bullet);
                        this.Game.Components.Remove(sender as Bullet);
                        bullet.Dispose();
                    }
                }
            );
        }

        private void CheckHit()
        {
            var this_rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            var hitbullets = new List<Bullet>();
            this.Player.Bullets.ForEach((bullet) =>
            {
                var bullet_rect = new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, bullet.Width, bullet.Height);
                if (this_rect.Intersects(bullet_rect))
                {
                    hitbullets.Add(bullet);
                    this.Health -= 0.001f;
                }
            });
            hitbullets.ForEach((bullet) => { bullet.Enabled = false; });
        }

        public override void Draw(GameTime gameTime)
        {
            this.spritebatch.Begin();
            this.spritebatch.Draw(
                this.texture,
                this.Position,
                null,
                this.GetHealthColor(),
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
             );
            this.spritebatch.End();
            base.Draw(gameTime);
        }

        private Color GetHealthColor()
        {
            return new Color(
                1.0f,
                this.Health,
                this.Health
            );
        }
    }
}
