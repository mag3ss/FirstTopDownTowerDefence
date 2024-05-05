using Godot;

public partial class Edmondossss : Node2D
{
	PackedScene standardTower = ResourceLoader.Load("res://Towers/defence_tower.tscn") as PackedScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddChild(standardTower.Instantiate());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
}
