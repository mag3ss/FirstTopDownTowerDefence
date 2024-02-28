using Godot;
using System;


public partial class bullet : RigidBody2D
{

	// Called when the node enters the scene tree for the first time.
	[Export] int bulletDamage = 1;
	private CustomSignals _customSignals;
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        Timer timer = GetNode<Timer>("BulletTimer");
        timer.Timeout += () => QueueFree();
    }



	public void _on_body_entered(Enemyscript body)
	{
		if (body.IsInGroup("enemys"))
		{
			_customSignals.EmitSignal(nameof(CustomSignals.EnemyDamage), bulletDamage, body.id.ToString());
			
			QueueFree();
		}
	}
}
