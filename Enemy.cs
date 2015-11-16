using Microsoft.Xna.Framework;
using System;

abstract class Enemy : Creature
{
    public Enemy(char symbol, int x, int y, int str, int dex, int aff) : base(symbol, x, y, str, dex, aff)
    {

    }

    public override void Draw(PixelFont f,AD2SpriteBatch sb)
    {
        if (HP > 0)
            f.Draw(sb, Symbol.ToString(), X, Y, Color.Red);
        else
            f.Draw(sb, Symbol.ToString(), X, Y, Color.DarkRed);

    }

}
