using System.Collections.Generic;

public abstract class Entity
{
    // Symbol represting this item when it is drawn.
    public char Symbol { get; protected set; }

    // The 'X' coordinate of this entity. Manipulated by velocity.
    public int X { get; protected set; }
    // The 'Y' coordinate of this entity. Manipulated by velocity.
    public int Y { get; protected set; }

    // The Size of this entity.
    public int Size = 0;

    // Deltas to the next X or Y value. Think of this as "fractional X" and "fractional Y"
    protected int DeltaX = 0;
    protected int DeltaY = 0;
    public static readonly int DeltaScale = 1000;

    // Entities can have velocities, in millipx/frame.
    // This is mostly for knockbacks and throwing items. 
    protected int Velocity = 0;

    //Directions we can be traveling in.
    public enum Direction { N, NE, E, SE, S, SW, W, NW };

    protected Direction VelocityDirection;

    public Entity(char symbol, int x, int y, int size)
    {
        Symbol = symbol;
        X = x;
        Y = y;
        Size = size;
    }

    //Moves the character based on direction. Interact based on these movements.
    public void Move(LinkedList<Entity> entities, bool[,] collideMap)
    {
        //move the character.
        switch (VelocityDirection)
        {
            case Direction.N:
                MoveNorth(entities);
                break;
            case Direction.S:
                MoveSouth(entities);
                break;
            case Direction.E:
                MoveEast(entities);
                break;
            case Direction.W:
                MoveWest(entities);
                break;
        }
    }

    protected abstract bool Interact(Entity e);

    private bool Interactions(LinkedList<Entity> allEnts)
    {
        foreach (Entity e in allEnts)
        {
            // Everything we collide with we could possibly interact with. 
            if (BasementExplorer.Collide(X, Y, Size, Size, e.X, e.Y, e.Size, e.Size))
            {
                //They interact with each other. If either value returns true, then halt any other commands.
                if (Interact(e))
                    return true;
            }
        }
        return false;
    }

    private void MoveNorth(LinkedList<Entity> e)
    {
        // Move south.
        DeltaY -= Velocity;
        //Check for any interactions.
        if (Interactions(e))
            return;

        //Move if we have the velocity to.
        while (DeltaY < 0)
        {
            DeltaY = DeltaY + DeltaScale;
            Y--;
            if (Interactions(e))
            {
                DeltaY = DeltaScale / 2;
                DeltaX = DeltaScale / 2;
                return;
            }
        }
    }

    private void MoveWest(LinkedList<Entity> e)
    {
        // Move south.
        DeltaX -= Velocity;
        //Check for any interactions.
        if (Interactions(e))
            return;

        //Move if we have the velocity to.
        while (DeltaX < 0)
        {
            DeltaX = DeltaX + DeltaScale;
            X--;
            if (Interactions(e))
            {
                DeltaY = DeltaScale / 2;
                DeltaX = DeltaScale / 2;
                return;
            }
        }
    }

    private void MoveEast(LinkedList<Entity> e)
    {
        // Move south.
        DeltaX += Velocity;
        //Check for any interactions.
        if (Interactions(e))
            return;

        //Move if we have the velocity to.
        while (DeltaX >= DeltaScale)
        {
            DeltaX = DeltaX - DeltaScale;
            X++;
            if (Interactions(e))
            {
                DeltaY = DeltaScale / 2;
                DeltaX = DeltaScale / 2;
                return;
            }
        }
    }

    private void MoveSouth(LinkedList<Entity> e)
    {
        // Move south.
        DeltaY += Velocity;
        //Check for any interactions.
        if (Interactions(e))
            return;

        //Move if we have the velocity to.
        while (DeltaY >= DeltaScale)
        {
            DeltaY = DeltaY - DeltaScale;
            Y++;
            if (Interactions(e))
            {
                DeltaY = DeltaScale / 2;
                DeltaX = DeltaScale / 2;
                return;
            }
        }
    }

    public abstract void Draw(PixelFont f, AD2SpriteBatch sb);

    public static Direction Opposite(Direction d)
    {
        switch (d)
        {
            case Direction.E: return Direction.W;
            case Direction.W: return Direction.E;

            case Direction.N: return Direction.S;
            case Direction.S: return Direction.N;

            case Direction.NE: return Direction.SW;
            case Direction.SW: return Direction.NE;

            case Direction.NW: return Direction.SE;
            case Direction.SE: return Direction.NW;
        }
        //TODO: Assert this never happens
        return Direction.N;
    }
}

