using Godot;

public partial class main_menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	private void OnQuitPressed(){
        GetTree().Quit();
    }

	private void OnPlayButtonPressed(){
	}
}
