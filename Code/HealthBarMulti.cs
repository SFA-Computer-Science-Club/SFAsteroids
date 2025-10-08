using Godot;
using System;

public partial class HealthBarMulti : Control
{

	[Export]
	public Ship player1;
	public Ship player2;

	public ProgressBar HealthP1;
	public ProgressBar HealthP2;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HealthP1 = GetNode<ProgressBar>("HealthBarItem");
		HealthP2 = GetNode<ProgressBar>("HealthBarItem2");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player1 != null)
		{
			HealthP1.Value = player1.Health;
		}
		if (player2 != null)
		{
			HealthP2.Value = player2.Health;
		}
	}
}
