using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Example;
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _texture, _blankTexture;
    private SoundEffectInstance _soundEffect;
    private RenderTarget2D _renderTarget;
    private int rotation = 0;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 320;
        _graphics.PreferredBackBufferHeight = 200;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        base.Initialize();
        Window.AllowUserResizing = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _texture = Content.Load<Texture2D>("test");
        _blankTexture = new Texture2D(GraphicsDevice, 1, 1);
        _blankTexture.SetData(new[] { Color.White });

        _soundEffect = Content.Load<SoundEffect>("testsound").CreateInstance();
        //_soundEffect.IsLooped = true;
        //_soundEffect.Play();

        _renderTarget = new RenderTarget2D(GraphicsDevice, 200, 200);
    }

    int x, y = 0;
    int h = 200;
    int _drawCount = 0;

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();
        if (keyboardState.IsKeyDown(Keys.S))
            h-=10;
        if (keyboardState.IsKeyDown(Keys.W))
            h+=10;
        if (keyboardState.IsKeyDown(Keys.Up))
            y-=10;
        if (keyboardState.IsKeyDown(Keys.Down))
            y+=10;

        rotation += 1;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _drawCount++;
        bool log = _drawCount <= 20;
        if (log) Console.WriteLine($"  Draw #{_drawCount}: SetRenderTarget(_renderTarget)");
        GraphicsDevice.SetRenderTarget(_renderTarget);
        if (log) Console.WriteLine($"  Draw #{_drawCount}: Clear(Green) on RT");
        GraphicsDevice.Clear(Color.Green);
        if (log) Console.WriteLine($"  Draw #{_drawCount}: SetRenderTarget(null)");
        GraphicsDevice.SetRenderTarget(null);
        if (log) Console.WriteLine($"  Draw #{_drawCount}: Clear(CornflowerBlue) on backbuffer");
        GraphicsDevice.Clear(Color.CornflowerBlue);

        GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
        GraphicsDevice.BlendState = BlendState.Opaque;
        GraphicsDevice.DepthStencilState = DepthStencilState.None;
        GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        var vp = GraphicsDevice.Viewport;

        if (log) Console.WriteLine($"  Draw #{_drawCount}: SpriteBatch.Begin()");
        _spriteBatch.Begin();
        // Draw your game objects here
        _spriteBatch.Draw(_blankTexture, new Rectangle(vp.X, vp.Y+10, vp.Width, vp.Height-20), Color.Red);
        _spriteBatch.Draw(_texture, new Rectangle(x, 10, 60, 60), null, Color.White, MathHelper.ToRadians(rotation), new Vector2(30, 30), SpriteEffects.None, 0f);
         _spriteBatch.Draw(_texture, new Rectangle(320-60, h-60, 60, 60), Color.White);
        _spriteBatch.Draw(_renderTarget, new Rectangle(120, 20, 40, 40), Color.White);
        if (log) Console.WriteLine($"  Draw #{_drawCount}: SpriteBatch.End()");
        _spriteBatch.End();
        if (log) Console.WriteLine($"  Draw #{_drawCount}: complete");

        base.Draw(gameTime);
    }
}

public static class Program
{
    [STAThread]
#if MG_Web
    static async Task Main()
    {

        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
#else
    static void Main()
    {
#endif
        Console.WriteLine("Creating Game1");
        try {
        using (var game = new Game1())
        {
            Console.WriteLine("Running Game1");
            game.Run();
#if MG_Web
            Console.WriteLine("Run returned now yielding to JS event loop");
            await tcs.Task;
            Console.WriteLine("Resuming after yield");
#endif
        }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception in Main: " + ex);
        }
    }
}