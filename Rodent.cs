class Rodent : Enemy
{
    public Rodent(string name, char symbol, int x, int y, int str, int dex) : base(name,symbol,x,y,str,dex,0)
    {
        Tickets = 1 + (int)(Utils.RandomNumber() * Str);
    }

    protected override bool Interact(Entity e, Direction lastMoveStepDirection)
    {
        // Rodents cannot yet attack so do nothing.
        return false;
    }

    public override void playAttackSound()
    {
        if (Utils.RandomNumber() < .5)
            SoundManager.Play("rodent1.wav");
        else
            SoundManager.Play("rodent2.wav");
    }
}
