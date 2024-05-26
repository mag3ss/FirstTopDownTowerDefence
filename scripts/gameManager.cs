using Godot;
using System;

public partial class gameManager : Node
{
	[Export] private PackedScene dayNightCycle;
	private CanvasLayer gameOverMenu;
	private int currentLevel;
	public static defence_tower defenceTowerInstance;
	PackedScene MainMenu = ResourceLoader.Load<PackedScene>("res://scenes/main_menu.tscn");
	PackedScene levelPicker = ResourceLoader.Load<PackedScene>("res://scenes/level_picker.tscn");
	private CanvasLayer SettingsMenu;
	private bool isOnMenu;
	public override void _Ready()
	{
		SettingsMenu = GetNode<CanvasLayer>("Settings");
	}

    public override void _PhysicsProcess(double delta)
    {
        if (gameManager.GlobalValues.playerHealth <= 0 && !isOnMenu){
			OnGameOver();
		}
    }

	private void OnGameOver()
	{
		GlobalValues.score = GlobalValues.killedEnemies * 10 + GlobalValues.playerCurrency;
		isOnMenu = true;
		GD.Print("Game Over");
		GetNode<Timer>("CanvasLayer/Timer").Stop();
		GetNode<CanvasLayer>("GameOverMenu").Visible = true;
		GetNode<Label>("GameOverMenu/Panel/Level").Text = "LEVEL " + GlobalValues.currentLevel + " CLEARED";
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/ScoreLabel").Text = "Score: " + GlobalValues.score;
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/TimeLabel").Text = "Alive " + GetNode<Label>("CanvasLayer/Time").Text;
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/WaveLabel").Text = "Waves Survived: " + GlobalValues.waveNumber;
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/KilledEnemiesLabel").Text = "Killed Enemies: " + GlobalValues.killedEnemies;
		GD.Print("Game Over");
		LevelEnum currentLevelEnum = (LevelEnum)GlobalValues.currentLevel;
		if (Levels.LevelInfo.TryGetValue(currentLevelEnum , out LevelData specificLevelData))
        {
			GD.Print("sdadasd");
            if (specificLevelData.Score > GlobalValues.score)
			{
				GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar1").Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
			}
			if (specificLevelData.Time > 10){
				GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar2").Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
			}
			if (specificLevelData.Wave > GlobalValues.waveNumber){
				GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar3").Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
			}
        }

	}

    public static class GlobalValues
	{
		public static int currentLevel;
		public static int playerHealth = 100;
		public static int playerCurrency = 100;

		public static bool IsOccupied;
		public static bool IsOnMenu;
		public static int gameSpeed = 1;
		public static int aliveEnemies = 0;
		public static int score = 0;
		public static int killedEnemies	= 0;
		public static int waveNumber;
		public static string Mode;
	}

    public override void _ShortcutInput(InputEvent @event) {
        if (@event.IsActionPressed("cancel")){
			ToggleSettingsMenu();
		}
    }

	private void ToggleSettingsMenu(){
		SettingsMenu.Visible = !SettingsMenu.Visible;
		if (!SettingsMenu.Visible) {
			GetNode<Button>("Settings/Control/TabContainer/Node/Button").Visible = false;
		} else if (GetNode<TabContainer>("Settings/Control/TabContainer").Visible){
			GetNode<Button>("Settings/Control/TabContainer/Node/Button").Visible = true;
		}
	}

	private void QuitGame(){
		GetTree().Quit();
	}

	private void QuitToMainMenu(){
		GetTree().ChangeSceneToPacked(MainMenu);
	}

	private void OptionsPressed() {

	}

	private void VideoResolutionSelected(int index) {
		switch (index) {
			case 0:
				GetViewport().GetWindow().Size = new Vector2I(1366, 768);
				break;
			case 1:
				GetViewport().GetWindow().Size = new Vector2I(1440, 900);
				break;
			case 2:
				GetViewport().GetWindow().Size = new Vector2I(1920, 1080);
				break;
			case 3:
				GetViewport().GetWindow().Size = new Vector2I(2560, 1440);
				break;
			case 4:
				GetViewport().GetWindow().Size = new Vector2I(3840, 2160);
				break;
		}
		GetViewport().GetWindow().Mode = Window.ModeEnum.ExclusiveFullscreen;
	}

	private void ScreenModeSelected(int index) {
		switch (index) {
			case 0:
				GetViewport().GetWindow().Mode = Window.ModeEnum.Fullscreen;
				break;
			case 1:
				GetViewport().GetWindow().Mode = Window.ModeEnum.Windowed;
				break;
			case 2:
				GetViewport().GetWindow().Mode = Window.ModeEnum.ExclusiveFullscreen;
				break;
			case 3:
				GetViewport().GetWindow().Mode = Window.ModeEnum.Maximized;
				break;
		}
	}

	private void ShowParticles(bool show) {
		if (show) {
			GetNode<CpuParticles2D>("TextureRect/CPUParticles2D").Emitting = true;
		} else {
			GetNode<CpuParticles2D>("TextureRect/CPUParticles2D").Emitting = false;
		}
	}

	private void DayNightCycle(bool show) {
		if (show) {
			if (GetNodeOrNull("DayNightCycle") == null) {
				AddChild(dayNightCycle.Instantiate());
			}
		} else {
			GetNodeOrNull("DayNightCycle")?.QueueFree();
		}
	}

	private void SettingsBackBtnPressed() {
		GetNode<Panel>("Settings/Control/Panel").Visible = !GetNode<Panel>("Settings/Control/Panel").Visible;
		GetNode<Button>("Settings/Control/TabContainer/Node/Button").Visible = !GetNode<Button>("Settings/Control/TabContainer/Node/Button").Visible;
		GetNode<TabContainer>("Settings/Control/TabContainer").Visible = !GetNode<TabContainer>("Settings/Control/TabContainer").Visible;

	}
}
