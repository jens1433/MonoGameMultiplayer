using System;
using BaseProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class GameEnvironment : Game
{
    protected GraphicsDeviceManager graphics;
    protected SpriteBatch spriteBatch;
    protected InputHelper inputHelper;
    protected Matrix spriteScale;
    protected Point windowSize;

    protected static Point screen;
    protected static GameStateManager gameStateManager;
    protected static Random random;
    protected static AssetManager assetManager;
    protected static GameSettingsManager gameSettingsManager;
    protected static Camera camera;
    protected static GameClient gameClient;
    protected static GameEnvironment instance;

    public GameEnvironment()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        inputHelper = new InputHelper();
        gameStateManager = new GameStateManager();
        spriteScale = Matrix.CreateScale(1, 1, 1);
        random = new Random();
        assetManager = new AssetManager(Content);
        gameSettingsManager = new GameSettingsManager();
        camera = new Camera();
        instance = this;
    }

    protected override void Initialize()
    {
        IsMouseVisible = true;
        base.Initialize();
    }

    public static Point Screen
    {
        get { return screen; }
        set { screen = value; }
    }

    public static Random Random
    {
        get { return random; }
    }

    public static AssetManager AssetManager
    {
        get { return assetManager; }
    }

    public static GameStateManager GameStateManager
    {
        get { return gameStateManager; }
    }

    public static GameSettingsManager GameSettingsManager
    {
        get { return gameSettingsManager; }
    }

    public static Camera Camera
    {
        get { return camera; }
    }

    public static GameClient GameClient
    {
        get { return gameClient; }
    }

    public static GameEnvironment Instance
    {
        get { return instance; }
    }

    public static float GravityConstant
    {
        get { return 1; }
    }

    public static bool IsDebug
    {
#if DEBUG
        get { return true; }
#else
        get { return false; }
#endif
    }

    public bool FullScreen
    {
        get { return graphics.IsFullScreen; }
        set
        {
            ApplyResolutionSettings(value);
        }
    }

    public void ApplyResolutionSettings(bool fullScreen = false)
    {
        if (!fullScreen)
        {
            graphics.PreferredBackBufferWidth = screen.X;
            graphics.PreferredBackBufferHeight = screen.Y;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }
        else
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        float targetAspectRatio = (float)screen.X / (float)screen.Y;
        int width = graphics.PreferredBackBufferWidth;
        int height = (int)(width / targetAspectRatio);
        if (height > graphics.PreferredBackBufferHeight)
        {
            height = graphics.PreferredBackBufferHeight;
            width = (int)(height * targetAspectRatio);
        }

        Viewport viewport = new Viewport();
        viewport.X = (graphics.PreferredBackBufferWidth / 2) - (width / 2);
        viewport.Y = (graphics.PreferredBackBufferHeight / 2) - (height / 2);
        viewport.Width = width;
        viewport.Height = height;
        GraphicsDevice.Viewport = viewport;

        inputHelper.Scale = new Vector2((float)GraphicsDevice.Viewport.Width / screen.X,
                                        (float)GraphicsDevice.Viewport.Height / screen.Y);
        inputHelper.Offset = new Vector2(viewport.X, viewport.Y);
        spriteScale = Matrix.CreateScale(inputHelper.Scale.X, inputHelper.Scale.Y, 1);

        mainRenderTarget = new RenderTarget2D(GraphicsDevice, width, height);
    }

    public static void CreateClient(string ip, int port)
    {
        gameClient = new GameClient(instance, ip, port);
    }

    Effect postfx;

    protected override void LoadContent()
    {
        DrawingHelper.Initialize(this.GraphicsDevice);
        spriteBatch = new SpriteBatch(GraphicsDevice);
        postfx = Content.Load<Effect>("Post");
    }

    protected void HandleInput()
    {
        inputHelper.Update();
        if (inputHelper.KeyPressed(Keys.Escape))
        {
            Exit();
        }
        if (inputHelper.KeyPressed(Keys.F5))
        {
            FullScreen = !FullScreen;
        }
        gameStateManager.HandleInput(inputHelper);
    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        gameStateManager.Update(gameTime);
    }

    RenderTarget2D mainRenderTarget;

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        GraphicsDevice.SetRenderTarget(mainRenderTarget);
        spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation(GraphicsDevice));
        gameStateManager.Draw(gameTime, spriteBatch);
        spriteBatch.End();
        GraphicsDevice.SetRenderTarget(null);
        spriteBatch.Begin(effect: postfx);
        spriteBatch.Draw(mainRenderTarget, Vector2.Zero, Color.White);
        spriteBatch.End();
    }
}