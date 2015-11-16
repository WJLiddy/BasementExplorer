using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class BasementExplorer : AD2Game
{
    PixelFont IBMFont;

    // Game Dims.
    public static readonly int BaseWidth = 400;
    public static readonly int BaseHeight = 300;

    public BasementExplorer() : base(BaseWidth, BaseHeight, 60)
    {
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
        IBMFont.Draw(primarySpriteBatch, " !\"#$%&'()*+,-./",2,2,Color.White);
        IBMFont.Draw(primarySpriteBatch, "0123456789:;<=>?", 2, 10, Color.White);
        IBMFont.Draw(primarySpriteBatch, "@ABCDEFGHIJKLMNO", 2, 18, Color.White);
        IBMFont.Draw(primarySpriteBatch, "PQRSTUVWXYZ[\\]^_", 2, 26, Color.White);
        IBMFont.Draw(primarySpriteBatch, "`abcdefghijklmno", 2, 34, Color.White);
        IBMFont.Draw(primarySpriteBatch, "pqrstuvwxyz{|}~", 2, 42, Color.White);

        Utils.drawRect(primarySpriteBatch, 50, 50, 1, 1, Color.Red);
        Utils.drawRect(primarySpriteBatch, 50, 60, 5, 5, Color.Green);
        Utils.drawRect(primarySpriteBatch, 50, 70, 1, 5, Color.Blue);
    
        Utils.drawRect(primarySpriteBatch, 50, 80, 5, 1, Color.White);
        Utils.drawRect(primarySpriteBatch, 50, 90, 10, 10, Color.PaleGoldenrod);
    }

    protected override void AD2LoadContent()
    {
        IBMFont = new PixelFont("fonts/IBMCGA.xml"); 
    }
}

