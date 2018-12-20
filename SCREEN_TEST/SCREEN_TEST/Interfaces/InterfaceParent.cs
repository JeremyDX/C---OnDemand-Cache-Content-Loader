using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class InterfaceParent
{
    public HoverResource hoverResouce;
    public short hoverIndex = -1;
    private InterfaceChild[] children;
    private Texture2D image;
    private byte valType;
    private Vector2 location;
    private Color color;

    public InterfaceParent(Texture2D image, CacheReader stream)
    {
        this.image = image;
        valType = stream.ReadByte();
        location = new Vector2();
        if (image != null)
        {
            location = GameScreen.getAlignmentVector(image.Bounds, valType, ((valType >> 0x0) & 0x3) < 0x2 ? stream.ReadShort() : -stream.ReadShort(), 
                ((valType >> 0x2) & 0x3) < 0x2 ? stream.ReadShort() : -stream.ReadShort());
        }
        color = stream.ReadColor();
        children = new InterfaceChild[stream.ReadByte()];
        for (int i = 0; i < children.Length; ++i)
        {
            children[i] = new InterfaceChild(stream, this);
        }
    }

    public void setHoverResource(short index)
    {
        if (hoverIndex == index)
            return;
        InterfaceChild child = children[index];
        if (child.hoverId == -1)
        {
            hoverResouce = null;
            return;
        }
        hoverResouce = GameCache.GetHoverDefinition(child.hoverId);
        hoverResouce.location = child.location;
        hoverResouce.location.X += 0.5f * (hoverResouce.getWidth() - child.getWidth());
        hoverResouce.location.X += 0.5f * (hoverResouce.getHeight() - child.getHeight());
        hoverIndex = index;
    }

    public Texture2D getImage()
    {
        return image;
    }

    public Rectangle getBounds()
    {
        return image.Bounds;
    }

    public Vector2 getLocation()
    {
        return location;
    }

    public int getType()
    {
        return (valType >> 0x4) & 0x3;
    }

    public Color getColor()
    {
        return color;
    }

    public InterfaceChild[] getChildren()
    {
        return children;
    }

    public InterfaceChild getChild(int index)
    {
        return children[index];
    }
}