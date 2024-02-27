using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Game : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public Vector2 ScreenSize;
	private PackedScene PackedShip;
	private ship ShipInstance;
	private bool multiplayer = false;	

	[Export] public int MaxLargeAsteroids = 15;
	private PackedScene lAsteroid;
	
	public override void _Ready()
	{
		ScreenSize = new Vector2(1920, 1080);
		//This loads a blank copy of the ship, from here, you have to instantiate it and place it in the world
		PackedShip = GD.Load<PackedScene>("res://Scenes/ship.tscn");
		lAsteroid = GD.Load<PackedScene>("res://Scenes/LargeAsteroid.tscn");
		StartSinglePlayerGame();
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
		GiveRandomVelocity(asteroid, 40);
	}

	private void GiveRandomVelocity(RigidBody2D body, int multiplier = 1)
	{
		Random random = new Random();
		Vector2 force = new Vector2(random.Next(-1000,1000) * multiplier, random.Next(-1000,1000) * multiplier);
		
		body.ApplyForce(force);
	}

	private void StartSinglePlayerGame()
	{
		SpawnShip();
		SpawnAsteroids();
	}

	private void SpawnAsteroids()
	{
		for (int i = 0; i < MaxLargeAsteroids; i++)
		{
			SpawnLargeAsteroid();
		}
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
			ranX = rand.Next(0, (int)ScreenSize.X);
			ranY = rand.Next(0, (int)ScreenSize.Y);

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


	private void SpawnShip()
	{
		ShipInstance = (ship) PackedShip.Instantiate();
		CallDeferred("add_sibling", ShipInstance);
		SpawnRandomLocation(ShipInstance);
		ShipInstance.ShipDeath += OnShipDeath;
	}

	private void ClearAsteroids()
	{
		Array<Node> largeAsteroids = GetTree().GetNodesInGroup("large_asteroid");
		foreach (Node asteroid in largeAsteroids)
		{
			asteroid.QueueFree();
		}
		
		Array<Node> smallAsteroids = GetTree().GetNodesInGroup("small_asteroid");
		foreach (Node asteroid in smallAsteroids)
		{
			asteroid.QueueFree();
		}
	}

	private void PlayerLostDisplay(ship diedShip)
	{
		GD.Print("print");
	}

	private void OnShipDeath(ship diedShip)
	{
		diedShip.Destroy();
		PlayerLostDisplay(diedShip);
		ClearAsteroids();
		StartSinglePlayerGame();
	}
}
