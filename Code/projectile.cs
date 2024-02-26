using Godot;
using System;

public partial class projectile : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public int Damage = 10;
	[Export] public string Type = "Projectile";
	
	public override void _Ready()
	{
		VisibleOnScreenNotifier2D node = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		node.ScreenExited += removeProjectile;
	}

	private void removeProjectile()
	{
		QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += LinearVelocity * (float)delta;
	}
}
