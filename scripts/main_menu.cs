using Godot;
using System;

public partial class main_menu : Control
{
	PackedScene LevelsMode = ResourceLoader.Load<PackedScene>("res://scenes/level_picker.tscn");
	private CanvasLayer mainMenu;
	private CanvasLayer modesMenu;
	private CanvasLayer optionsMenu;

	public override void _Ready() {
		mainMenu = GetNode<CanvasLayer>("MainMenu");
		modesMenu = GetNode<CanvasLayer>("Modes");
		optionsMenu = GetNode<CanvasLayer>("OptionsMenu");
	}

	private void OnQuitPressed(){
        GetTree().Quit();
    }

	private void OnPlayButtonPressed(){
		mainMenu.Visible = false;
		modesMenu.Visible = true;
	}

	private void LevelsButtonPressed(){
		GetTree().ChangeSceneToPacked(LevelsMode);
	}


	private void OnOptionsPressed(){
		mainMenu.Visible = false;
		optionsMenu.Visible = true;
	}
}
