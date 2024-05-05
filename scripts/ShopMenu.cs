using System;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Collections;

public partial class ShopMenu : CanvasLayer
{
	private CustomSignals _customSignals;
	private bool _isMouseOver;
	private bool _isTowerSelected;
	private CharacterBody2D _towerrr;
	private Node2D _tower;
	private Node2D newTower;
	private int tempValue;
	private int towerID;
	private Array<PackedScene> towerCollecion = new Array<PackedScene>();
	[Export] private PackedScene FireWizard;
	[Export] private PackedScene Exectioner;
 
	public override void _Ready() {
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_tower = GetParent().GetNode<Node2D>("TowerSpawner");
		// -------------------------------------
		// Tower Collection
		// -------------------------------------
		towerCollecion.Add(FireWizard);
		towerCollecion.Add(Exectioner);
		// -------------------------------------
	}

	public override void _Process(double delta){
		if (newTower != null){
			newTower.GlobalPosition = newTower.GetGlobalMousePosition();
			if (!Input.IsMouseButtonPressed((MouseButton)1)){
				if (gameManager.GlobalValues.IsOccupied == false){
					var ActualTower = (Node2D)towerCollecion[towerID].Instantiate();
					_tower.AddChild(ActualTower);
					ActualTower.GlobalPosition = newTower.GlobalPosition;
				} else {
					gameManager.GlobalValues.playerCurrency += tempValue;
				}
				newTower.QueueFree();
				newTower = null;
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
			towerID = _towerID;
			HandleBuyedTower(_towerID);
		}
	}

	private void HandleBuyedTower(int IDnumber){
		hideMenu();
		newTower = (Node2D)towerCollecion[IDnumber].Instantiate();
		foreach (Node child in newTower.GetChildren()){
			if (child is TextureRect textureRect){
				textureRect.Visible = true;
			} else if (child is Panel range) {
				range.Visible = true;
			} else if (child is Area2D area){
				area.Visible = false;
			}
		}
		_tower.GetParent().AddChild(newTower);
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






