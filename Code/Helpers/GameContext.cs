using System;
using System.Collections.Generic;
using Godot;

namespace SpaceGame.Code.Helpers;

public enum GameType
{
   Multiplayer,
   Singleplayer,
}

public class GameContext
{
   public GameType GameType { get; set; } = GameType.Singleplayer;
   public List<Ship> Ships { get; set; } = new List<Ship>();
}