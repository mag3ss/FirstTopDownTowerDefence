using Godot;
using System;
using System.Text.Json;

public partial class gameManager : Node
{
	[Export] private PackedScene dayNightCycle;
	private CanvasLayer gameOverMenu;
	private int currentLevel;
	public static DefenceTower defenceTowerInstance;
	PackedScene MainMenu = ResourceLoader.Load<PackedScene>("res://scenes/MainMenu.tscn");
	PackedScene levelPicker = ResourceLoader.Load<PackedScene>("res://scenes/LevelPicker.tscn");
	private CanvasLayer SettingsMenu;
	private bool isOnMenu;
	RWFile rwFile = new RWFile();
	public override void _Ready()
	{
		SettingsMenu = GetNode<CanvasLayer>("Settings");
		
	}

    public override void _PhysicsProcess(double delta)
    {
        if (GlobalValues.playerHealth <= 0 && !isOnMenu){
			OnGameOver();
		}
    }

	private void OnGameOver()
	{
		if (GlobalValues.currentLevel == 0){
			GetNode<Button>("GameOverMenu/Panel/NextLevel").Hide();
			GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar1").Hide();
			GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar2").Hide();
			GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar3").Hide();
			SaveStruct endlessData = rwFile.Load(0);
			if (GlobalValues.score > endlessData.Score) {
				endlessData.Score = GlobalValues.score;
				JsonSerializer.Serialize(endlessData);
				GetNode<Label>("GameOverMenu/Panel/Level").Text = "New Best Score!";
			} else {
				GetNode<Label>("GameOverMenu/Panel/Level").Hide();
			}
		} else {
			GetNode<Label>("GameOverMenu/Panel/Level").Text = "LEVEL " + GlobalValues.currentLevel + " CLEARED";
		}
		GlobalValues.score = GlobalValues.killedEnemies * 10 + GlobalValues.playerCurrency;
		isOnMenu = true;
		GD.Print("Game Over");
		GetNode<Timer>("CanvasLayer/Timer").Stop();
		GetNode<CanvasLayer>("GameOverMenu").Visible = true;
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/ScoreLabel").Text = "1. Score: " + GlobalValues.score;
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/TimeLabel").Text = "Alive " + GetNode<Label>("CanvasLayer/Time").Text;
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/WaveLabel").Text = "2. Waves Survived: " + GlobalValues.waveNumber;
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/KilledEnemiesLabel").Text = "3. Killed Enemies: " + GlobalValues.killedEnemies;
		GD.Print("Game Over");
		int stars = 0;
		LevelEnum currentLevelEnum = (LevelEnum)GlobalValues.currentLevel;
		if (Levels.LevelInfo.TryGetValue(currentLevelEnum , out LevelData specificLevelData))
        {
            if (specificLevelData.Score > GlobalValues.score)
			{
				stars++;
				GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar1").Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
			}
			if (specificLevelData.Time > 10){
				stars++;
				GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar2").Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
			}
			if (specificLevelData.Wave > GlobalValues.waveNumber){
				stars++;
				GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar3").Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
			}
		}
		SaveStruct savedData = rwFile.Load(GlobalValues.currentLevel);
		if (GlobalValues.score > savedData.Score){
			GlobalValues.score = savedData.Score;
			stars = savedData.Stars;
			JsonSerializer.Serialize(savedData);
		}
	}

	private void NextLevel() {
		if (GlobalValues.currentLevel == 10){
			GetTree().ChangeSceneToPacked(MainMenu);
		} else {
			GlobalValues.currentLevel += 1;
			GetTree().ReloadCurrentScene();
		}
	}

	private void PlayAgain(){
		GetTree().ReloadCurrentScene();
		GlobalValues.playerHealth = 100;
		GlobalValues.playerCurrency = 100;
		GlobalValues.score = 0;
		GlobalValues.killedEnemies = 0;
		GlobalValues.waveNumber = 0;
	}


    public static class GlobalValues
	{
		public static int currentLevel;
		public static float playerHealth = 100;
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
