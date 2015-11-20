using Microsoft.Xna.Framework;

public abstract class Item : Entity
{
    public bool OnFloor { get; protected set; } = true;
    public Color Color;

    public Item(string name, char symbol, Color color, int x, int y, int size) : base (name, symbol,x,y,size)
    {
        Color = color;
    }

    public void Drop(int x, int y)
    {
        X = x;
        Y = y;
        OnFloor = true;
    }

    public void PickUp()
    {
        OnFloor = false;
    }
}
