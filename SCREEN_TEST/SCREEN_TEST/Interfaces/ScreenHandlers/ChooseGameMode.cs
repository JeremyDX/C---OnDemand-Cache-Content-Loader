using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class ChooseGameMode : ScreenInterface
{
    public override void begin()
    {
        GameCache.ShowInterface(0x1, true);
        MenuInputFactory.SetMode
        (
            MenuInputFactory.GameStateMode.UPDATE_HORIZONTAL, 4.0f, 0x3, false
        );
        if (GameCache.GetInterfaceDefinition(0x1).hoverIndex == -1 ||
            GameCache.GetInterfaceDefinition(0x1).hoverResouce == null)
            GameCache.GetInterfaceDefinition(0x1).setHoverResource(0);
    }

    public override void update(GameTime gameTime)
    {

    }

    public override void handleInput()
    {
        GamePadState state = MenuInputFactory.GetState();
        GamePadState previous = MenuInputFactory.GetPreviousState();
        GameCache.GetInterfaceDefinition(0x1).setHoverResource(MenuInputFactory.selectedItemIndex);
    }
}