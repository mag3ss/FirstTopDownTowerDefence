using Godot;

public partial class LevelPicker : Control
{
	PackedScene endlessMode = ResourceLoader.Load<PackedScene>("res://scenes/root.tscn");
	RWFile rwFile = new RWFile();
	private bool[] levelsUnlocked = new bool[10] {true, false, false, false, false, false, false, false, false, false};
	private CanvasLayer mainMenu;
	private Label StarLabel;
	public override void _Ready()
	{
		StarLabel = GetNode<Label>("StarCountLabel");
		StarLabel.Text = "Stars: " + StarsCount().ToString();
		rwFile.Save(10400, 1, 1, 1, 100, 1, 1);
		mainMenu = GetNode<CanvasLayer>("WMIcons");
		
		switch (StarsCount()){
			case 1:
				levelsUnlocked[1] = true;
				GetNode<TextureButton>("WMIcons/Icon2").TooltipText = "";
				break;
			case 3:
				levelsUnlocked[2] = true;
				GetNode<TextureButton>("WMIcons/Icon3").TooltipText = "";
				break;
			case 5:
				levelsUnlocked[3] = true;
				GetNode<TextureButton>("WMIcons/Icon4").TooltipText = "";
				break;
			case 7:
				levelsUnlocked[4] = true;
				GetNode<TextureButton>("WMIcons/Icon5").TooltipText = "";
				break;
			case 9:
				levelsUnlocked[5] = true;
				GetNode<TextureButton>("WMIcons/Icon6").TooltipText = "";
				break;
			case 11:
				levelsUnlocked[6] = true;
				GetNode<TextureButton>("WMIcons/Icon7").TooltipText = "";
				break;
			case 13:
				levelsUnlocked[7] = true;
				GetNode<TextureButton>("WMIcons/Icon8").TooltipText = "";
				break;
			case 15:
				levelsUnlocked[8] = true;
				GetNode<TextureButton>("WMIcons/Icon9").TooltipText = "";
				break;
			case 21:
				levelsUnlocked[9] = true;
				GetNode<TextureButton>("WMIcons/Icon10").TooltipText = "";
				break;
		}
		
		int index = 1;
		foreach (Panel levels in GetNode("CanvasLayer").GetChildren()) {
			if (Godot.FileAccess.FileExists($"res://Data/LevelData/Level{index}.txt")) {
				SaveStruct dataSave = rwFile.Load(index);
				levels.GetNode<Label>("Score").Text = "Score: " + dataSave.Score.ToString();
			}
			index++;
		}
	}

	private int StarsCount() {
		int index = 1;
		int starCount = 0;
		while (Godot.FileAccess.FileExists($"res://Data/LevelData/Level{index}.txt")) {
			SaveStruct dataSave = rwFile.Load(index);
			starCount += dataSave.Stars;
			index++;
		}
		return starCount;
	}

	private void TogglePanel(int levelIndex, bool unlocked) {
		if (levelsUnlocked[levelIndex - 1]) {
			GetNode<Panel>("CanvasLayer/Level" + levelIndex).Visible = !GetNode<Panel>("CanvasLayer/Level" + levelIndex).Visible;
			foreach (TextureButton child in mainMenu.GetChildren()) {
			child.Disabled = true;
			if (child.Name == "Icon" + levelIndex) {
				child.Disabled = false;
			} else if (unlocked) {
				child.Disabled = false;
			}}
			gameManager.GlobalValues.currentLevel = levelIndex;
		}
	}

	private void OnPlayButton() {
		GetTree().ChangeSceneToPacked(endlessMode);
	}
}
