using Microsoft.Xna.Framework;

abstract class Enemy : Creature
{
    public Enemy(string name, char symbol, int x, int y, int str, int dex, int aff) : base(name, symbol, x, y, str, dex, aff)
    {

    }

    public override void Draw(PixelFont f,AD2SpriteBatch sb)
    {
        if (HP > 0)
            f.Draw(sb, Symbol.ToString(), BasementExplorer.MapXOffset + X, BasementExplorer.MapYOffset + Y, Color.Red);
        else
            f.Draw(sb, Symbol.ToString(), BasementExplorer.MapXOffset + X, BasementExplorer.MapYOffset + Y, Color.DarkRed);

    }

}
