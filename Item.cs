abstract class Item : Entity
{
    bool onFloor = true;

    public Item(string name, char symbol, int x, int y, int size) : base (name, symbol,x,y,size)
    {

    }
}
