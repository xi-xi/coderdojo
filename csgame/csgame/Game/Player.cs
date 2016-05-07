using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FieldObject;

namespace Shooter
{
    class Player : DrawableGameComponent, IShooter
    {
        private const int CRASHED_TIME = 1000;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Texture2D bulletTexture;
        private KeyboardState currentKeyboardState;
        private Vector2 bulletSpeed = new Vector2(50.0f, 0.0f);
        const float PLAYER_MOVE_SPEED = 8f;
        public Vector2 Position;
        public float Health;
        private bool IsCrashed;
        private TimeSpan crashedTime;
        private List<Bullet> bullets;
        public List<Bullet> Bullets { get { return this.bullets; } }
        public int Width
        {
            get { return this.texture.Width; }
        }
        public int Height
        {
            get { return this.texture.Height; }
        }

        public Enemy Enemy { get; set; }

        public Player(GameService.MainGame game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            this.Health = 1.0f;
            this.bullets = new List<Bullet>();
            this.IsCrashed = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.texture = this.Game.Content.Load<Texture2D>("Graphics\\player.png");
            this.bulletTexture = this.Game.Content.Load<Texture2D>("Graphics\\bullet.png");
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
            if (currentKeyboardState.IsKeyDown(Keys.Z))
            {
                this.ShootBullet();
            }
            if(this.Health <= 0)
            {
                this.Enabled = false;
                this.Visible = false;
            }
            this.UpdateCrash(gameTime);
            this.CheckHitBullet(gameTime);
            base.Update(gameTime);
        }

        private void UpdateCrash(GameTime gt)
        {
            if (!this.IsCrashed)
            {
                return;
            }
            if((gt.TotalGameTime - this.crashedTime).TotalMilliseconds >= CRASHED_TIME)
            {
                this.IsCrashed = false;
                return;
            }
        }

        private void CheckHitBullet(GameTime gt)
        {
            if (this.IsCrashed)
            {
                return;
            }
            var this_rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            this.Enemy.Bullets.ForEach((bullet) =>
            {
                var bullet_rect = new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, bullet.Width, bullet.Height);
                if (this_rect.Intersects(bullet_rect)){
                    this.Crash(gt);
                }
            });
        }

        private void Crash(GameTime gt)
        {
            this.IsCrashed = true;
            this.Health -= 0.2f;
            this.crashedTime = gt.TotalGameTime;
        }

        private void ShootBullet()
        {
            var bullet = new Bullet(this.Game as GameService.MainGame)
            {
                Texture = this.bulletTexture,
                Position = new Vector2(
                        this.Position.X + this.Width,
                        this.Position.Y + this.Height / 2
                    ),
                Speed = this.bulletSpeed
            };
            this.Game.Components.Add(bullet);
            this.bullets.Add(bullet);
            bullet.EnabledChanged += new EventHandler<EventArgs>(
                (object sender, EventArgs e) => {
                    if(!(sender as Bullet).Enabled)
                    {
                        bullets.Remove(sender as Bullet);
                        this.Game.Components.Remove(sender as Bullet);
                    }
                }
            );
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(
                this.texture,
                this.Position,
                null,
                this.GetColor(gameTime),
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
            );
            base.Draw(gameTime);
            this.spriteBatch.End();
        }

        private Color GetColor(GameTime gt)
        {
            float alpha = 1.0f;
            float freq = (float)CRASHED_TIME / 5.0f;
            if (this.IsCrashed)
            {
                alpha = ((gt.TotalGameTime - this.crashedTime).Milliseconds % freq) / freq;
            }
            return new Color(
                1.0f,
                this.Health,
                this.Health
            ) * alpha;
        }
    }
}
