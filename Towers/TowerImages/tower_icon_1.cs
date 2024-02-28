using Godot;
using System;

public partial class tower_icon_1 : Node2D
{
	private Panel _Red;
	private bool _inside;
	public override void _Ready(){
		_Red = GetNode<Panel>("Red");
	}

	public void OnArea2dAreaEntered(Area2D area){
		if(area.IsInGroup("FriendlyTower")){
			_Red.Visible = true;
			gameManager.GlobalValues.IsOccupied = true;
		}
	}
	public void OnArea2dAreaExited(Area2D area){
        _Red.Visible = false;
        gameManager.GlobalValues.IsOccupied = false;
    }
	

}
