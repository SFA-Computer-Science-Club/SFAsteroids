using Godot;
using System;

public partial class HealthBar : Control
{

	[Export]
	public Ship player1;

	public ProgressBar Health;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Health = GetNode<ProgressBar>("HealthBarItem");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player1 != null)
		{
			Health.Value = player1.Health;
		}
	}
}
