class Rodent : Enemy
{
    public Rodent(char symbol, int x, int y, int str, int dex) : base(symbol,x,y,str,dex,0)
    {
        
    }

    public override int MeleeDamage()
    {
        return 1 + (Str / 2);
    }

    protected override bool Interact(Entity e, Direction lastMoveStepDirection)
    {
        // Rodents cannot yet attack so do nothing.
        return false;
    }
}
