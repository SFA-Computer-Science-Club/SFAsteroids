using Godot;
using System;
using SpaceGame.Code;

public partial class GameGUI : Control
{
	// Called when the node enters the scene tree for the first time.
	private Ship ship;
	private HealthBar ShipHealthBar;
	private Label TimeLabel;
	private Label PointsLabel;
	public override void _Ready()
	{
		TimeLabel = GetNode<Label>("GamePanel/Time");
		PointsLabel = GetNode<Label>("GamePanel/Points");
		ShipHealthBar = (HealthBar) GetNode<Panel>("GamePanel").GetNode<Control>("HealthBar");
	}

	public void ForceReady()
	{
		ShipHealthBar = (HealthBar) GetNode<Panel>("GamePanel").GetNode<Control>("HealthBar");
	}

	public void SetShip(Ship s)
	{
		ship = s;
		ShipHealthBar.Adornee = s;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Node parent = GetParent();

		GetNode<Label>("GamePanel/FPS").Text = "FPS: " + Engine.GetFramesPerSecond();
		if (parent is Game)
		{
			Game game = (Game)parent;

			TimeLabel.Text = Math.Round((game.TimeElapsed), 0) + " seconds";
		}

		if (ship != null)
		{
			PointsLabel.Text = ship.ShipPlayer.points + " points";
		}
	}
}
