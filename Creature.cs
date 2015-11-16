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

    public void KnockBack(int damage, int velocity, Direction velocityDirection)
    {
        // Knock Back is % of damage.
        double knockBackRatio = ((double)damage) / ((double)(Str * HPPerStr));

        VelocityDirection = velocityDirection;
        Velocity = velocity;
    }
   
}
