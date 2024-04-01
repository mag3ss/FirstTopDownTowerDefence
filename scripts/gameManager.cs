using Godot;
using System;
using System.Diagnostics;

public partial class gameManager : Node
{

	public static defence_tower defenceTowerInstance;

	public static class GlobalValues
	{
		public static int playerHealth = 100;
		public static int playerCurrency = 100;

		public static bool IsOccupied;
		public static bool IsOnMenu;
	}
}
