using Godot;

public partial class PlayerCamera2D : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public float MinZoom { get; set; } = 0.8f;
	[Export] public float MaxZoom { get; set; } = 1.4f;
	[Export] public float ZoomFactor { get; set; } = 0.1f;

	private float _zoomLevel = 1.0f;

	public override void _Process(double delta)
	{
		if (Input.IsActionJustReleased("Scroll_Up") && gameManager.GlobalValues.IsOnMenu == false && Zoom <= new Vector2(2,2))
		{
			Zoom *= MaxZoom;
			
		}
		else if (Input.IsActionJustReleased("Scroll_Down") && gameManager.GlobalValues.IsOnMenu == false && Zoom >= new Vector2(1, 1))
		{
			Zoom *= MinZoom;
		}
	}
}


