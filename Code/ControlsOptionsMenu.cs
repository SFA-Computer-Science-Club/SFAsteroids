using Godot;
using System;
using System.Collections.Generic;

public partial class ControlsOptionsMenu : Control
{

	private PackedScene _inputButtonScene;
	private VBoxContainer _buttonContainer;
	private bool _isRemapping = false;
	private string _actionToRemap = null;
	private Button _buttonToRemap = null;
	
	/* Add any new actions into this, probably should move this to a dedicated class to make it easier when binding new items */
	private readonly Dictionary<string, string> _actionMap = new Dictionary<string, string>()
	{
		{"player_1_down", "Player 1 Move Down"},
		{"player_1_up", "Player 1 Move Up"},
		{"player_1_left", "Player 1 Move Left"},
		{"player_1_right", "Player 1 Move Right"},
		{"player_1_fire", "Player 1 Move Fire"},
	};
	
	public override void _Ready()
	{
		_inputButtonScene = GD.Load<PackedScene>("res://Scenes/UI/ControlSettingsButtonScene.tscn");
		_buttonContainer =  GetNode<VBoxContainer>(GetPath() + "/PanelContainer/MarginContainer/VBoxContainer/ScrollContainer/ActionList");
		PopulateActionList();
	}


	public override void _Process(double delta)
	{
	}

	private void ButtonPressed(Button button, string action)
	{
		if (!_isRemapping)
		{
			_isRemapping = true;
			_actionToRemap = action;
			_buttonToRemap = button;

			Label labelInput = (Label)button.FindChild("LabelInput");
			labelInput.Text = "Press key to rebind..";
			
		}
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (!_isRemapping)
		{
			return;
		}

		if (@event is InputEventMouseMotion)
		{
			return;S
		}
		
		InputMap.ActionEraseEvents(_actionToRemap);
		InputMap.ActionAddEvent(_actionToRemap, @event);
		
		Label labelInput = (Label)_buttonToRemap.FindChild("LabelInput");
		labelInput.Text = @event.AsText();
		
		_isRemapping = false;
		_actionToRemap = null;
		_buttonToRemap = null;
	}

	public void PopulateActionList()
	{
		foreach (var action in _actionMap)
		{
			Button buttonInstance = (Button)_inputButtonScene.Instantiate();
			Label actionLabel = (Label)buttonInstance.FindChild("LabelAction");
			Label inputLabel  = (Label)buttonInstance.FindChild("LabelInput");
			
			actionLabel.Text = action.Value;

			var events = InputMap.ActionGetEvents(action.Key);

			if (events?.Count > 0)
			{
				inputLabel.Text = events[0].AsText();
			}
			else
			{
				inputLabel.Text = "";
			}
			
			_buttonContainer.AddChild(buttonInstance);
			buttonInstance.Pressed += () => ButtonPressed(buttonInstance, action.Key);
		}
	}
}
