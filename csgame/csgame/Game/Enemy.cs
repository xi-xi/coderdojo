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
        enum State
        {
            Beginner,
            Easy,
            Normal,
            Hard,
            Lunatic
        }
        private const float MAX_HEALTH = 1.0f;
        private State state;
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
        private TimeSpan lastShootTime;

        public Enemy(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            this.bullets = new List<Bullet>();
            this.Health = MAX_HEALTH;
            this.state = State.Beginner;
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
            this.DoAction(gameTime);
            this.CheckHit();
            this.UpdateState();
            if(this.Health <= 0)
            {
                this.Enabled = false;
                this.Visible = false;
            }
            base.Update(gameTime);
        }

        private void DoAction(GameTime gametime)
        {
            switch (this.state)
            {
                case State.Beginner:
                    this.BeginnerAction(gametime);
                    break;
                case State.Easy:
                    this.EasyAction(gametime);
                    break;
                case State.Normal:
                    this.NormalAction(gametime);
                    break;
                case State.Hard:
                    this.HardAction(gametime);
                    break;
                case State.Lunatic:
                    this.LunaticAction(gametime);
                    break;
            }
        }

        private void BeginnerAction(GameTime gt)
        {
            if(this.lastShootTime == null)
            {
                this.lastShootTime = gt.TotalGameTime;
                return;
            }
            if ((gt.TotalGameTime - this.lastShootTime).TotalMilliseconds >= 250)
            {
                var playerDirection = this.Player.Position - this.Position;
                playerDirection.Normalize();
                this.ShootBullet(
                    playerDirection * 10f,
                    null
                );
                this.lastShootTime = gt.TotalGameTime;
            }
        }

        private void EasyAction(GameTime gt)
        {
            if (this.lastShootTime == null)
            {
                this.lastShootTime = gt.TotalGameTime;
                return;
            }
            if ((gt.TotalGameTime - this.lastShootTime).TotalMilliseconds >= 200)
            {
                var playerDirection = this.Player.Position - this.Position;
                playerDirection.Normalize();
                this.ShootBullet(
                    playerDirection * 5f,
                    null
                );
                var d30 = Vector2.Transform(playerDirection, Matrix.CreateRotationZ((float)Math.PI / 3.0f));
                this.ShootBullet(
                    d30 * 5f,
                    null
                );
                var d30_ = Vector2.Transform(playerDirection, Matrix.CreateRotationZ(-(float)Math.PI / 3.0f));
                this.ShootBullet(
                    d30_ * 5f,
                    null
                );
                this.lastShootTime = gt.TotalGameTime;
            }
        }

        private void NormalAction(GameTime gt)
        {
            if (this.lastShootTime == null)
            {
                this.lastShootTime = gt.TotalGameTime;
                return;
            }
            if ((gt.TotalGameTime - this.lastShootTime).TotalMilliseconds >= 200)
            {
                var playerDirection = this.Player.Position - this.Position;
                playerDirection.Normalize();
                for (float r = -2.0f; r <= 2.0f; r += 1.0f)
                {
                    var d = Vector2.Transform(
                        playerDirection,
                        Matrix.CreateRotationZ((float)Math.PI / 6.0f * r)
                    );
                    this.ShootBullet(d * 2.5f, null);
                }
                this.lastShootTime = gt.TotalGameTime;
            }
        }

        private void HardAction(GameTime gt)
        {
            if (this.lastShootTime == null)
            {
                this.lastShootTime = gt.TotalGameTime;
                return;
            }
            if ((gt.TotalGameTime - this.lastShootTime).TotalMilliseconds >= 200)
            {
                var playerDirection = this.Player.Position - this.Position;
                playerDirection.Normalize();
                for(float r = -4.0f; r <= 4.0f; r += 1.0f)
                {
                    var d = Vector2.Transform(
                        playerDirection,
                        Matrix.CreateRotationZ((float)Math.PI / 6.0f * r)
                    );
                    for(float a = -0.01f; a <= 0.01f; a += 0.01f)
                    {
                        var acc = Vector2.Transform(
                            d,
                            Matrix.CreateRotationZ((float)Math.PI / 2.0f)
                        ) * a;
                        this.ShootBullet(d * 2.0f, acc);
                    }
                }
                this.lastShootTime = gt.TotalGameTime;
            }
        }

        private void LunaticAction(GameTime gt)
        {
            if (this.lastShootTime == null)
            {
                this.lastShootTime = gt.TotalGameTime;
                return;
            }
            if ((gt.TotalGameTime - this.lastShootTime).TotalMilliseconds >= 200)
            {
                var playerDirection = this.Player.Position - this.Position;
                playerDirection.Normalize();
                for (float r = -6.0f; r <= 6.0f; r += 0.5f)
                {
                    var d = Vector2.Transform(
                        playerDirection,
                        Matrix.CreateRotationZ((float)Math.PI / 6.0f * r)
                    );
                    var acc = Vector2.Transform(
                        d,
                        Matrix.CreateRotationZ((float)Math.PI / 2.0f)
                    ) * ((float)this.random.NextDouble() - 0.5f) * 0.005f;
                    this.ShootBullet(d * 2.0f, acc);
                }
                this.lastShootTime = gt.TotalGameTime;
            }
        }

        private void UpdateState()
        {
            this.state = this.Health > MAX_HEALTH * 0.8 ? State.Beginner
                : this.Health > MAX_HEALTH * 0.6 ? State.Easy
                : this.Health > MAX_HEALTH * 0.4 ? State.Normal
                : this.Health > MAX_HEALTH * 0.2 ? State.Hard
                : State.Lunatic;
            this.Game.Window.Title = this.state.ToString();
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
