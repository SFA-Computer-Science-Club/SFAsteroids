using Godot;
using System;

public partial class LargeAsteroid : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public int Health = 30;
	[Export] public string Type = "LargeAsteroid";
	private ProgressBar HealthBar;
	public override void _Ready()
	{
		HealthBar = GetNode<ProgressBar>("HealthBar");
		HealthBar.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void Destroyed()
	{
		QueueFree();
	}

	private void Collision(Node2D node)
	{
		projectile proj = (projectile) node;
		if (proj == null)
		{
			return;
		}

		HealthBar.Show();
		
		Health -= proj.Damage;
		HealthBar.Value = Health;

		node.QueueFree();
		
		if (Health <= 0)
		{
			Destroyed();
		}
	}
}
