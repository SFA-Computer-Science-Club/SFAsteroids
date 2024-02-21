using Godot;
using System;

public partial class ship : RigidBody2D 
{

	//When [Export] is used above a variable, it will show up in the game editor, with a default value of 2
	[Export] 
	public int EnginePower { get; set; } = 2;

	[Export] public double FireDelay { get; set; } = 0.5;

	[Signal]
	public delegate void HitEventHandler();

	public Vector2 ScreenSize;
	private RigidBody2D physics;
	private CollisionShape2D collider;
	private PackedScene projectile;
	private Timer timer = new Timer();
	private AudioStreamPlayer2D player;


	//TODO Ensure that the colliding body is an Asteroid, otherwise, ignore it
	private void OnBodyEntered(Node2D body)
	{
		EmitSignal(SignalName.Hit);
		collider.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
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
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = LinearVelocity; // The player's movement vector.
		var mousePos = GetViewport().GetMousePosition(); //2D mouse Position
		var angleTo = Position.AngleToPoint(mousePos); //Find the angle needed to reach the players mouse position from current ship position
		var forwardVector = new Vector2(Mathf.Cos(GlobalRotation + Mathf.DegToRad(-90)), Mathf.Sin(GlobalRotation + Mathf.DegToRad(-90))).Normalized();
		var rightVector = new Vector2(Mathf.Cos(GlobalRotation), Mathf.Sin(GlobalRotation)).Normalized();
		Rotation = angleTo + Mathf.DegToRad(90f);
		if (!velocity.IsZeroApprox())
		{
			Vector2 scaled = velocity*-0.002f;
			ApplyForce(scaled);
		}
		
		//Movement code
		bool anyPressed = false;
		
		if (Input.IsActionPressed("move_right"))
		{
			//velocity.X += 1;
			ApplyForce(rightVector * EnginePower/2);
			anyPressed = true;
		}

		if (Input.IsActionPressed("move_left"))
		{
			ApplyForce(rightVector * -EnginePower/2);
			anyPressed = true;
		}

		if (Input.IsActionPressed("move_down"))
		{

			ApplyForce(-EnginePower * forwardVector);
			anyPressed = true;
		}

		if (Input.IsActionPressed("move_up"))
		{
			
			ApplyForce(EnginePower * forwardVector);
			anyPressed = true;
		}

		//Handles firing
		if (Input.IsActionPressed("fire"))
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
				player.Play();
			}
		}

		//Restricts max velocity
		velocity.X = Mathf.Clamp(velocity.X, -250, 250);
		velocity.Y = Mathf.Clamp(velocity.Y, -250, 250);

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
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}
}