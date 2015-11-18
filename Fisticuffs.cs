using Microsoft.Xna.Framework;
using System;

public class Fisticuffs : PrimaryWeapon
{
    //Special weapon used when no item eq'd. Dirty Implementation.
    public Fisticuffs() : base("Nothing",'?',Color.White,-1,-1,-1,-1,-1,-1)
    {

    }

    new public int Accuracy(Creature c)
    {
        //Make it based on int.
        return 50 + (c.Aff / 2);
    }


    public override int HitDamage(Creature owner, Creature target)
    {
        return 1 + (int)(Utils.RandomNumber()*owner.Str);
    }

    public override int Power(Creature owner)
    {
        return owner.Str;
    }

    public override string WarningMessage(Creature c)
    {
        return "";
    }


    }
