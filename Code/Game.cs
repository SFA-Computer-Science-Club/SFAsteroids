using Godot;
using System;

public partial class Game : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public Vector2 ScreenSize;
	private PackedScene ship;
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		ship = GD.Load<PackedScene>("res://Scenes/ship.tscn");

		var spawnedShip = ship.Instantiate();
		AddChild(spawnedShip);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
