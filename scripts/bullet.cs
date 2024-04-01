using Godot;
using System;
using static Godot.TextServer;


public partial class bullet : CharacterBody2D
{

	// Called when the node enters the scene tree for the first time.
	[Export] int bulletDamage = 1;
	private CustomSignals _customSignals;
	float speed = 300;
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		Timer timer = GetNode<Timer>("BulletTimer");
		timer.Timeout += () => QueueFree();
	}



	public void OnArea2dBodyEntered(Enemyscript body)
	{
		if (body.IsInGroup("enemys"))
		{
			_customSignals.EmitSignal(nameof(CustomSignals.EnemyDamage), bulletDamage, body.id.ToString());
			
			QueueFree();
		}
	}


    public override void _PhysicsProcess(double delta)
    {
		var parent = (defence_tower)GetParent().GetParent();

		Velocity = Transform.X * speed;
		LookAt(parent.target.GlobalPosition);
        MoveAndSlide();
    }
}
