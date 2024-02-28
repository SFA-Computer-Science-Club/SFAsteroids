using Godot;
using System;

public partial class SmallAsteroid : RigidBody2D
{
    // Called when the node enters the scene tree for the first time.
    [Export] public int Health = 5;
    [Export] public int Points = 2;
    private Vector2 _screenSize;
    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        GetNode<VisibleOnScreenNotifier2D>("OnScreen").ScreenExited += OutOfBounds;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void Destroyed()
    {
        QueueFree();
    }
	
    private void OutOfBounds()
    {
        Vector2 asteroidPosition = Position;
        if(asteroidPosition.Y < _screenSize.Y)
        {
            asteroidPosition.Y += _screenSize.Y;
        }

        if(asteroidPosition.Y > _screenSize.Y)
        {
            asteroidPosition.Y -= _screenSize.Y;
        }

        if(asteroidPosition.X < _screenSize.X)
        {
            asteroidPosition.X += _screenSize.X;
        }

        if(asteroidPosition.X > _screenSize.X)
        {
            asteroidPosition.X -= _screenSize.X;
        }
		
        PhysicsServer2D.BodySetState(GetRid(), PhysicsServer2D.BodyState.Transform, Transform2D.Identity.Translated(asteroidPosition));
    }

    private void Collision(Node2D node)
    {
        if (!node.IsInGroup("projectile"))
        {
            return;
        }

        projectile proj = (projectile)node;		
		
        QueueFree();
    }
}