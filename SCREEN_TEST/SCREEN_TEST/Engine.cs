using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using System.IO;

public class Engine : Microsoft.Xna.Framework.Game
{
    public static SpriteBatch spriteBatch;
    public static SpriteFont font;
    public static Texture2D img;

    public Engine()
    {
        GameScreen.initializeScreen(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    public bool CheckCacheStatus()
    {
        return GameCache.InitializeHeader();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GameScreen.getDevice());
        CacheReader.CreateDevice(GameScreen.getDevice());
        GameCache.ShowWindow(0x0);
    }

    protected override void UnloadContent()
    {
        CacheReader.Close();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            this.Exit();
        MenuInputFactory.update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        InterfaceParent screen;
        if ((screen = GameCache.Poll()) != null)
        {
            if (screen.getImage() == null)
            {
                GameScreen.getDevice().Clear(screen.getColor());
                spriteBatch.Begin();
            }
            else
            {
                GameScreen.getDevice().Clear(Color.Black);
                spriteBatch.Begin();
                spriteBatch.Draw(screen.getImage(), screen.getLocation(), screen.getColor());
            }
            int n = -1;
            foreach (InterfaceChild child in screen.getChildren())
            {
                HoverResource resource = null;
                if (++n == screen.hoverIndex && (resource = screen.hoverResouce) != null)
                {
                    spriteBatch.Draw(resource.getImage(), resource.location, resource.getColor());
                }
                if (child.getType() == 0x0 && child.getImage() != null && resource == null)
                {
                    spriteBatch.Draw(child.getImage(), child.getLocation(), child.getColor());
                }
                else if (child.getType() == 0x1)
                {
                    GameCache.GetFontDefinition(child.getResourceId()).Draw(spriteBatch, child.getLocation(), child.getText(), child.getColor());
                }
            }
        }
        else
        {
            GameScreen.getDevice().Clear(Color.Black);
            spriteBatch.Begin();
        }
        handleGame(gameTime);
        while ((screen = GameCache.Poll()) != null)
        {
            spriteBatch.Draw(screen.getImage(), screen.getLocation(), screen.getColor());
        }
        spriteBatch.End();
        base.Draw(gameTime);
    }

    private void handleGame(GameTime gameTime)
    {
     
    }

}











