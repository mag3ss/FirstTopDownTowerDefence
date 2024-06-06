using Godot;
using System;
using System.Text.Json;

public partial class gameManager : Node
{
	[Export] private PackedScene dayNightCycle;
	private CanvasLayer gameOverMenu;
	private int currentLevel;
	public static DefenceTower defenceTowerInstance;
	private CanvasLayer timer;
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
		if (GlobalValues.playerHealth <= 0 && !hasCalledGameOver) {
			hasCalledGameOver = true;
			OnGameOver();
		}
	}
	private bool hasCalledGameOver = false;

	private void OnGameOver()
	{
		isOnMenu = true;
		GetNode<CanvasLayer>("GameOverMenu").Visible = true;
		if (GlobalValues.currentLevel == 0){
			GetNode<Button>("GameOverMenu/Panel/VBoxContainer/NextLevel").Hide();
			GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar1").Hide();
			GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar2").Hide();
			GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar3").Hide();
			SaveStruct endlessData = rwFile.Load(0);
			if (GlobalValues.score > endlessData.Score) {
				endlessData.Score = GlobalValues.score;
				GetNode<Label>("GameOverMenu/Panel/Level").Text = "New Best Score!";
			} else {
				GetNode<Label>("GameOverMenu/Panel/Level").Hide();
			}
			rwFile.Save(endlessData.Score, endlessData.LengthAlive, endlessData.KilledEnemies, endlessData.Wave, endlessData.Stars, 0);
		}
		var timer = (Progressbars)GetNode<CanvasLayer>("CanvasLayer");
		int totalSeconds = timer.hours * 3600 + timer.minutes * 60 + timer.seconds;
		GlobalValues.score = GlobalValues.killedEnemies * 10 + GlobalValues.playerCurrency;
		GD.Print("Game Over");
		GetNode<Timer>("CanvasLayer/Timer").Stop();
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/ScoreLabel").Text = "1. Score: " + GlobalValues.score;
		GetNode<Label>("GameOverMenu/Panel/TimeLabel").Text = GetNode<Label>("CanvasLayer/Time").Text;
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/WaveLabel").Text = "2. " + GetNode<Label>("CanvasLayer/WaveNum").Text;
		GetNode<Label>("GameOverMenu/Panel/VBoxContainer/KilledEnemiesLabel").Text = "3. Killed Enemies: " + GlobalValues.killedEnemies.ToString();
		GD.Print("Game Over");
		int stars = 0;
		LevelEnum currentLevelEnum = (LevelEnum)GlobalValues.currentLevel;
		if (Levels.LevelInfo.TryGetValue(currentLevelEnum , out LevelData specificLevelData))
        {
            if (specificLevelData.Score < GlobalValues.score)
			{
				stars++;
				GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar1").Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
			}
			if (specificLevelData.Wave < GlobalValues.waveNumber){
				stars++;
				GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar2").Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
			}
			if (specificLevelData.Time < totalSeconds){
				stars++;
				GetNode<Sprite2D>("GameOverMenu/Panel/EmptyStar3").Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
			}
			if (stars == 3) {
				GetNode<Label>("GameOverMenu/Panel/Level").Text = "LEVEL " + GlobalValues.currentLevel + " CLEARED";
			} else {
				GetNode<Label>("GameOverMenu/Panel/Level").Text = "LEVEL " + GlobalValues.currentLevel + " FAILED";
			}
		}
		SaveStruct savedData = rwFile.Load(GlobalValues.currentLevel);
		// Compare score and update if higher
		if (GlobalValues.score > savedData.Score){
			savedData.Score = GlobalValues.score;
		} if (stars > savedData.Stars) {
			savedData.Stars = stars;
		} if (GlobalValues.waveNumber > savedData.Wave) {
			savedData.Wave = GlobalValues.waveNumber;
		} if (GlobalValues.killedEnemies > savedData.KilledEnemies) {
			savedData.KilledEnemies = GlobalValues.killedEnemies;
		} if (totalSeconds > savedData.LengthAlive) {
			savedData.LengthAlive = totalSeconds;
		}
		rwFile.Save(savedData.Score, savedData.LengthAlive, savedData.KilledEnemies, savedData.Wave, savedData.Stars, GlobalValues.currentLevel);
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
		public static int waveNumber = 0;
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
