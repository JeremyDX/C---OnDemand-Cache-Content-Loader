using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public abstract class ScreenInterface
{
    public abstract void handleInput();
    public abstract void begin();
    public abstract void update(GameTime gameTime);
}