using Godot;

public partial class MainMenu : Control
{
	PackedScene levelsMode = ResourceLoader.Load<PackedScene>("res://scenes/LevelPicker.tscn");
	PackedScene endlessMode = ResourceLoader.Load<PackedScene>("res://scenes/root.tscn");
	private CanvasLayer mainMenu;
	private CanvasLayer modesMenu;
	private CanvasLayer optionsMenu;

	public override void _Ready() {
		mainMenu = GetNode<CanvasLayer>("MainMenu");
		modesMenu = GetNode<CanvasLayer>("Modes");
		optionsMenu = GetNode<CanvasLayer>("OptionsMenu");
	}

	// Exits the game
	private void OnQuitPressed(){
        GetTree().Quit();
    }

	private void OnPlayButtonPressed(){
		mainMenu.Visible = false;
		modesMenu.Visible = true;
	}

	private void LevelsButtonPressed(){
		gameManager.GlobalValues.Mode = "Levels";
		GetTree().ChangeSceneToPacked(levelsMode);
	}

	private void EndlessMode() {
		gameManager.GlobalValues.Mode = "Endless";
		gameManager.GlobalValues.currentLevel = 0;
		GetTree().ChangeSceneToPacked(endlessMode);
	}


	private void OnOptionsPressed(){
		mainMenu.Visible = false;
		optionsMenu.Visible = true;
	}
}
