using System;

abstract class Creature : Entity
{
    public static readonly int MaxLevel = 100;

    public static readonly int BaseHP = 9;
    public static readonly int HPPerStr = 4;

    public static readonly int MinMoveSpeed = 400;
    public static readonly int MaxMoveSpeed = 900;

    public static readonly int MinKnockback = 1000;
    public static readonly int MaxKnockback = 8000;

    public int HP { get; protected set; }

    //100-level-system
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
        // Knock Back is % of damage. Max is 1.0.
        double knockBackRatio = Math.Min(1.0, (double)damage / MaxHP());

        VelocityDirection = velocityDirection;

        //The maximum launch speed is ~8000 px, to avoid skipping over things.
        //And we want a minimum knockback so you actually do get knocked back a bit.
        Velocity = MinKnockback + (int)((MaxKnockback - MinKnockback) * knockBackRatio);
        Utils.Log("Velocity : " + Velocity);
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
        KnockBack(e.MeleeDamage(), Opposite(VelocityDirection));
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

    public int Speed()
    {
        return MinMoveSpeed + (Dex * ((MaxMoveSpeed - MinMoveSpeed) / MaxLevel));
    }

    public void Walk(Direction d)
    {
        //Assume we have control
        if (Velocity <= Speed())
        {
            VelocityDirection = d;
            Velocity = Speed();
        }
    }
}
