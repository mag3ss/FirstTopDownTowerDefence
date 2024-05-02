using Godot;
using Godot.Collections;

public partial class ShopMenu : CanvasLayer
{
	private CustomSignals _customSignals;
	private bool _isMouseOver;
	private bool _isTowerSelected;
	private CharacterBody2D _towerrr;
	private Node2D _tower;
	private Node2D icon1;
	private int tempValue;

	private Array<Node2D> towerCollecion = new Array<Node2D>();

	[Export] private Node2D FireWizard;

	private TileMap _tileMap;

	
	PackedScene standardTower = ResourceLoader.Load("res://Towers/defence_tower.tscn") as PackedScene;
 
		public override void _Ready() {
			_tileMap = GetParent().GetNode<TileMap>("TileMap");
			// _tileMap.SetCell(0, new Vector2I(1,1), 0, new Vector2I(1,1));
			_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
			_tower = GetParent().GetNode<Node2D>("TowerSpawner");
		}

	public override void _Process(double delta){
		if (_tileMap != null){
			_tileMap.GlobalPosition = _tileMap.GetGlobalMousePosition();
			if (!Input.IsMouseButtonPressed((MouseButton)1)){
				if (gameManager.GlobalValues.IsOccupied == false)
				{
					Node2D standard_tower = (Node2D)standardTower.Instantiate();
					_tower.AddChild(standard_tower);
					standard_tower.Position = icon1.GlobalPosition;
				} else {
					gameManager.GlobalValues.playerCurrency += tempValue;
				}
				_tileMap.QueueFree();
				_tileMap = null;
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
			HandleBuyedTower(_towerID, _towerCost);
			
		}
	}

	public void HandleBuyedTower(int IDnumber, int towerCost){
		hideMenu();
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






