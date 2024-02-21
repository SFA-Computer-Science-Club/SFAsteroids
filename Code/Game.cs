using Godot;
using System;

public partial class Game : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public Vector2 ScreenSize;
	private PackedScene ship;
	
	//TODO 1) Add GUI that will describe ship health, total points, and starting, and ending matches
	
	//TODO 2) Make the ship spawn in the center, and asteroids to spawn around it
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		//This loads a blank copy of the ship, from here, you have to instantiate it and place it in the world
		ship = GD.Load<PackedScene>("res://Scenes/ship.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//Fired when the ship detects a collision
	//TODO 3) Add game mechanics when the ship collides
	public void OnShipHit()
	{
		
	}
}
