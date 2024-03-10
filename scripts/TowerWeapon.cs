using Godot;
using System;

public partial class TowerWeapon : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] PackedScene bulletScene;
	[Export] float bullet_speed = 400;
	[Export] float bps = 5f;
	[Export] float bullet_damage = 30f;

	float fire_rate;

    public override void _Ready()
	{
		fire_rate = 1 / bps;
	}

	public void _on_attack_timer_timeout()
	{
        if (gameManager.defenceTowerInstance.enemies.Count !>= 0)
        {
            RigidBody2D bullet = bulletScene.Instantiate<RigidBody2D>();

            bullet.Rotation = GlobalRotation;
            bullet.Position = GlobalPosition;
            bullet.LinearVelocity = bullet.Transform.X * bullet_speed;

            GetTree().Root.AddChild(bullet);
        }

    }


}
