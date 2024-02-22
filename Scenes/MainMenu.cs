using Godot;
using System;

public partial class MainMenu : Node2D
{
	private PackedScene gameScene;
	private Node2D gameInstance;
	private Button button;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameScene = GD.Load<PackedScene>("res://Scenes/Game.tscn");
		button = GetNode<Button>("Button");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void onButtonPressed()
	{
		gameInstance = (Node2D)gameScene.Instantiate();
		AddChild(gameInstance);
		button.Hide();
	}

}
