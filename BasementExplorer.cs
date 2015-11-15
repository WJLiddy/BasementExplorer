using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class BasementExplorer : AD2Game
{

    // Game Dims.
    public static readonly int BaseWidth = 400;
    public static readonly int BaseHeight = 300;

    public BasementExplorer() : base(BaseWidth, BaseHeight, 60)
    {
        //lol stub constructor
        Renderer.Resolution = Renderer.ResolutionType.WindowedLarge;
    }

    public static bool Collide(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2 )
    {
        return !(x1 > x2 + w2 ||
                y1 > y2 + h2 ||
                x2 > x1 + w1 ||
                y2 > y1 + h1);
    }

    protected override void AD2Logic(int ms, KeyboardState keyboardState, GamePadState[] gamePadState)
    {
        
    }

    protected override void AD2Draw(AD2SpriteBatch primarySpriteBatch)
    {
   
    }

    protected override void AD2LoadContent()
    {
      
    }
}

