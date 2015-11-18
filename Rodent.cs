class Rodent : Enemy
{
    public Rodent(string name, char symbol, int x, int y, int str, int dex) : base(name,symbol,x,y,str,dex,0)
    {
        
    }

    protected override bool Interact(Entity e, Direction lastMoveStepDirection)
    {
        // Rodents cannot yet attack so do nothing.
        return false;
    }
}
