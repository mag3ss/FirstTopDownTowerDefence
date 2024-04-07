using Godot;
using System;

public partial class main_menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void OnQuitPressed()
	{
        GetTree().Quit();
    }


}
