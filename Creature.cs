abstract class Creature : Entity
{
    public static readonly int HPPerStr;
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

    public void combat(Creature e)
    {
        UndoMove();
        //Now, have the creatures hurt each other.
        e.Hurt(MeleeDamage());
        Hurt(e.MeleeDamage());
        //Finally, do knockback. Creatures are knockback based.
        e.KnockBack(MeleeDamage(), VelocityDirection);
        KnockBack(e.MeleeDamage(), Entity.Opposite(VelocityDirection));
    }

    public abstract int MeleeDamage();

    private void UndoMove()
    {
        //Step 1: undo the move by the instigating creature.
        if (VelocityDirection == Direction.N || VelocityDirection == Direction.NW || VelocityDirection == Direction.NE)
        {
            Y--;
            DeltaY = DeltaScale / 2;
        }

        if (VelocityDirection == Direction.S || VelocityDirection == Direction.SW || VelocityDirection == Direction.SE)
        {
            Y++;
            DeltaY = DeltaScale / 2;
        }

        //Step 1: undo the move by the instigating creature.
        if (VelocityDirection == Direction.N || VelocityDirection == Direction.NW || VelocityDirection == Direction.NE)
        {
            Y--;
            DeltaY = DeltaScale / 2;
        }

        if (VelocityDirection == Direction.W || VelocityDirection == Direction.SW || VelocityDirection == Direction.NW)
        {
            X--;
            DeltaX = DeltaScale / 2;
        }


        if (VelocityDirection == Direction.E || VelocityDirection == Direction.SE || VelocityDirection == Direction.NE)
        {
            X++;
            DeltaX = DeltaScale / 2;
        }
    }
}
