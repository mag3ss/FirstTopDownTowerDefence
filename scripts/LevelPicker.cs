using Godot;

public partial class LevelPicker : Control
{
	PackedScene endlessMode = ResourceLoader.Load<PackedScene>("res://scenes/gameManager.tscn");
	RWFile rwFile = new RWFile();
	private bool[] levelsUnlocked = new bool[10] {true, false, false, false, false, false, false, false, false, false};
	private CanvasLayer mainMenu;
	private Label StarLabel;
	public override void _Ready()
	{
		StarLabel = GetNode<Label>("StarCountLabel");
		StarLabel.Text = "Stars: " + StarsCount().ToString();
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

		foreach (Panel levels in GetNode("CanvasLayer").GetChildren())
		{
			if (levels == null)
			{
				GD.Print($"Panel at index {index} is null");
				index++;
				continue;
			}

			if (Godot.FileAccess.FileExists($"res://Data/LevelData/Level{index}.txt"))
			{
				SaveStruct dataSave = rwFile.Load(index);
				if (dataSave.IsDefault())
				{
					GD.Print($"Failed to load data for Level{index}");
				}
				else
				{
					var scoreLabel = levels.GetNode<Label>("Score");
					if (scoreLabel == null)
					{
						GD.Print($"Score label not found in Panel at index {index}");
					}
					else
					{
						scoreLabel.Text = "Score: " + dataSave.Score.ToString();
					}
				}
			}

			SaveStruct newData = rwFile.Load(index);
			if (newData.IsDefault())
			{
				GD.Print($"Failed to load new data for Level{index}");
			}
			else
			{
				LevelEnum currentLevelEnum = (LevelEnum)index;
				if (Levels.LevelInfo.TryGetValue(currentLevelEnum, out LevelData specificLevelData))
				{
					if (specificLevelData == null)
					{
						GD.Print($"No LevelData found for {currentLevelEnum}");
					}
					else
					{
						if (newData.Score >= specificLevelData.Score)
						{
							var star1 = levels.GetNode<Sprite2D>("Star1");
							if (star1 == null)
							{
								GD.Print($"EmptyStar1 not found in Panel at index {index}");
							}
							else
							{
								star1.Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
							}
						}
						if (newData.Wave >= specificLevelData.Wave)
						{
							var star2 = levels.GetNode<Sprite2D>("Star2");
							if (star2 == null)
							{
								GD.Print($"EmptyStar2 not found in Panel at index {index}");
							}
							else
							{
								star2.Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
							}
						}
						if (newData.LengthAlive >= specificLevelData.Time)
						{
							var star3 = levels.GetNode<Sprite2D>("Star3");
							if (star3 == null)
							{
								GD.Print($"EmptyStar3 not found in Panel at index {index}");
							}
							else
							{
								star3.Texture = ResourceLoader.Load<Texture2D>("res://assets/UI/FilledStar.png");
							}
						}
						rwFile.Save(newData.Score, newData.LengthAlive, newData.KilledEnemies, newData.Wave, newData.Stars, index);
					}
				}
				else
				{
					GD.Print($"LevelEnum {currentLevelEnum} not found in LevelInfo");
				}
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

	private void OnPlayButton(int index) {
		gameManager.GlobalValues.currentLevel = index;
		if (endlessMode != null) {
			GetTree().ChangeSceneToPacked(endlessMode);
		}
		
	}
}
