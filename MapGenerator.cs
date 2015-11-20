using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

class MapGenerator
{
    public static readonly Color FloorColor = new Color(66,44,44);
    public static readonly Color WallColor = new Color(44,44,44);
 
    public static OcclusionMap Generate()
    {
        //Make room for edges which are always walls.
        bool[,] space = new bool[260 - 2, 260 - 2];

        for (int i = 0; i != 200; i++)
        {
            for (int j = 0; j != 200; j++)
            {
                space[i, j] = true;
            }
        }

        Texture2D Map = new Texture2D(Renderer.GraphicsDevice,260, 260);

        Color[] color = new Color[260 * 260];

        for (int i = 0; i != 260; i++)
        {
            for (int j = 0; j != 260; j++)
            {
                color[i * 260 + j] = ( i != 0 && j != 0 && i != 259 && j != 259 && space[i-1,j-1] ? FloorColor : WallColor);
            }
        }

        Map.SetData(color);
        Stream stream = File.Create("test.png");
        Map.SaveAsPng(stream, 260, 260);
        return null;
    }
}
