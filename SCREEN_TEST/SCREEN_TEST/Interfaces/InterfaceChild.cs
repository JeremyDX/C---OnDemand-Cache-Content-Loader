using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class InterfaceChild
{
    private InterfaceParent parent;
    private Texture2D image = null;
    public char[] text;
    public byte scaleFactor = 100;
    private byte resourceId = 0;
    public short hoverId;
    private int valType;
    public Vector2 location;
    public Color color;

    public InterfaceChild(CacheReader stream, InterfaceParent parent)
    {
        Rectangle rect = Rectangle.Empty;
        valType = stream.ReadInt();
        if (getType() == 0x0)
        {
            int capacity = 0;
            if ((capacity = stream.ReadInt()) > 0)
            {
                image = stream.ReadImage(capacity);
                rect = image.Bounds;
            }
        }
        else if (getType() == 0x1)
        {
            resourceId = (byte)stream.ReadByte();
            scaleFactor = (byte)stream.ReadByte();
            text = stream.ReadCharArray();
            rect.Width = GameCache.GetFontDefinition(resourceId).SecureMeasureString(text);
            rect.Height = GameCache.GetFontDefinition(resourceId).height;
        }
        location = GameScreen.getAlignmentVector(rect, valType, getAlignX() < 0x2 ? stream.ReadSignedShort() : -stream.ReadSignedShort(),
                getAlignY() < 0x2 ? stream.ReadSignedShort() : -stream.ReadSignedShort());
        color = stream.ReadColor();
        hoverId = (short)stream.ReadShort();
        this.parent = parent;
    }

    public int getWidth()
    {
        if (image == null)
            return GameCache.GetFontDefinition(resourceId).SecureMeasureString(text);
        return image.Bounds.Width;
    }

    public int getHeight()
    {
        if (image == null)
            return GameCache.GetFontDefinition(resourceId).height;
        return image.Bounds.Height;
    }

    public Color getColor()
    {
        return color;
    }

    public Texture2D getImage()
    {
        return image;
    }

    public byte getResourceId()
    {
        return resourceId;
    }

    public byte getScaleFactor()
    {
        return scaleFactor;
    }

    public char[] getText()
    {
        return text;
    }

    public int getAlignX()
    {
        return (valType >> 0x0) & 0x3;
    }

    public int getAlignY()
    {
        return (valType >> 0x2) & 0x3;
    }

    public int getType()
    {
        return (valType >> 0x4) & 0x3;
    }

    public Vector2 getLocation()
    {
        Vector2 medium = location;
        return medium += parent.getLocation();
    }
}