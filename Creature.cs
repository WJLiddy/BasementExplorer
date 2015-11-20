using System;
using System.Collections.Generic;

abstract public class Creature : Entity
{
    public static readonly int MaxLevel = 100;

    public static readonly int BaseHP = 9;
    public static readonly int HPPerStr = 4;

    public static readonly int MinMoveSpeed = 400;
    public static readonly int MaxMoveSpeed = 900;

    public static readonly int MinKnockback = 1000;
    public static readonly int MaxKnockback = 8000;

    public int HP { get; protected set; }

    public PrimaryWeapon PrimaryWeapon { get; protected set; }


    private LinkedList<Observer> Observers;

    //100-level-system
    public int Str { get; protected set; }
    public int Dex { get; protected set; }
    public int Aff { get; protected set; }


    //All creatures are respresented by ascii letters, so as a result, they all have size 8.
    public Creature(string name, char symbol, int x, int y, int str, int dex, int aff) : base(name, symbol, x, y, 8)
    {
        Observers = new LinkedList<Observer>();
        Str = str;
        Dex = dex;
        Aff = aff;
        HP = MaxHP();
        PrimaryWeapon = null;
    }

    public void Hurt(Creature source, int damage)
    {

        HP -= damage;
        source.Notify("Hit " + Name + " for " + damage + ".");
        Notify("Took " + damage + " from " + source.Name + ".");

        if (HP <= 0)
        {
            Notify("Slain by " + source.Name + "!");
            source.Notify(Name + " has been slain!");
        }
    }

    public void KnockBack(int damage, Direction velocityDirection)
    {
        // Knock Back is % of damage. Max is 1.0.
        double knockBackRatio = Math.Min(1.0, (double)damage / MaxHP());

        VelocityDirection = velocityDirection;

        //The maximum launch speed is ~8000 px, to avoid skipping over things.
        //And we want a minimum knockback so you actually do get knocked back a bit.
        Velocity = MinKnockback + (int)((MaxKnockback - MinKnockback) * knockBackRatio);
    }

    public void combat(Creature e, Direction lastMoveStepDirection)
    {
        // Step back to the square we were in before.
        UndoMove(lastMoveStepDirection);

        // Let us try to fetch the two accuracies first.
        bool thisHit = PrimaryWeapon == null ? PunchAccuracyRoll() : PrimaryWeapon.AccuracyRoll(this);
        bool enemyHit = e.PrimaryWeapon == null ? e.PunchAccuracyRoll() : e.PrimaryWeapon.AccuracyRoll(this);


        if (!thisHit && !enemyHit)
            //No one hit anyone. So return. 
            return;

        if (thisHit)
        { 
            int thisMeleeDamage = PrimaryWeapon == null ? PunchPowerRoll() : PrimaryWeapon.PowerRoll(this, e);
            e.Hurt(this, thisMeleeDamage);
            e.KnockBack(thisMeleeDamage, VelocityDirection);
        } else
        {
            Notify("Missed " + e.Name + ".");
            e.Notify(Name + " Missed.");
        }


        if (e.HP > 0)
        {
            if (enemyHit)
            {
                int enemyMeleeDamage = e.PrimaryWeapon == null ? e.PunchPowerRoll() : e.PrimaryWeapon.PowerRoll(e, this);
                Hurt(e, enemyMeleeDamage);
                KnockBack(enemyMeleeDamage, Opposite(VelocityDirection));
            } else
            {
                e.Notify("Missed " + Name + ".");
                Notify(e.Name + " Missed.");
            }
        }
    }

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
        return BaseHP + (Str * HPPerStr);
    }

    public int Speed()
    {
        return MinMoveSpeed + (Dex * ((MaxMoveSpeed - MinMoveSpeed) / MaxLevel));
    }

    public void Walk(Direction d)
    {
        //Assume we have control
        //TODO: change: do not walk if dead.
        if (Velocity <= Speed() && HP > 0)
        {
            VelocityDirection = d;
            Velocity = Speed();
        }
    }

    public void AddObserver(Observer o)
    {
        Observers.AddFirst(o);
    }

    public void Notify(string message)
    {
        foreach (Observer o in Observers)
        {
            o.Observe(message);
        }
    }


    public void Notify(string message, Object r)
    {
        foreach (Observer o in Observers)
        {
            o.Observe(message, r);
        }
    }

    public void Heal(int addHP)
    {
        HP += addHP;
        HP = Math.Min(HP, MaxHP());
    }

    public int PunchPower()
    { 
        return Str;
    }


    public int PunchAccuracy()
    {
        //TODO HARDCODED
        return 75;
    }

    public bool PunchAccuracyRoll()
    {
        // 1 - 100 sided dice.
        return 1 + (int)(Utils.RandomNumber() * 100) <= PunchAccuracy();
    }

    public int PunchPowerRoll()
    {
        return 1 + (int)(Utils.RandomNumber() * PunchPower());
    }
}
