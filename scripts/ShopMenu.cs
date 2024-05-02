using Godot;

public partial class ShopMenu : CanvasLayer
{
	private CustomSignals _customSignals;
	private bool _isMouseOver;
	private bool _isTowerSelected;
	private CharacterBody2D _towerrr;
	private Node2D _tower;
	private Node2D icon1;
	private int tempValue;

	private Resource _tileMap;

	
	PackedScene standardTower = ResourceLoader.Load("res://Towers/defence_tower.tscn") as PackedScene;
 
		public override void _Ready()
		{
			_tileMap = GetNode<Resource>("res://Towers/TowerImages/TowerIcons.tres");
			GD.Print(_tileMap);
	        

			_customSignals = GetTree().Root.GetNode<CustomSignals>("CustomSignals");
			_customSignals.BuyedTower += HandleBuyedTower;
			_tower = GetParent().GetNode<Node2D>("TowerSpawner");
		}

	public override void _Process(double delta){
		if (icon1 != null){
			icon1.GlobalPosition = icon1.GetGlobalMousePosition();
			if (!Input.IsMouseButtonPressed((MouseButton)1)){
				if (gameManager.GlobalValues.IsOccupied == false)
				{
					Node2D standard_tower = (Node2D)standardTower.Instantiate();
					_tower.AddChild(standard_tower);
					standard_tower.Position = icon1.GlobalPosition;
					icon1.QueueFree();
					icon1 = null;
				}
				else
				{
					gameManager.GlobalValues.playerCurrency += tempValue;
					icon1.QueueFree();
					icon1 = null;
				}
				
			}
		}
	}

	public override void _Input(InputEvent @event){
		if (Input.IsActionJustPressed("Shop")){
			hideMenu();
		}
	}

	private void OnGuiInput(InputEvent @event, int _towerID, int _towerCost){
		if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && _towerCost <= gameManager.GlobalValues.playerCurrency){
			gameManager.GlobalValues.playerCurrency -= _towerCost;
			_customSignals.EmitSignal(nameof(CustomSignals.ChangedCurrency));
			_customSignals.EmitSignal(nameof(CustomSignals.BuyedTower), _towerID, _towerCost);
		}
	}

	public void HandleBuyedTower(int IDnumber, int towerCost){
		hideMenu();
		switch (IDnumber){
			case 100:
				PressedTower("Tower1");
				towerCost = tempValue;
				break;
			case 101:
				GD.Print("Tower 2 bought");
				break;
			case 102:
				GD.Print("Tower 3 bought");
				break;
			case 103:
				GD.Print("Tower 4 bought");
				break;
			default:
				GD.Print("No tower bought");
				break;
		}
	}

	public void PressedTower(string IDnum){
		// GetTree().Root.AddChild(towerCollecion[IDnum].Instantiate());
		icon1 = GetTree().Root.GetNode<Node2D>("Node2D");
		_isTowerSelected = true;
	}

	private void hideMenu(){
        Visible = !Visible;
        if (gameManager.GlobalValues.IsOnMenu){
            gameManager.GlobalValues.IsOnMenu = false;
        } else {
            gameManager.GlobalValues.IsOnMenu = true;
        }
    }
}






