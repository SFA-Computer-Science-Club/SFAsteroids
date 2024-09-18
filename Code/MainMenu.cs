using Godot;
using System;
using System.Reflection;

public partial class MainMenu : Node2D
{
	private PackedScene _gameScene;
	private Game _game;
	private CanvasLayer _gameInstance;
	private Button _singlePlayerButton;
	private Button _multiPlayerButton;
	private Button _controlButton;
	private Node2D _title;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameScene = GD.Load<PackedScene>("res://Scenes/Game.tscn");
		_singlePlayerButton = GetNode<Button>("SinglePlayerButton");
		_multiPlayerButton = GetNode<Button>("MultiPlayerButton");
		_controlButton = GetNode<Button>("ControlsButton");
		_title = GetNode<Sprite2D>("Title");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void HideUI()
	{
		_singlePlayerButton.Hide();
		_multiPlayerButton.Hide();
		_controlButton.Hide();
		_title.Hide();
	}

	private void SetupGame(bool isMultiPlayer)
	{
		_gameInstance = (CanvasLayer)_gameScene.Instantiate();
		_game = (Game)_gameInstance;
		_game.SetMultiplayer(isMultiPlayer);
		AddChild(_gameInstance);
		HideUI();
	}

	private void MultiplayerButton_Pressed()
	{
		SetupGame(true);
	}

	private void onButtonPressed()
	{
		SetupGame(false);
	}
	
	private void ControlButton_Pressed()
	{
		ControlsOptionsMenu controls = (ControlsOptionsMenu)FindChild("ControlsOptionsMenu");

		if (controls.Visible)
		{
			controls.Hide();
		}
		else
		{
			controls.Show();
		}
	}

}
