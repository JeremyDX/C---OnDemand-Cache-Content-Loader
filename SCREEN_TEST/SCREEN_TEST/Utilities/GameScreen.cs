using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Content;

public class GameScreen
{
    public static GraphicsDeviceManager d3dpp;
    public static Rectangle SAFE_ZONE = new Rectangle(0, 0, 0, 0);

    public static GraphicsDevice getDevice()
    {
        return d3dpp.GraphicsDevice;
    }

    public static void initializeScreen(Engine engine)
    {
        d3dpp = new GraphicsDeviceManager(engine);
        d3dpp.PreferredBackBufferWidth = 3840;
        d3dpp.PreferredBackBufferHeight = 2160;
        d3dpp.PreferredBackBufferFormat = SurfaceFormat.Color;
        d3dpp.ApplyChanges();
        SAFE_ZONE = d3dpp.GraphicsDevice.Viewport.TitleSafeArea;
        int displayX = ceil(1.25 * SAFE_ZONE.Width);
        int displayY = ceil(1.25 * SAFE_ZONE.Height);
        switch (displayY)
        {
            case 960:  //768
                SAFE_ZONE.X = 54;
                break;
            case 1080: //864
                SAFE_ZONE.X = 64;
                break;
            case 1200: //960
                SAFE_ZONE.X = 36;
                break;
            case 1350: //1080
                SAFE_ZONE.X = 36;
                break;
            default:
                SAFE_ZONE.X /= 2;
                break;
        }
        float ratio = displayY / ((displayX * 9.0f) / 16.0f);
        SAFE_ZONE.Y = (int)(SAFE_ZONE.X * ratio);
        SAFE_ZONE.Width = displayX - (SAFE_ZONE.X * 2);
        SAFE_ZONE.Height = displayY - (SAFE_ZONE.Y * 2) + 10;
    }

    public static Vector2 getAlignmentVector(Rectangle rect, int settings, int offX, int offY)
    {
        Vector2 aligned = Vector2.Zero;
        int valX = ((settings >> 0x0) & 0x3);
        int valY = ((settings >> 0x2) & 0x3);
        int xIndex = ((settings >> 0x6) & 0xF);
        int xSpaces = ((settings >> 0xA) & 0xF);
        int yIndex = ((settings >> 0xE) & 0xF);
        int ySpaces = ((settings >> 0x12) & 0xF);
        if (xSpaces > 0x0)
        {
            int gutters = xSpaces + 1;
            float position = xIndex / (float)gutters;
            int available_area = SAFE_ZONE.Center.X << 1;
            position *= available_area;
            position -= rect.Width * 0.5f * (0x2 - valX);
            aligned.X = position + (SAFE_ZONE.X * (~valX + 0x2));
        }
        else
        {
            aligned.X += SAFE_ZONE.X * (~valX + 0x2);
            aligned.X += valX * (((SAFE_ZONE.Center.X << 0x1) - rect.Width) >> 0x1);
        }
        if (ySpaces > 0x0)
        {
            int gutters = ySpaces + 1;
            float position = yIndex / (float)gutters;
            int available_area = SAFE_ZONE.Center.Y << 1;
            position *= available_area;
            position -= rect.Height * 0.5f * (0x2 - valY);
            aligned.Y = position + (SAFE_ZONE.Y * (~valY + 0x2));
        }
        else
        {
            aligned.Y += SAFE_ZONE.Y * (~valY + 0x2);
            aligned.Y += valY * (((SAFE_ZONE.Center.Y << 0x1) - rect.Height) >> 0x1);
        }
        aligned.X += offX;
        aligned.Y += offY;
        return aligned;
    }

    public static int ceil(double value)
    {
        if ((int)value == value)
            return (int)value;
        return (int)value + 1;
    }

    public static string toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Safe Zone: ");
        sb.Append(SAFE_ZONE);
        return sb.ToString();
    }

}