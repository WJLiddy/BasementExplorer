using Microsoft.Xna.Framework;

public abstract class Item : Entity
{
    public bool OnFloor = true;
    public Color Color;

    public Item(string name, char symbol, Color color, int x, int y, int size) : base (name, symbol,x,y,size)
    {
        Color = color;
    }
}
