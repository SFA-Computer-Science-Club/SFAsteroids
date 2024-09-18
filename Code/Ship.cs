using Godot;
using System;
using SpaceGame.Code;
using SpaceGame.Code.Helpers;

public partial class Ship : RigidBody2D
{

	//When [Export] is used above a variable, it will show up in the game editor, with a default value of 2

	[Export] 
	public int EnginePower { get; set; } = 5;

	[Export] public bool ShowName { get; set; } = false;
	
	[Export] public double FireDelay { get; set; } = 0.5;
	[Signal]
	public delegate void HitEventHandler();

	[Signal]
	public delegate void ShipDeathEventHandler(Ship diedShip);

	public Player ShipPlayer;
	
	[Export]
	public double Health { get; set; } = 100;
	
	public GameContext GameContext { get; set; }

	public Vector2 ScreenSize;
	private RigidBody2D physics;
	private CollisionShape2D collider;
	private PackedScene projectile;
	private Timer timer = new Timer();
	private AudioStreamPlayer2D player;
	private Timer GodMode;
	private Sprite2D _nameSprite;
	private Label _nameLabel;
	
	private void OnBodyEntered(Node2D body)
	{
		if (Health <= 0)
		{
			return;
		}
		if (!GodMode.IsStopped())
		{
			return;
		}
		EmitSignal(SignalName.Hit);
		
		if (body.IsInGroup("large_asteroid")) {
			Health -= 25;
			GodMode.Start();
			GetNode<AudioStreamPlayer2D>("DamageSound").Play();
		}
		else if (body.IsInGroup("small_asteroid"))
		{
			Health -= 12.5;
			GodMode.Start();
			GetNode<AudioStreamPlayer2D>("DamageSound").Play();
		}

		if (Health <= 25)
		{
			GetNode<AudioStreamPlayer2D>("LowHealthSound").Play();
		}

		if (Health <= 0)
		{
			EmitSignal(SignalName.ShipDeath, this);
		}
	}

	public void Destroy()
	{
		QueueFree();
	}

	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		projectile = GD.Load<PackedScene>("res://Scenes/projectile.tscn");
		AddChild(timer);
		timer.WaitTime = 0.25f;
		timer.OneShot = true;
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
		player = GetNode<AudioStreamPlayer2D>("AudioPlayer");
		GodMode = GetNode<Timer>("GodMode");
		_nameSprite = GetNode<Sprite2D>("NameSprite");
		_nameLabel = GetNode<Label>("NameSprite/NameLabel");
		_nameLabel.Text = ShipPlayer.PlayerName;
		GodMode.OneShot = true;
		GodMode.WaitTime = 3;

		VisibleOnScreenNotifier2D node = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		node.ScreenExited += shipOutOfBounds;
	}

	private void shipOutOfBounds()
	{
		Vector2 ShipPosition = Position;
		if(ShipPosition.Y < ScreenSize.Y)
		{
			ShipPosition.Y += ScreenSize.Y;
		}

		if(ShipPosition.Y > ScreenSize.Y)
		{
			ShipPosition.Y -= ScreenSize.Y;
		}

		if(ShipPosition.X < ScreenSize.X)
		{
			ShipPosition.X += ScreenSize.X;
		}

		if(ShipPosition.X > ScreenSize.X)
		{
			ShipPosition.X -= ScreenSize.X;
		}
		
	
		SetDeferred(RigidBody2D.PropertyName.Position, ShipPosition);
	}

	private void HandleNameRotation()
	{
		if (!ShowName || _nameSprite == null)
		{
			return;
		}
		
		_nameSprite.SetGlobalRotation(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = LinearVelocity; // The player's movement vector.
		var forwardVector = new Vector2(Mathf.Cos(GlobalRotation + Mathf.DegToRad(-90)), Mathf.Sin(GlobalRotation + Mathf.DegToRad(-90))).Normalized();
		
		HandleNameRotation();
		
		int rotationDir = 0;
		float rotationSpeed = 3.5f;
		
		if (!velocity.IsZeroApprox())
		{
			Vector2 scaled = velocity * -0.002f;
			ApplyForce(scaled);
		}

		//Movement code
		bool anyPressed = false;

		if (Input.IsActionPressed("player_1_right"))
		{
			rotationDir+=1;
			anyPressed = true;
		}

		if (Input.IsActionPressed("player_1_left"))
		{
			rotationDir-=1;
			anyPressed = true;
		}

		if (Input.IsActionPressed("player_1_down"))
		{
			
			ApplyForce(-EnginePower * forwardVector);
			anyPressed = true;
		}

		if (Input.IsActionPressed("player_1_up"))
		{

			ApplyForce(EnginePower * forwardVector);
			anyPressed = true;
		}

		//Handles firing
		if (Input.IsActionPressed("player_1_fire"))
		{
			if (timer.IsStopped())
			{
				timer.Start();
				var projectileInstance = projectile.Instantiate();
				AddSibling(projectileInstance);

				var proj = (RigidBody2D)projectileInstance;
				proj.Position = Position;
				proj.Rotation = Rotation;
				
				proj.ApplyForce(forwardVector * 400);

				global::projectile actualProj = (projectile)projectileInstance;
				actualProj.FiredFrom = this;
				
				player.Play();
			}
		}

		//Restricts max velocity
		velocity.X = Mathf.Clamp(velocity.X, -350, 350);
		velocity.Y = Mathf.Clamp(velocity.Y, -350, 350);

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		//Handles the engine animation
		if (anyPressed)
		{
			animatedSprite2D.Frame = 1;
		}
		else
		{
			animatedSprite2D.Frame = 0;
		}

		//This code actually updates the position by the velocity
		Rotation += rotationDir * rotationSpeed * (float)delta;
		Position += velocity * (float)delta;

	}
}
