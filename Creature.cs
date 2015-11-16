abstract class Creature : Entity
{
    public static readonly int BaseHP = 9;
    public static readonly int HPPerStr = 4;
    public int HP { get; protected set; }

    protected int Str;
    protected int Dex;
    protected int Aff;


    //All creatures are respresented by ascii letters, so as a result, they all have size 8.
    public Creature (char symbol, int x, int y, int str, int dex, int aff) : base(symbol,x,y,8)
    {
        Str = str;
        Dex = dex;
        Aff = aff;
        HP = MaxHP(); 
    }

    public void Hurt(int damage)
    {
        HP -= damage;
    }

    public void KnockBack(int damage, Direction velocityDirection)
    {
        // Knock Back is % of damage.
        double knockBackRatio = ((double)damage) / ((double)(Str * HPPerStr));

        VelocityDirection = velocityDirection;
        Velocity = 8000;// * ((int)knockBackRatio);
    }

    public void combat(Creature e, Direction lastMoveStepDirection)
    {
        // Step back to the square we were in before.
        UndoMove(lastMoveStepDirection);
        // Have the creatures hurt each other.
        e.Hurt(MeleeDamage());
        Hurt(e.MeleeDamage());
        // Finally, do knockback.
        e.KnockBack(MeleeDamage(), VelocityDirection);
        KnockBack(e.MeleeDamage(), Entity.Opposite(VelocityDirection));
    }

    public abstract int MeleeDamage();

    private void UndoMove(Direction lastMoveStep)
    {
        //Step 1: undo the move by the instigating creature.
        if (lastMoveStep == Direction.N)
        {
            Y++;
            DeltaY = DeltaScale / 2;
        }

        if (lastMoveStep == Direction.S)
        {
            Y--;
            DeltaY = DeltaScale / 2;
        }

        if (lastMoveStep == Direction.W)
        {
            X++;
            DeltaY = DeltaScale / 2;
        }

        if (lastMoveStep == Direction.E)
        {
            X--;
            DeltaX = DeltaScale / 2;
        }
    }

    public int MaxHP()
    {
        return BaseHP + (Str* HPPerStr);
    }
}
