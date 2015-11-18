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
    HUD H;
    LinkedList<Entity> Entities;
    LinkedList<Creature> AliveCreatures;
    LinkedList<Creature> DeadCreatures;
    LinkedList<Item> ItemsOnGround;

    // Game Dims.
    public static readonly int BaseWidth = 400;
    public static readonly int BaseHeight = 300;

    public static int MapXOffset = 70;
    public static int MapYOffset = 20;

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

        H.Update();

        //Move the player based on Key
        P.InputWalkDirection(keyboardState);
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

        //TODO: Don't like this behavior of map.
        TestMap.DrawBase(primarySpriteBatch,-MapXOffset,-MapYOffset);

        foreach (Creature e in DeadCreatures)
        {
            e.Draw(IBMFont, primarySpriteBatch);
        }

        foreach (Item i in ItemsOnGround)
        {
            i.Draw(IBMFont, primarySpriteBatch);
        }

        foreach (Creature e in AliveCreatures)
        {
            e.Draw(IBMFont, primarySpriteBatch);
        }


        //THIS is UGLY.
        TestMap.RenderRoofs(primarySpriteBatch, TestMap.getLOS(coords,-MapXOffset,-MapYOffset), -MapXOffset, -MapYOffset);
        
        //Draw borders around the map.
        Utils.drawRect(primarySpriteBatch, 0, 0, 70, 300, Color.Black);
        Utils.drawRect(primarySpriteBatch, 400 - 70, 0, 70, 300, Color.Black);
        Utils.drawRect(primarySpriteBatch, 0, 0, 400, 20, Color.Black);
        Utils.drawRect(primarySpriteBatch, 0, 280, 400, 20, Color.Black);

        H.Draw(IBMFont, primarySpriteBatch);
    }

    protected override void AD2LoadContent()
    {
        P = new Player("Blue",Color.Blue, new Color(0, 0, 100, 255), 100, 100);
        H = new HUD(1, P);
        P.AddObserver(H);
        IBMFont = new PixelFont("fonts/IBMCGA.xml");
        TestMap = new OcclusionMap("testmap/testmap.xml",BaseWidth,BaseHeight);

        //functionify this
        Entities = new LinkedList<Entity>();
        AliveCreatures = new LinkedList<Creature>();
        DeadCreatures = new LinkedList<Creature>();
        ItemsOnGround = new LinkedList<Item>();

        AddCreature(P);
        AddCreature(new Rodent("Hamster",'h', 30, 30, 0, 20));
        AddCreature(new Rodent("Gerbil", 'g', 40, 30, 2, 20));
        AddItem(new PrimaryCrit("Stick", '/', Color.Brown, 0, 10, 0, 4, 30, 50, 10));
        //A god pwns you.
        AddCreature(new Rodent("God",'G', 80, 200, 100, 100));

    }

    private void AddCreature(Creature c)
    {
        Entities.AddLast(c);
        AliveCreatures.AddFirst(c);
    }

    private void AddItem(Item i)
    {
        Entities.AddLast(i);
        ItemsOnGround.AddFirst(i);
    }
}

