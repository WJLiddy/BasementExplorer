using Microsoft.Xna.Framework;

class HPPlus : PrimaryWeapon
{
    int HPGain;

    public HPPlus(string name, char symbol, Color c, int strReq, int dexReq, int affReq, int power, int x, int y, int HPgain) : base(name, symbol, c, strReq, dexReq, affReq, power, x, y)
    {
        HPGain = HPgain;
    }

    public override int PowerRoll(Creature owner, Creature target)
    {
        int damage = base.PowerRoll(owner, target);
        owner.Heal(damage);
        return damage;
    }
}