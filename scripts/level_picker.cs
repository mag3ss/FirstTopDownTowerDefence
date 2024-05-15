using Godot;
using System;

public partial class level_picker : Control
{
	private bool[] levelsUnlocked = new bool[10] {true, false, false, false, false, false, false, false, false, false};
	private CanvasLayer mainMenu;
	public override void _Ready()
	{
		mainMenu = GetNode<CanvasLayer>("WMIcons");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void TogglePanel(int levelIndex, bool unlocked) {
		GetNode<Panel>("CanvasLayer/Level" + levelIndex).Visible = !GetNode<Panel>("CanvasLayer/Level" + levelIndex).Visible;
		foreach (TextureButton child in mainMenu.GetChildren()) {
			child.Disabled = true;
			if (child.Name == "Icon" + levelIndex) {
				child.Disabled = false;
			} else if (unlocked) {
				child.Disabled = false;
			}
		}
	}
}
