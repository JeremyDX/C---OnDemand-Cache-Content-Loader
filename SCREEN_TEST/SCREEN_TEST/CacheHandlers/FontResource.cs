using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class FontResource
{
    private static Rectangle BOUNDS = new Rectangle(0, 0, 0, 0);

    private Texture2D resource;
    private short[] indexing;
    private byte[] widths;
    private byte offset;
    public byte height;

    public FontResource(Texture2D resource, short[] indexing, 
            byte[] widths, byte offset, byte height)
    {
        this.resource = resource;
        this.indexing = indexing;
        this.widths = widths;
        this.offset = offset;
        this.height = height;
    }

    public FontResource(Texture2D resource, CacheReader stream)
    {
        this.resource = resource;
        this.offset = (byte)stream.ReadByte();
        this.height = (byte)stream.ReadByte();
        int length = stream.ReadByte();
        indexing = new short[length];
        widths = new byte[length];
        for (int i = 0; i < length; i++)
        {
            indexing[i] = (short)stream.ReadShort();
            widths[i] = (byte)stream.ReadByte();
        }
    }

    public void Draw(SpriteBatch batch, Vector2 location, string text, Color color)
    {
        Draw(batch, location, text.ToCharArray(), color);
    }

    public void Draw(SpriteBatch batch, Vector2 location, char[] text, Color color)
    {
        float baseX = location.X;
        foreach (char c in text)
        {
            int index = ((int)c - offset) & 0xFF;
            if (index >= indexing.Length)
            {
                switch (c)
                {
                    case '\n':
                        location.X = baseX;
                        location.Y += height;
                        break;
                }
                continue;
            }
            BOUNDS.X = indexing[index];
            BOUNDS.Width = widths[index];
            BOUNDS.Y = 1 + ((index >> 4) * height);
            BOUNDS.Height = height - 1;
            //batch.Draw(resource, location, BOUNDS, color, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            batch.Draw(resource, location, BOUNDS, color);
            location.X += BOUNDS.Width;
        }
    }

    public int MeasureString(string text)
    {
        return MeasureString(text.ToCharArray());
    }

    public int MeasureString(char[] text)
    {
        int size = 0;
        foreach (char c in text)
            size += widths[(int)c - offset];
        return size;
    }

    public byte MeasureCharacter(char c)
    {
        return widths[(int)c - offset];
    }

    public int SecureMeasureString(string text)
    {
        return SecureMeasureString(text.ToCharArray());
    }

    public int SecureMeasureString(char[] text)
    {
        int size = 0;
        foreach (char c in text)
        {
            int index = ((int)c - offset) & 0xFF;
            if (index >= widths.Length)
                size += 0;
            else
                size += widths[(int)c - offset];
        }
        return size;
    }

    public byte SecureMeasureCharacter(char c)
    {
        int index = ((int)c - offset) & 0xFF;
        if (index >= widths.Length)
            return 0;
        return widths[index];
    }
}