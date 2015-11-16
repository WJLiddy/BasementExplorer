abstract class Creature : Entity
{
    public static readonly int HPPerStr = 3;
    protected int HP;

    protected int Str;
    protected int Dex;
    protected int Aff;


    //All creatures are respresented by ascii letters, so as a result, they all have size 8.
    public Creature (char symbol, int x, int y, int str, int dex, int aff) : base(symbol,x,y,8)
    {
        Str = str;
        Dex = dex;
        Aff = aff;
        HP = Str * HPPerStr;
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
    }

    public void combat(Creature e, Direction lastMoveStepDirection)
    {
        UndoMove(lastMoveStepDirection);
        //Now, have the creatures hurt each other.
        e.Hurt(MeleeDamage());
        Hurt(e.MeleeDamage());
        //Finally, do knockback. Creatures are knockback based.
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
}
