using Godot;
using System;

public partial class HealthBar : Node2D
{

	[Export]
	public ship Adornee;

	public ProgressBar Health;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Health = GetNode<ProgressBar>("HealthBarItem");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Adornee == null)
		{
			Hide();
		}
		else
		{
			Health.Value = Adornee.Health;
		}
	}
}
