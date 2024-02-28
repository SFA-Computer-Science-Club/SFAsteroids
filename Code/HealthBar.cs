using Godot;
using System;

public partial class HealthBar : Control
{

	[Export]
	public Ship Adornee;

	public ProgressBar Health;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Health = GetNode<ProgressBar>("HealthBarItem");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Adornee != null)
		{
			Health.Value = Adornee.Health;
		}
	}
}
