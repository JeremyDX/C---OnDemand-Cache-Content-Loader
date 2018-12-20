using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class GameCache
{
    private static byte idx = 0;
    public static byte[] screens;
    private static HoverResource[] hoverSprites;
    private static FontResource[] resources;
    private static InterfaceParent[] interfaces;
    private static ScreenInterface[] windows;
    private static int window_state;

    private static int[] CACHE_INDEX_TABLE;

    private static uint CRC_INDEX = 0xA09CD3F;

    private static uint[] CRC_TABLE =
    {
        0xf56f4680, 0xd5bd1a00, 0xab7a3400, 0xab7a3400, 0xe97a800, 0xf0245000, 0x65f52000
    };
    private static uint CHECKSUM = 0x40a90598;

    public static bool InitializeHeader()
    {
        int l;
        windows = new ScreenInterface[0x2];
        window_state = (int)(CRC_INDEX &= LoadGameWindows());
        screens = new byte[0x4];
        CACHE_INDEX_TABLE = new int[0x3];
        for (int i = 0; i < screens.Length; i++)
            screens[i] = (byte)0xFF;
        CHECKSUM *= CacheReader.Open();
        CacheReader stream = CacheReader.SeekCacheSizeShort(0x0);
        if (Verify(stream.Capacity()))
        {
            if (Verify(l = stream.ReadByte()))
            {
                resources = new FontResource[l];
                if (Verify(l = stream.ReadByte()))
                {
                    interfaces = new InterfaceParent[l];
                    if (Verify(l = stream.ReadShort()))
                    {
                        hoverSprites = new HoverResource[l];
                        if (Verify(l = stream.ReadInt()))
                        {
                            CACHE_INDEX_TABLE[0x0] = l;
                            if (Verify(l = stream.ReadInt()))
                            {
                                CACHE_INDEX_TABLE[0x1] = l;
                                if (Verify(l = stream.ReadInt()))
                                {
                                    CACHE_INDEX_TABLE[0x2] = l;
                                }
                            }
                        }
                    }
                }
            }
        }
        return CRC_INDEX == CRC_TABLE.Length;
    }

    private static uint LoadGameWindows()
    {
        windows[0] = new MainStartMenu();
        windows[1] = new ChooseGameMode();
        return (uint)~(0xFFFFFFFFFFFFFFFFL);
    }

    public static string Debug()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("\nDebugState: ");
        sb.Append(CRC_INDEX - 1);
        return sb.ToString();
    }

    private static bool Verify(int value)
    {
        return !(GameCache.CRC_TABLE[GameCache.CRC_INDEX++] != (uint)(GameCache.CHECKSUM *= (uint)value));
    }

    public static bool ShowInterface(int id, bool set)
    {
        if (GetInterfaceDefinition(id) != null)
            screens[0] = (byte)(set ? id : 0xFF);
        return true;
    }

    public static void ShowWindow(int id)
    {
        windows[id].begin();
        window_state = id;
    }

    public static void HandleWindowInput(GameTime gameTime)
    {
        int state = window_state;
        windows[state].handleInput();
        windows[state].update(gameTime);
    }

    public static FontResource GetFontDefinition(int id)
    {
        if (resources[id] == null)
            resources[id] = LoadFontResource(0x0, id);
        return resources[id];
    }

    public static InterfaceParent GetInterfaceDefinition(int id)
    {
        if (interfaces[id] == null)
            interfaces[id] = LoadInterfaceResource(0x1, id);
        return interfaces[id];
    }

    public static HoverResource GetHoverDefinition(int id)
    {
        if (hoverSprites[id] == null)
            hoverSprites[id] = LoadHoverResource(0x2, id);
        return hoverSprites[id];
    }

    public static InterfaceParent Poll()
    {
        if (idx >= screens.Length)
        {
            idx = 0;
            return null;
        }
        if (screens[idx] != 0xFF)
            return interfaces[screens[idx++]];
        ++idx;
        return Poll();
    }

    private static FontResource LoadFontResource(int table, int id)
    {
        CacheReader stream = CacheReader.slice(
            CACHE_INDEX_TABLE[table] + (id * 0x4)
        );
        return new FontResource(stream.ReadImage(), stream);
    }

    private static InterfaceParent LoadInterfaceResource(int table, int id)
    {
        CacheReader stream = CacheReader.slice(
            CACHE_INDEX_TABLE[table] + (id * 0x4)
        );
        int image_size = stream.ReadInt();
        Texture2D image = null;
        if (image_size > 0)
            image = stream.ReadImage(image_size);
        return new InterfaceParent(image, stream);
    }

    private static HoverResource LoadHoverResource(int table, int id)
    {
        return new HoverResource(CacheReader.slice(
            CACHE_INDEX_TABLE[table] + (id * 0x4)
        ));
    }
}