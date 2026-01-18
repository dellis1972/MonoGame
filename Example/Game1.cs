using System;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Example;
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    // private SpriteBatch _spriteBatch;
    // private Texture2D _texture;
    // private RenderTarget2D _renderTarget;

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
    }

    protected override void LoadContent()
    {
        // _spriteBatch = new SpriteBatch(GraphicsDevice);

        // _texture = new Texture2D(GraphicsDevice, 2, 2);
        // Color[] data = [Color.Red, Color.Green, Color.Blue, Color.Yellow];
        // _texture.SetData(data);

        // _renderTarget = new RenderTarget2D(GraphicsDevice, 200, 200);
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        var oldViewport = GraphicsDevice.Viewport;

        GraphicsDevice.Viewport = new Viewport(0, 0, 10, 10);

        GraphicsDevice.Clear(Color.Red);

        GraphicsDevice.Viewport = oldViewport;

        // GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
        // GraphicsDevice.BlendState = BlendState.Opaque;
        // GraphicsDevice.DepthStencilState = DepthStencilState.None;
        // GraphicsDevice.RasterizerState = RasterizerState.CullNone;

        // _spriteBatch.Begin();
        // // Draw your game objects here
        // _spriteBatch.Draw(_texture, new Rectangle(10, 20, 100, 60), Color.White);
        // _spriteBatch.End();

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
}