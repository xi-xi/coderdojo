using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DAction
{
    class Player : DrawableGameComponent
    {
        Model model;
        float aspectRatio;
        SpriteBatch spriteBatch;
        public Vector3 Position { get; set; }
        float rotation;
        public Vector3 CameraPosition { get; set; }

        public Player(Game game)
            :base(game)
        {

        }

        public override void Initialize()
        {
            this.Position = Vector3.Zero;
            this.CameraPosition = new Vector3(0f, 50.0f, 1000.0f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            model = Game.Content.Load<Model>("Model\\cube");
            aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.rotation += MathHelper.ToRadians((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.1f);
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach(var mesh in model.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(rotation) *
                        Matrix.CreateTranslation(this.Position);
                    effect.View = Matrix.CreateLookAt(
                        this.CameraPosition,
                        Vector3.Zero,
                        Vector3.Up
                    );
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f),
                        aspectRatio,
                        1.0f,
                        10000.0f
                    );
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
