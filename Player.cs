using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Player : Creature
{
    public Color MainColor;
    public Color DarkColor;

    public Player(string name, Color main, Color dark, int x, int y) : base(name,'@',x,y,3,3,3)
    {
        MainColor = main;
        DarkColor = dark;
    }

    public override void Draw(PixelFont f, AD2SpriteBatch sb)
    {
        f.Draw(sb, Symbol.ToString(), BasementExplorer.MapXOffset + X, BasementExplorer.MapYOffset + Y, (HP > 0) ? MainColor : DarkColor);
    }

    public void InputWalkDirection(KeyboardState ks)
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

    //How to ensure that an enemy, when colliding with you, only gets hit once? 
    //We say that all creatures 

    //To prevent multi-striking, 
    protected override bool Interact(Entity e, Direction lastMoveStepDirection)
    {
        if(e is Enemy && ((Enemy)e).HP > 0)
        {
            combat((Enemy)e,lastMoveStepDirection);
            return true;
        }
        return false;
    }

    public override int MeleeDamage()
    {
        return 2;
    }
}
