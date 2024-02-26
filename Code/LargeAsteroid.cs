using Godot;
using System;

public partial class LargeAsteroid : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public int Health = 30;
	[Export] public string Type = "LargeAsteroid";
	private ProgressBar _healthBar;
	private Vector2 _screenSize;
	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthBar.Hide();
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
		
		_healthBar.Show();
		
		Health -= proj.Damage;
		_healthBar.Value = Health;

		node.QueueFree();
		
		if (Health <= 0)
		{
			Destroyed();
		}
	}
}
