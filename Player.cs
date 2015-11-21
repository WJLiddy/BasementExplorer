using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Player : Creature
{
    public static readonly int InventorySize = 6;

    public Color MainColor;
    public Color DarkColor;
    private Item[] Inventory;
    private int inventoryPointer = 0;
    private bool pickUpFlag = false;
    private bool pickUpHeld = false;

    public Player(string name, Color main, Color dark, int x, int y) : base(name,'@',x,y,3,3,3)
    {
        MainColor = main;
        DarkColor = dark;
        Inventory = new Item[InventorySize];
    }

    public override void Draw(PixelFont f, AD2SpriteBatch sb)
    {
        f.Draw(sb, Symbol.ToString(), BasementExplorer.MapXOffset + X, BasementExplorer.MapYOffset + Y, (HP > 0) ? MainColor : DarkColor);
    }

    public void Input(KeyboardState ks)
    {
        InputWalkDirection(ks);
        InputPickUp(ks);
    }
    private void InputWalkDirection(KeyboardState ks)
    {
        //check diags
        if (ks.IsKeyDown(Keys.Left) && ks.IsKeyDown(Keys.Up))
            Walk(Direction.NW);
        else if (ks.IsKeyDown(Keys.Right) && ks.IsKeyDown(Keys.Up))
            Walk(Direction.NE);
        else if (ks.IsKeyDown(Keys.Left) && ks.IsKeyDown(Keys.Down))
            Walk(Direction.SW);
        else if (ks.IsKeyDown(Keys.Right) && ks.IsKeyDown(Keys.Down))
            Walk(Direction.SE);
        else if (ks.IsKeyDown(Keys.Left))
            Walk(Direction.W);
        else if (ks.IsKeyDown(Keys.Right))
            Walk(Direction.E);
        else if (ks.IsKeyDown(Keys.Up))
            Walk(Direction.N);
        else if (ks.IsKeyDown(Keys.Down))
            Walk(Direction.S);
        else
            Velocity = 0;
    }

    private void InputPickUp(KeyboardState ks)
    {
        if(ks.IsKeyDown(Keys.Z))
        {
           pickUpFlag = !pickUpHeld;
        }

        pickUpHeld = ks.IsKeyDown(Keys.Z);
    }

    protected override bool Interact(Entity e, Direction lastMoveStepDirection)
    {
        if(e is Enemy && ((Enemy)e).HP > 0)
        {
            combat((Enemy)e,lastMoveStepDirection);
            return true;
        }

        if(e is PrimaryWeapon && ((PrimaryWeapon)e).OnFloor)
        {
            Notify("Standing on", e);
            if(pickUpFlag)
            {
                if(PrimaryWeapon != null)
                    PrimaryWeapon.Drop(X, Y);

                PrimaryWeapon = ((PrimaryWeapon)e);
                PrimaryWeapon.PickUp();
                Notify("Picked up a " + PrimaryWeapon.Name);
                return true;
            }

        }
        return false;
    }

    public override void playAttackSound()
    {
        if (PrimaryWeapon == null)
            SoundManager.Play("punch.wav");
        else
            PrimaryWeapon.PlayStrikeSound();

    }
}
