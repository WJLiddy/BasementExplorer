using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class Player : Creature
{
    Color playerColor;

    public Player(Color c, int x, int y) : base('@',x,y,3,3,3)
    {
        playerColor = c;
    }

    public override void Draw(PixelFont f, AD2SpriteBatch sb)
    {
        if (HP > 0)
            f.Draw(sb, Symbol.ToString(), BasementExplorer.MapXOffset + X, Y, playerColor);
    }

    public void Input(KeyboardState ks)
    {
        //check diags
        if (ks.IsKeyDown(Keys.Left) && ks.IsKeyDown(Keys.Up))
        {
            VelocityDirection = Direction.NW;
            Velocity = 700;
        }
        else if (ks.IsKeyDown(Keys.Right) && ks.IsKeyDown(Keys.Up))
        {
            VelocityDirection = Direction.NE;
            Velocity = 700;
        }
        else if (ks.IsKeyDown(Keys.Left) && ks.IsKeyDown(Keys.Down))
        {
            VelocityDirection = Direction.SW;
            Velocity = 700;
        }
        else if (ks.IsKeyDown(Keys.Right) && ks.IsKeyDown(Keys.Down))
        {
            VelocityDirection = Direction.SE;
            Velocity = 700;
        }

        // Check Cardinal
        else if (ks.IsKeyDown(Keys.Left))
        {
            VelocityDirection = Direction.W;
            Velocity = 700;
        }
        else if (ks.IsKeyDown(Keys.Right))
        {
            VelocityDirection = Direction.E;
            Velocity = 700;
        }
        else if (ks.IsKeyDown(Keys.Up))
        {
            VelocityDirection = Direction.N;
            Velocity = 700;
        }
        else if (ks.IsKeyDown(Keys.Down))
        {
            VelocityDirection = Direction.S;
            Velocity = 700;
        }
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
