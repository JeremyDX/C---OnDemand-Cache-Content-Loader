using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class MainStartMenu : ScreenInterface
{
    static Color color;
    static float opacity = 1.0f;
    static bool reversed = false;

    public override void begin()
    {
        GameCache.ShowInterface(0, true);
        color = GameCache.GetInterfaceDefinition(0).getChild(2).color;
        MenuInputFactory.SetMode
        (
            MenuInputFactory.GameStateMode.UPDATE_HORIZONTAL
        );
    }

    public override void update(GameTime gameTime)
    {
        InterfaceChild child = GameCache.GetInterfaceDefinition(0).getChild(2);
        child.color = color * opacity;
        opacity += reversed ? 0.02f : -0.02f;
        if (opacity <= 0.20f)
        {
            reversed = true;
            child.text[6] = child.text[6] == '@' ? '>' : '@';
        }
        else if (opacity >= 1.0f)
            reversed = false;
    }

    public override void handleInput()
    {
        GamePadState state = MenuInputFactory.GetState();
        GamePadState previous = MenuInputFactory.GetPreviousState();
        if (state.Buttons.A != previous.Buttons.A && (int)state.Buttons.A == 0x1)
            GameCache.ShowWindow(0x1);
        else if (state.Buttons.Start != previous.Buttons.Start && (int)state.Buttons.Start == 0x1)
            GameCache.ShowWindow(0x1);
    }
}
