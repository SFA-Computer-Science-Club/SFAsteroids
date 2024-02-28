using Godot;
using System;

public partial class LargeAsteroid : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public int Health = 30;
	[Export] public int Points = 5;

	[Signal]
	public delegate void DestroyedEventHandler(LargeAsteroid asteroid, Ship destructor);
	private ProgressBar _healthBar;
	private Vector2 _screenSize;
	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthBar.Hide();
		GetNode<VisibleOnScreenNotifier2D>("OnScreen").ScreenExited += OutOfBounds;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnDestruction(Ship destructor)
	{
		EmitSignal(SignalName.Destroyed, this, destructor);
		QueueFree();
	}

	public void SetRefScreenSize(Vector2 size)
	{
		_screenSize = size;
	}

	public void ForceOutOfBoundsCheck()
	{
		OutOfBounds();
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

	public void SetPhysicsPosition(Vector2 pos)
	{
		PhysicsServer2D.BodySetState(GetRid(), PhysicsServer2D.BodyState.Transform, Transform2D.Identity.Translated(pos));
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
			OnDestruction(proj.FiredFrom);
		}
	}
}
