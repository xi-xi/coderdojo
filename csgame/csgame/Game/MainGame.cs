using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter;

namespace GameService
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Enemy enemy;
        Menu menu;
        ResultScene result;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            player = new Player(this)
            {
                Position = new Vector2(
                        GraphicsDevice.Viewport.TitleSafeArea.X,
                        GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2
                    )
            };
            player.EnabledChanged += PlayerEnableChanged;
            enemy = new Enemy(this)
            {
                Position = new Vector2(
                    GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width * 0.8f,
                    GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2
                ),
                Player = player
            };
            enemy.EnabledChanged += EnemyEnabledChanged;
            player.Enemy = enemy;
            this.result = new ResultScene(this);
            menu = new Menu(this);
            menu.OnItemSelected += OnMenuItemSelected;
            this.Components.Add(menu);
            base.Initialize();
        }

        private void EnemyEnabledChanged(object sender, System.EventArgs e)
        {
            if (!enemy.Enabled)
            {
                this.ShowResult("CLEAR");
            }
        }

        private void PlayerEnableChanged(object sender, System.EventArgs e)
        {
            if (!player.Enabled)
            {
                this.ShowResult("FAILED");
            }
        }

        private void ShowResult(string str)
        {
            if (this.Components.Contains(result))
            {
                return;
            }
            this.Components.Add(this.result);
            this.result.ResultString = str;
        }

        private void OnMenuItemSelected(Menu.MenuItemType item)
        {
            if(item == Menu.MenuItemType.Start)
            {
                this.StartGame();
            }
            else if (item == Menu.MenuItemType.Exit)
            {
                this.Exit();
            }
        }

        private void StartGame()
        {
            this.menu.Enabled = false;
            this.menu.Visible = false;
            this.Components.Add(new BackGround(this));
            this.Components.Add(player);
            this.Components.Add(enemy);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            this.previousKeyboardState = currentKeyboardState;
            this.currentKeyboardState = Keyboard.GetState();
            if(previousKeyboardState.IsKeyUp(Keys.F) && currentKeyboardState.IsKeyDown(Keys.F))
            {
                this.graphics.ToggleFullScreen();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
