using Microsoft.Xna.Framework;

class PrimaryCrit : PrimaryWeapon
{
    int CritChance;
    public static readonly int CritMultiplier = 2;
    public PrimaryCrit(string name, char symbol, Color c, int strReq, int dexReq, int affReq, int power, int x, int y, int critChance) : base(name,symbol,c,strReq,dexReq,affReq,power,x,y)
    {
        CritChance = critChance;
    }

    public override int HitDamage(Creature owner, Creature target)
    {
        if(((int)(Utils.RandomNumber() * 100.0)) > CritChance)
            return base.HitDamage(owner,target);
        else
        {
            owner.Notify("Critical Hit on " + target.Name + "!");
            target.Notify(owner.Name + " rolled a Critical Hit!");
            return CritMultiplier * MaxPower;
        }
    }
}
/**
    // Stun: chance enemy cannot move for 1 second.
    Stun,
    // Venom: Enemies lose x HP a second for 3 seconds.
    Venom,
    // Speed: User speed increases by x (dex) levels.
    Speed,
    // Strength: Add x to stat..
    StrPlus, DexPlus, IntPlus,

    // Neutral Effects (1)
    None,
    // Const: Damage is always as displayed.
    Const,

    // Negative Effects (7)
    // Chance of missing x.
    Miss,
    HPMinus,
    Strain,
    Toxic,
    Slow,
    StrMinus,
    DexMinus,
    IntMinus
    */
