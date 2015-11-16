using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class BasementExplorer : AD2Game
{
    PixelFont IBMFont;
    OcclusionMap TestMap;

    // Some LinkedLists holding all of our entities. This is is not mutually exclusive. 
    // That is things can be in multiple lists.
    Player P;
    LinkedList<Entity> Entities;
    LinkedList<Creature> AliveCreatures;
    LinkedList<Creature> DeadCreatures;
    LinkedList<Item> ItemsOnGround;

    // Game Dims.
    public static readonly int BaseWidth = 400;
    public static readonly int BaseHeight = 300;

    public static int MapXOffset = 70;

    //Milliseconds. 1000 / 50 = 20
    public BasementExplorer() : base(BaseWidth, BaseHeight, 20)
    {
        Renderer.Resolution = Renderer.ResolutionType.FullScreen;
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
        //Move the player based on Key
        P.Input(keyboardState);
        foreach (Entity e in Entities)
        {
            e.Update(Entities, TestMap);
        }

        LinkedList<Creature> JustKilledCreatures = new LinkedList<Creature>();

        foreach (Creature e in AliveCreatures)
        {
            if(e.HP <= 0)
                JustKilledCreatures.AddFirst(e);
        }

        foreach (Creature e in JustKilledCreatures)
        {
            AliveCreatures.Remove(e);
            DeadCreatures.AddFirst(e);
        }
    }

    protected override void AD2Draw(AD2SpriteBatch primarySpriteBatch)
    {
        LinkedList<int[]> coords = new LinkedList<int[]>();
        int[] coord = new int[] { P.X + (P.Size / 2), P.Y + (P.Size / 2) };
        coords.AddFirst(coord);


        /**
        IBMFont.Draw(primarySpriteBatch, " !\"#$%&'()*+,-./",2,2,Color.White);
        Utils.drawRect(primarySpriteBatch, 50, 60, 5, 5, Color.Green);
        */
        //TODO: Don't like this behavior of map.
        TestMap.DrawBase(primarySpriteBatch,-70,0);



        foreach (Creature e in DeadCreatures)
        {
            e.Draw(IBMFont, primarySpriteBatch);
        }

        foreach (Creature e in AliveCreatures)
        {
            e.Draw(IBMFont, primarySpriteBatch);
        }


        TestMap.RenderRoofs(primarySpriteBatch, TestMap.getLOS(coords,-70,0), -70, 0);
        //GUI
        Utils.drawRect(primarySpriteBatch, 0, 0, 70,130, Color.Blue);
        Utils.drawRect(primarySpriteBatch, 0, 130, 70, 130, Color.Green);
        Utils.drawRect(primarySpriteBatch, 260 + 70, 0, 70, 130, Color.Purple);
        Utils.drawRect(primarySpriteBatch, 260 + 70, 130, 70, 130, Color.Orange);

        //Console
        Utils.drawRect(primarySpriteBatch, 0, 260, 400, 40, Color.Black);



    }

    protected override void AD2LoadContent()
    {
        P = new Player(Color.Blue, 100, 100);
        IBMFont = new PixelFont("fonts/IBMCGA.xml");
        TestMap = new OcclusionMap("testmap/testmap.xml",BaseWidth,BaseHeight);

        //functionify this
        Entities = new LinkedList<Entity>();
        AliveCreatures = new LinkedList<Creature>();
        DeadCreatures = new LinkedList<Creature>();

        Rodent r = new Rodent('h', 30, 30, 0, 20);
        Entities.AddFirst(P);
        Entities.AddLast(r);

        AliveCreatures.AddFirst(P);
        AliveCreatures.AddLast(r);
    }
}

