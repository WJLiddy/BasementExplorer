using System;
using Microsoft.Xna.Framework;

class Player : Creature
{
    Color playerColor;

    public Player(Color c, int x, int y) : base('@',x,y,3,3,3)
    {

    }

    public override void Draw(PixelFont f, AD2SpriteBatch sb)
    {
        if(HP > 0)
            f.Draw(sb, Symbol.ToString(), X, Y, playerColor);
    }

    //How to ensure that an enemy, when colliding with you, only gets hit once? 
    //We say that all creatures 

    //To prevent multi-striking, 
    protected override bool Interact(Entity e)
    {
        if(e is Enemy)
        {
            combat((Enemy)e);
            return true;
        }
        return false;
    }

    public override int MeleeDamage()
    {
        return 2;
    }
}
