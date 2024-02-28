using Godot;
using System;

public partial class Tower_1 : Panel
{
    // Called when the node enters the scene tree for the first time.
    private CustomSignals _customSignals;
    public int _towerID = 100;
    private int _towerCost = 50;
    public override void _Ready(){
        _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
    }

	private void OnGuiInput(InputEvent @event){
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && _towerCost < gameManager.GlobalValues.playerCurrency){
            gameManager.GlobalValues.playerCurrency -= _towerCost;
            _customSignals.EmitSignal(nameof(CustomSignals.ChangedCurrency));
            _customSignals.EmitSignal(nameof(CustomSignals.BuyedTower), _towerID, _towerCost);
        }
    }
}
