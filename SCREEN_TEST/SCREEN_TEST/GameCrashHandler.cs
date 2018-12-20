using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using System.IO;

public class GameCrashHandler : Microsoft.Xna.Framework.Game
{
    GraphicsDeviceManager graphics;
    public static SpriteFont font;
    public static SpriteBatch spriteBatch;

    public GameCrashHandler()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Assets";
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        font = Content.Load<SpriteFont>("SpriteFont1");
    }

    protected override void UnloadContent()
    {

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            this.Exit();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin();
        spriteBatch.DrawString(font, GameCache.Debug(), new Vector2(200, 200), Color.White);
        spriteBatch.End();
        base.Draw(gameTime);
    }

}











