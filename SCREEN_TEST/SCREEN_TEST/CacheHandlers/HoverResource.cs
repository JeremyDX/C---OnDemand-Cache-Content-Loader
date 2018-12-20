using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class HoverResource
{
    private Texture2D image;
    public Vector2 location;
    private Color color;

    public HoverResource(CacheReader stream)
    {
        image = stream.ReadImage();
        color = stream.ReadColor();
    }

    public int getWidth()
    {
        return image.Bounds.Width;
    }

    public int getHeight()
    {
        return image.Bounds.Height;
    }

    public Texture2D getImage() 
    {
        return image;
    }

    public Color getColor()
    {
        return color;
    }
}