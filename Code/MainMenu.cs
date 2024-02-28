using Godot;
using System;
using System.Reflection;

public partial class MainMenu : Node2D
{
	private PackedScene gameScene;
	private CanvasLayer gameInstance;
	private Button button;
	private Node2D title;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameScene = GD.Load<PackedScene>("res://Scenes/Game.tscn");
		button = GetNode<Button>("Button");
		title = GetNode<Sprite2D>("Title");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void onButtonPressed()
	{
		gameInstance = (CanvasLayer)gameScene.Instantiate();
		AddChild(gameInstance);
		button.Hide();
		title.Hide();
	}

}
