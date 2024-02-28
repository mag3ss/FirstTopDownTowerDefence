using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
    [Export] public int StepSize = 3;
    public override void _Ready()
	{

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		InputHandler();
	}


    private void InputHandler()
    {

        if (Input.IsActionPressed("down"))
        {
            Vector2 newPosition = Position + new Vector2(0, 1 * StepSize);
            Position = newPosition;
        }
        if (Input.IsActionPressed("up"))
        {
            Vector2 newPosition = Position + new Vector2(0, -1 * StepSize);
            Position = newPosition;
        }
        if (Input.IsActionPressed("left"))
        {
            Vector2 newPosition = Position + new Vector2(-1 * StepSize, 0);
            Position = newPosition;
        }
        if (Input.IsActionPressed("right"))
        {
            Vector2 newPosition = Position + new Vector2(1 * StepSize, 0);
            Position = newPosition;
        }


    }
}
