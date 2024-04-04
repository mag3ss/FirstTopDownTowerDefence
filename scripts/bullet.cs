using Godot;
using System;
using static Godot.TextServer;


public partial class bullet : CharacterBody2D
{

	// Called when the node enters the scene tree for the first time.
	[Export] int bulletDamage = 1;
	private CustomSignals _customSignals;
	float speed = 300;
	private string previousName;
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
		var enemyTarget = parent.target;
		if (enemyTarget != null){
            if (enemyTarget.Name != previousName){
                LookAt(enemyTarget.GlobalPosition);
                previousName = enemyTarget.Name;
                Velocity = Vector2.Zero;
            }
            else
            {
                Velocity = Transform.X * speed;
            }
        }
        MoveAndSlide();
    }
}
