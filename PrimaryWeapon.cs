using Microsoft.Xna.Framework;
using System;

public class PrimaryWeapon : Item
{
    private class PenaltySet
    {
        public enum Stat { Str,Dex,Aff }

        public int StrPenalty { get; private set; } 
        public int DexPenalty { get; private set; }
        public int IntPenalty { get; private set; }

        public PenaltySet(PrimaryWeapon i, Creature c)
        {
            StrPenalty = Math.Max(0, (i.StrReq - c.Str) * (i.StrReq - c.Str));
            DexPenalty = Math.Max(0, (i.DexReq - c.Dex) * (i.DexReq - c.Dex));
            IntPenalty = Math.Max(0, (i.AffReq - c.Aff) * (i.AffReq - c.Aff));
        }

        public int TotalPenalty()
        {
            return StrPenalty + DexPenalty + IntPenalty;
        }

        public Stat LimitingStat()
        {
            int limitingStat = Math.Max(StrPenalty, Math.Max(DexPenalty, IntPenalty));

            if (limitingStat == StrPenalty)
                return Stat.Str;
            if (limitingStat == DexPenalty)
                return Stat.Dex;
            if (limitingStat == IntPenalty)
                return Stat.Aff;

            Utils.Log("Error: failed LimitingStat calculation for primaryWeapon");
            return Stat.Str;
        }
    }

    public static readonly int PenaltyWarningThreshold = 10;

    //requirements to use at 100 accuracy
    public int StrReq;
    public int DexReq;
    public int AffReq;
    protected int MaxPower;

   
    public PrimaryWeapon(string name, char symbol, Color color, int strReq, int dexReq, int affReq, int power, int x,int y) : base(name,symbol,color,x,y,8)
    {
        StrReq = strReq;
        DexReq = dexReq;
        AffReq = affReq;
        MaxPower = power;
    }

    public int Accuracy(Creature c)
    {
        PenaltySet p = new PenaltySet(this, c);
        return Math.Max(1, 99 - p.TotalPenalty());
    }

    public virtual string WarningMessage(Creature c)
    {
        PenaltySet p = new PenaltySet(this, c);
        if (p.TotalPenalty() < PenaltyWarningThreshold)
            return "";
        switch(p.LimitingStat())
        {
            case PenaltySet.Stat.Str:
                return "Low STR";
            case PenaltySet.Stat.Dex:
                return "Low DEX";
            case PenaltySet.Stat.Aff:
                return "Low AFF";
        }

        Utils.Log("Error in PrimaryWeapon: Warning Message");
        return "";
    }


    public override void Draw(PixelFont f, AD2SpriteBatch sb)
    {
        if (OnFloor)
            f.Draw(sb, Symbol.ToString(), BasementExplorer.MapXOffset +  X, BasementExplorer.MapYOffset + Y, this.Color);
    }

    protected override bool Interact(Entity e, Direction lastMoveStepDirection)
    {
        //item does not interact with anything. It just sits there on the ground.
        return false;
    }

    public virtual string SpecialMessage()
    {
        //Nothing special about a basic primary weapon.
        return "";
    }

    public virtual int PowerRoll(Creature owner, Creature target)
    {
        return 1 + (int)(Utils.RandomNumber() * MaxPower);
    }

    public int Power(Creature c)
    {
        return MaxPower;
    }

    public virtual bool AccuracyRoll(Creature owner)
    {
        return (1 + (int)(Utils.RandomNumber() * 100)) <= Accuracy(owner);
    }

    public void PlayStrikeSound()
    {
        SoundManager.Play("blunt.wav");
    }
}
