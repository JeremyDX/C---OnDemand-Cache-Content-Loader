using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class MenuInputFactory
{
    public enum GameStateMode
    {
        NOTHING = 0x0, 
        PROCESS_INPUT = 0x1, 
        UPDATE_VERTICAL = 0x2, 
        UPDATE_HORIZONTAL = 0x4,
        UPDATE_BOTH = 0x8,
    }

    public static byte selectedItemIndex = 0;

    private static float holdTime = 0.0f;
    private static bool isHolding = false;

    private static bool LOOPING;
    private static byte MAX_OPTIONS;
    private static float MOVE_SPEED;

    private static GamePadState CURRENT_STATE;
    private static GamePadState PREVIOUS_STATE;

    private static GameStateMode MODE;

    public static void update(GameTime gameTime)
    {
        if (MODE == GameStateMode.NOTHING)
            return;
        CURRENT_STATE = GamePad.GetState(0);
        switch (MODE)
        {
           case GameStateMode.UPDATE_VERTICAL:
                UpdateUpDownMovement(gameTime);
                break;
           case GameStateMode.UPDATE_HORIZONTAL:
                UpdateLeftRightMovement(gameTime);
                break;
           case GameStateMode.UPDATE_BOTH:
                break;
        }
        GameCache.HandleWindowInput(gameTime);
        PREVIOUS_STATE = CURRENT_STATE;
    }

    public static void SetMode(GameStateMode mode)
    {
        SetMode(mode, 0.0F, 0x0, false);
    }

    public static void SetMode(GameStateMode mode, float speed, int options, bool loop)
    {
        MODE = mode;
        MOVE_SPEED = 1000.0f / speed;
        MAX_OPTIONS = (byte)(options - 1);
        LOOPING = loop;
    }

    public static GamePadState GetState()
    {
        return CURRENT_STATE;
    }

    public static GamePadState GetPreviousState()
    {
        return PREVIOUS_STATE;
    }

    public static void ResizeList(int size)
    {
        MAX_OPTIONS = (byte)(size - 1);
    }

    //This increments the list.
    private static void Increment(int time, Boolean released)
    {
        holdTime += time;
        if (released)
        {
            isHolding = false;
            holdTime = 0.0f;
        }
        else
        {
            if (holdTime == time && !isHolding)
                ++selectedItemIndex;
        }
        if (holdTime > MOVE_SPEED)
        {
            ++selectedItemIndex;
            isHolding = true;
            holdTime = 0.0f;
        }
        if (selectedItemIndex > MAX_OPTIONS)
            selectedItemIndex = LOOPING ? (byte)0 : MAX_OPTIONS;
    }

    //This decrements this list.
    private static void Decrement(int time, Boolean released)
    {
        holdTime += time;
        if (released)
        {
            isHolding = false;
            holdTime = 0.0f;
        }
        else
        {
            if (holdTime == time && !isHolding)
                --selectedItemIndex;
        }
        if (holdTime > MOVE_SPEED)
        {
            --selectedItemIndex;
            isHolding = true;
            holdTime = 0.0f;
        }
        if (selectedItemIndex == 255)
            selectedItemIndex = LOOPING ? MAX_OPTIONS : (byte)0;
    }

    //This decides if the list should be incremented, decrements, or do nothing.
    private static void UpdateUpDownMovement(GameTime gameTime)
    {
        if (PREVIOUS_STATE.ThumbSticks.Left.Y <= -0.15f)
        {
            Increment(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.ThumbSticks.Left.Y > -0.15f);
        }
        else if (PREVIOUS_STATE.ThumbSticks.Left.Y >= 0.15f)
        {
            Decrement(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.ThumbSticks.Left.Y < 0.15f);
        }
        else if (PREVIOUS_STATE.ThumbSticks.Right.Y <= -0.15f)
        {
            Increment(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.ThumbSticks.Right.Y > -0.15f);
        }
        else if (PREVIOUS_STATE.ThumbSticks.Right.Y >= 0.15f)
        {
            Decrement(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.ThumbSticks.Right.Y < 0.15f);
        }
        else if (PREVIOUS_STATE.DPad.Down == ButtonState.Pressed)
        {
            Increment(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.DPad.Down == ButtonState.Released);
        }
        else if (PREVIOUS_STATE.DPad.Up == ButtonState.Pressed)
        {
            Decrement(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.DPad.Up == ButtonState.Released);
        }
    }

    //This decides if the list should be incremented, decrements, or do nothing.
    private static void UpdateLeftRightMovement(GameTime gameTime)
    {
        if (PREVIOUS_STATE.ThumbSticks.Left.X >= 0.15f)
        {
            Increment(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.ThumbSticks.Left.X < 0.15f);
        }
        else if (PREVIOUS_STATE.ThumbSticks.Left.X <= -0.15f)
        {
            Decrement(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.ThumbSticks.Left.X > -0.15f);
        }
        else if (PREVIOUS_STATE.ThumbSticks.Right.X >= 0.15f)
        {
            Increment(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.ThumbSticks.Right.X < 0.15f);
        }
        else if (PREVIOUS_STATE.ThumbSticks.Right.X <= -0.15f)
        {
            Decrement(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.ThumbSticks.Right.X > -0.15f);
        }
        else if (PREVIOUS_STATE.DPad.Right == ButtonState.Pressed)
        {
            Increment(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.DPad.Right == ButtonState.Released);
        }
        else if (PREVIOUS_STATE.DPad.Left == ButtonState.Pressed)
        {
            Decrement(gameTime.ElapsedGameTime.Milliseconds, CURRENT_STATE.DPad.Left == ButtonState.Released);
        }
    }
}