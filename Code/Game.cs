using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Game : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public Vector2 ScreenSize;
	private PackedScene ship;
	private ship ShipInstance;

	[Export] public int MaxLargeAsteroids = 15;
	private PackedScene lAsteroid;
	
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		//This loads a blank copy of the ship, from here, you have to instantiate it and place it in the world
		ship = GD.Load<PackedScene>("res://Scenes/ship.tscn");
		lAsteroid = GD.Load<PackedScene>("res://Scenes/LargeAsteroid.tscn");
		spawnShip();

		for (int i = 0; i < MaxLargeAsteroids; i++)
		{
			SpawnLargeAsteroid();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SpawnLargeAsteroid()
	{
		LargeAsteroid asteroid = (LargeAsteroid) lAsteroid.Instantiate();
		CallDeferred("add_child", asteroid);
		SpawnRandomLocation(asteroid);
	}
	
	private void SpawnRandomLocation(RigidBody2D node)
	{
		Vector2 spawnPosition = new Vector2();
		bool cantSpawn = true;
		Random rand = new Random();
		int ranX;
		int ranY;
		while (cantSpawn)
		{
			ranX = rand.Next(0, (int)GetViewportRect().Size.X);
			ranY = rand.Next(0, (int)GetViewportRect().Size.Y);

			spawnPosition = new Vector2(ranX, ranY);

			node.Position = spawnPosition;

			Array<Node2D> collisions = node.GetCollidingBodies();

			if (collisions.Count == 0)
			{
				cantSpawn = false;
			}
			else
			{
				GD.Print("Cant spawn, trying again");
			}
		}
		
	}


	private void spawnShip()
	{
		ShipInstance = (ship) ship.Instantiate();
		CallDeferred("add_sibling", ShipInstance);
		SpawnRandomLocation(ShipInstance);
	}

	public void OnShipDeath(ship ship)
	{
		ship.Destroy();
	}
}
