using Godot;

public partial class PlayerCamera2D : Camera2D
{
    // Called when the node enters the scene tree for the first time.
    [Export] public float MinZoom = 0.8f;
    [Export] public float MaxZoom = 1.4f;
    [Export] public float ZoomFactor = 0.1f;

    private float _zoomLevel = 1.0f;
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustReleased("Scroll_Up") && gameManager.GlobalValues.IsOnMenu == false && _zoomLevel < MaxZoom)
        {
            _zoomLevel += ZoomFactor;
            Zoom = new Vector2(_zoomLevel, _zoomLevel);
        }
        else if (Input.IsActionJustReleased("Scroll_Down") && gameManager.GlobalValues.IsOnMenu == false && _zoomLevel > MinZoom)
        {
            _zoomLevel -= ZoomFactor;
            Zoom = new Vector2(_zoomLevel, _zoomLevel);
        }
    }
}


