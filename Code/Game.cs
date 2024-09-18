using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using SpaceGame.Code;
using SpaceGame.Code.Helpers;

public partial class Game : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public Vector2 ScreenSize;
	private PackedScene PackedShip;
	private Ship ShipInstance;
	private bool _multiplayer;

	private PackedScene PackedGameGUI;
	private GameGUI SinglePlayerGUI;
	//private MultiPlayerGameGUI MultiPlayerGUI;

	[Export] public int MaxLargeAsteroids = 5;
	[Export] public double TimeElapsed = 0;
	private PackedScene lAsteroid;
	
	public override void _Ready()
	{
		ScreenSize = DisplayServer.WindowGetSize();
		//This loads a blank copy of the ship, from here, you have to instantiate it and place it in the world
		PackedShip = GD.Load<PackedScene>("res://Scenes/Ship.tscn");
		PackedGameGUI = GD.Load<PackedScene>("res://Scenes/UI/GameGUI.tscn");
		lAsteroid = GD.Load<PackedScene>("res://Scenes/LargeAsteroid.tscn");

		if (_multiplayer)
		{
			StartMultiPlayerGame();
		}
		else
		{
			StartSinglePlayerGame();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		TimeElapsed += delta;
	}

	public void SetMultiplayer(bool value)
	{
		_multiplayer = value;
	}

	private void SpawnLargeAsteroid()
	{
		LargeAsteroid asteroid = (LargeAsteroid) lAsteroid.Instantiate();
		asteroid.SetRefScreenSize(ScreenSize);
		CallDeferred("add_child", asteroid);
		SpawnRandomLocationOutsideScreen(asteroid);
		Vector2 randomPoint = PickRandomLocationInGame();
		GiveVelocity(asteroid, randomPoint, 60);
		GiveRandomAngularVelocity(asteroid, 50);

		asteroid.Destroyed += OnLargeAsteroidDestroyed;
	}

	private void OnLargeAsteroidDestroyed(LargeAsteroid asteroid, Ship destructor = null)
	{
		AwardPointsToPlayer(destructor.ShipPlayer, asteroid.Points);
		SpawnLargeAsteroid();
	}

	private void AwardPointsToPlayer(Player player, int amount)
	{
		player.AddPoints(amount);
	}

	private void GiveVelocity(RigidBody2D body, Vector2 direction, int multiplier = 1)
	{
		Vector2 dirToMove = (body.Position - direction).Normalized();
		Random random = new Random();
		
		body.ApplyForce(-dirToMove * (random.Next(300,1000) * multiplier));
	}

	private void GiveRandomVelocity(RigidBody2D body, int multiplier = 1)
	{
		Random random = new Random();
		Vector2 force = new Vector2(random.Next(-1000,1000) * multiplier, random.Next(-1000,1000) * multiplier);
		
		body.ApplyForce(force);
	}

	private void GiveRandomAngularVelocity(RigidBody2D body, int multiplier = 1)
	{
		Random random = new Random();
		
		
		body.ApplyTorque(random.Next(-1000,1000) * multiplier);
	}

	private void StartMultiPlayerGame()
	{
		Player player = new Player();
		Player player2 = new Player();
		
		player.PlayerName = "Player 1";
		player2.PlayerName = "Player 2";
		Ship player1Ship = SpawnShip(player);
		Ship player2Ship = SpawnShip(player2);
		GameContext gameContext = new GameContext()
		{
			GameType = GameType.Multiplayer,
			Ships = new List<Ship>(){player1Ship, player2Ship}	
		};
		SpawnAsteroids();

		player1Ship.GameContext = gameContext;
		player2Ship.GameContext = gameContext;
		player1Ship.ShowName = true;
		player2Ship.ShowName = true;	
		
		SinglePlayerGUI = PackedGameGUI.Instantiate<GameGUI>();
		CallDeferred("add_child", SinglePlayerGUI);
		SinglePlayerGUI.ForceReady();
		SinglePlayerGUI.SetShip(player1Ship);
	}

	private void StartSinglePlayerGame()
	{
		
		Player player = new Player();
		Ship playerShip = SpawnShip(player);
		SpawnAsteroids();
		
		SinglePlayerGUI = PackedGameGUI.Instantiate<GameGUI>();
		CallDeferred("add_child", SinglePlayerGUI);
		SinglePlayerGUI.ForceReady();
		SinglePlayerGUI.SetShip(playerShip);
	}

	private void SpawnAsteroids()
	{
		var random = new Random();
		for (int i = 0; i < MaxLargeAsteroids; i++)
		{
			SpawnLargeAsteroid();
		}
	}

	private void SpawnRandomLocationOutsideScreen(RigidBody2D node)
	{
		Vector2 spawnPosition = new Vector2();
		bool cantSpawn = true;
		Random rand = new Random();
		int ranX;
		int ranY;
		while (cantSpawn)
		{
			if (rand.Next(0, 2) == 0)
			{
				ranX = rand.Next(-100, 0);
			}
			else
			{
				ranX = rand.Next((int)ScreenSize.X, (int)ScreenSize.X + 100);
			}

			if (rand.Next(0, 2) == 0)
			{
				ranY = rand.Next(-100, 0);
			}
			else
			{
				ranY = rand.Next((int)ScreenSize.Y, (int)ScreenSize.Y + 100);
			}

			spawnPosition = new Vector2(ranX, ranY);

			node.Position = spawnPosition;

			Array<Node2D> collisions = node.GetCollidingBodies();

			if (collisions.Count == 0)
			{
				cantSpawn = false;
				if (node.IsInGroup("large_asteroid"))
				{
					LargeAsteroid ast = (LargeAsteroid)node;
					ast.SetPhysicsPosition(spawnPosition);
				}
			}
			else
			{
			}
		}
	}

	private Vector2 PickRandomLocationInGame()
	{
		var rand = new Random();
		return new Vector2(rand.Next(0, (int)ScreenSize.X), rand.Next(0,(int)ScreenSize.Y));
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


	private Ship SpawnShip(Player player = null)
	{
		ShipInstance = (Ship) PackedShip.Instantiate();
		ShipInstance.ShipPlayer = player;
		CallDeferred("add_child", ShipInstance);
		SpawnRandomLocation(ShipInstance);
		ShipInstance.ShipDeath += OnShipDeath;

		return ShipInstance;
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

	private void PlayerLostDisplay(Ship diedShip)
	{
	}

	private void OnShipDeath(Ship diedShip)
	{
		diedShip.Destroy();
		PlayerLostDisplay(diedShip);
		ClearAsteroids();
		TimeElapsed = 0;
		StartSinglePlayerGame();
	}
}
