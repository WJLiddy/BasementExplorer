using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class BasementExplorer : AD2Game
{
    PixelFont IBMFont;
    OcclusionMap TestMap;
    Player P;

    // Game Dims.
    public static readonly int BaseWidth = 400;
    public static readonly int BaseHeight = 300;

    public static int MapXOffset = 70;

    //Milliseconds. 1000 / 50 = 20
    public BasementExplorer() : base(BaseWidth, BaseHeight, 20)
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
        P.FooMove(keyboardState);
        //TODO: Name to update.
        P.Move(new LinkedList<Entity>(), TestMap);
    }

    protected override void AD2Draw(AD2SpriteBatch primarySpriteBatch)
    {
        LinkedList<int[]> coords = new LinkedList<int[]>();
        int[] coord = new int[] { P.X + (P.Size / 2), P.Y + (P.Size / 2)};
        coords.AddFirst(coord);
        /**
        IBMFont.Draw(primarySpriteBatch, " !\"#$%&'()*+,-./",2,2,Color.White);
        Utils.drawRect(primarySpriteBatch, 50, 60, 5, 5, Color.Green);
        */
        //TODO: Don't like this behavior of map.
        TestMap.DrawBase(primarySpriteBatch,-70,0);


        TestMap.RenderRoofs(primarySpriteBatch, TestMap.getLOS(coords,-70,0), -70, 0);
        //GUI
        Utils.drawRect(primarySpriteBatch, 0, 0, 70,130, Color.Blue);
        Utils.drawRect(primarySpriteBatch, 0, 130, 70, 130, Color.Green);
        Utils.drawRect(primarySpriteBatch, 260 + 70, 0, 70, 130, Color.Purple);
        Utils.drawRect(primarySpriteBatch, 260 + 70, 130, 70, 130, Color.Orange);

        //Console
        Utils.drawRect(primarySpriteBatch, 0, 260, 400, 40, Color.Black);


        P.Draw(IBMFont, primarySpriteBatch);

    }

    protected override void AD2LoadContent()
    {
        P = new Player(Color.Blue, 100, 100);
        IBMFont = new PixelFont("fonts/IBMCGA.xml");
        TestMap = new OcclusionMap("testmap/testmap.xml",BaseWidth,BaseHeight);
    }
}

