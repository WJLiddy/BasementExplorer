﻿using System.Collections.Generic;

public abstract class Entity
{
    // Symbol represting this item when it is drawn.
    public char Symbol { get; protected set; }

    // The 'X' coordinate of this entity. Manipulated by velocity.
    public int X { get; protected set; }
    // The 'Y' coordinate of this entity. Manipulated by velocity.
    public int Y { get; protected set; }

    // The Size of this entity.
    public int Size { get; private set; }

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

    // Entities have friction. High friction enties slide less far than low frition entities.
    // By default this is just .1, as a test.
    private double FrictionPercentPerFrame = .1;

    public string Name { get; private set; }

    public Entity(string name, char symbol, int x, int y, int size)
    {
        Name = name;
        Symbol = symbol;
        X = x;
        Y = y;
        Size = size;
    }

    //Moves the character based on direction. Interact based on these movements.
    public void Update(LinkedList<Entity> entities, CollisionMap collideMap)
    {
        Velocity = (int)(Velocity * (1.0 - FrictionPercentPerFrame));
        //When moving diagonally,
        //North/South is moved then the other directions.
        //This is slightly problematic because at high speeds you may glitch through objects because you go in an L-shape.
        //move the character
        //As soon an anything happens we stop doing interactions.
        //This is to prevent errors such as attacking an enemy twice. One event per update.
        if (VelocityDirection == Direction.N || VelocityDirection == Direction.NW || VelocityDirection == Direction.NE)
        {
            if (MoveNorth(entities,collideMap))
                return;
        }
        if (VelocityDirection == Direction.S || VelocityDirection == Direction.SW || VelocityDirection == Direction.SE)
        {
            if (MoveSouth(entities, collideMap))
                return;
        }
        if (VelocityDirection == Direction.W || VelocityDirection == Direction.SW || VelocityDirection == Direction.NW)
                 MoveWest(entities,collideMap);
        if (VelocityDirection == Direction.E || VelocityDirection == Direction.SE || VelocityDirection == Direction.NE)
                MoveEast(entities,collideMap);
    }

    protected abstract bool Interact(Entity e, Direction lastMoveStepDirection);

    private bool Interactions(LinkedList<Entity> allEnts, Direction lastMoveStepDirection)
    {
        foreach (Entity e in allEnts)
        {
            //do not interact with self
            if (e == this)
                continue;

            // Everything we collide with we could possibly interact with. 
            if (BasementExplorer.Collide(X, Y, Size, Size, e.X, e.Y, e.Size, e.Size))
            {
                //They interact with each other. If either value returns true, then halt any other commands.
                if (Interact(e,lastMoveStepDirection))
                    return true;
            }
        }
        return false;
    }

    private bool MoveNorth(LinkedList<Entity> e, CollisionMap c)
    {
        // Move.
        DeltaY -= Velocity;
        //Check for any interactions.
        if (Interactions(e,Direction.N))
            return true;

        //Move if we have the velocity to.
        while (DeltaY < 0)
        {
            for (int i = 0; i != Size; i++)
            {
                if (c.Collide(X + i, Y - 1))
                {
                    DeltaY = 0;
                    return false;
                }
            }
               
            DeltaY = DeltaY + DeltaScale;
            Y--;
            if (Interactions(e, Direction.N))
            {
                DeltaY = DeltaScale / 2;
                DeltaX = DeltaScale / 2;
                return true;
            }
        }
        return false;
    }

    private bool MoveWest(LinkedList<Entity> e, CollisionMap c)
    {
        // Move south.
        DeltaX -= Velocity;
        //Check for any interactions.
        if (Interactions(e, Direction.W))
            return true;

        //Move if we have the velocity to.
        while (DeltaX < 0)
        {
            for (int i = 0; i != Size; i++)
            {
                if (c.Collide(X - 1, Y + i))
                {
                    DeltaX = 0;
                    return false;
                }
            }
            DeltaX = DeltaX + DeltaScale;
            X--;
            //halt movement.
            if (Interactions(e, Direction.W))
            {
                DeltaY = DeltaScale / 2;
                DeltaX = DeltaScale / 2;
                return true;
            }
        }
        return false;
    }

    private bool MoveEast(LinkedList<Entity> e, CollisionMap c)
    {
        // Move south.
        DeltaX += Velocity;
        //Check for any interactions.
        if (Interactions(e, Direction.E))
            return true;

        //Move if we have the velocity to.
        while (DeltaX >= DeltaScale)
        {
            for (int i = 0; i != Size; i++)
            {
                if (c.Collide(X + Size, Y + i))
                 {
                    DeltaX = DeltaScale - 1;
                    return false;
                 }
            }



            DeltaX = DeltaX - DeltaScale;
            X++;
            if (Interactions(e, Direction.E))
            {
                DeltaY = DeltaScale / 2;
                DeltaX = DeltaScale / 2;
                return true;
            }
        }
        return false;
    }

    private bool MoveSouth(LinkedList<Entity> e, CollisionMap c)
    {
        // Move south.
        DeltaY += Velocity;
        //Check for any interactions.
        if (Interactions(e, Direction.S))
            return true;

        //Move if we have the velocity to.
        while (DeltaY >= DeltaScale)
        {

            for (int i = 0; i != Size; i++)
            {
                if (c.Collide(X + i, Y + Size))
                {
                    DeltaY = DeltaScale - 1;
                    return false;
                }
            }

                DeltaY = DeltaY - DeltaScale;
            Y++;
            if (Interactions(e, Direction.S))
            {
                DeltaY = DeltaScale / 2;
                DeltaX = DeltaScale / 2;
                return true;
            }
        }
        return false;
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

