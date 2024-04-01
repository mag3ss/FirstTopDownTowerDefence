using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

public partial class defence_tower : StaticBody2D
{
    [Export] PackedScene bulletScene;
    [Export] float bullet_speed = 400;
    [Export] float bps = 5f;
    [Export] float bullet_damage = 30f;
    private bool isShooting;
    private CharacterBody2D bullet; // Declare bullet as a class-level variable
	private Node bulletContainer;
	private Marker2D Aim;
	private bool onMenu;

    float fire_rate;
    public Node2D target;

	public Array<Node2D> enemies;

	private Timer attackTimer;
	private Timer followTimer;

	private Area2D attackArea;

	private CanvasLayer upgradeMenu;

	//private CustomSignals _customSignals;

	private bool lookAtClosestEnemy;

	private CollisionShape2D attackRange;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Aim = GetNode<Marker2D>("Aim");
		bulletContainer = GetNode<Node>("BulletContainer");
		upgradeMenu = GetNode<CanvasLayer>("Menu");
		attackTimer = GetNode<Timer>("AttackTimer");
		attackArea = GetNode<Area2D>("AttackRange");
		followTimer = GetNode<Timer>("FollowTimer");

		enemies = new Array<Node2D>();
        fire_rate = 1 / bps;
		gameManager.defenceTowerInstance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (lookAtClosestEnemy)
		{
			LookAtClosestEnemy();
		}
		else
		{
			LookAtHighestProgress();
		}
    }

    private void LookAtClosestEnemy()
	{
		target = null;
		float minDistance = float.MaxValue;

		foreach (Node2D enemy in enemies)
		{
			float distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);

			if (distance < minDistance)
			{
				target = enemy;
				minDistance = distance;
			}
		}

		if (target != null)
		{
			LookAt(target.GlobalPosition);
		}
	}

	private void LookAtHighestProgress()
	{
		target = null; 
		float maxProgress = -1;

		foreach (Node2D enemy in enemies)
		{
			PathFollow2D path = (PathFollow2D)enemy.GetParent();
			float progress = path.ProgressRatio;

			if (progress < 100 && (target == null || progress > ((PathFollow2D)target.GetParent()).ProgressRatio))
			{
				target = enemy;
				maxProgress = progress;
			}
		}

		if (target != null)
		{
			LookAt(target.GlobalPosition);
		}
	}

	private void _on_attack_range_body_entered(Node2D body)
	{
		if (body.IsInGroup("enemys"))
		{
			enemies.Add(body);
			attackTimer.Start();
		}
	}

	private void _on_attack_range_body_exited(Node2D body)
	{
		List<Node2D> enemiesToRemove = new List<Node2D>();

		foreach (Node2D child in enemies)
		{
			if (child == body)
			{
				enemiesToRemove.Add(child);
			}
		}

		if (enemies.Count <= 0)
		{
			attackTimer.Stop();
		}

		//Temp Array 
		foreach (Node2D enemyToRemove in enemiesToRemove)
		{
			enemies.Remove(enemyToRemove);
		}
	}

    public void _on_attack_timer_timeout()
    {
        isShooting = true;
        if (gameManager.defenceTowerInstance.enemies.Count >= 0)
        {
            bullet = bulletScene.Instantiate<CharacterBody2D>();
            bulletContainer.AddChild(bullet);
			bullet.GlobalPosition = Aim.GlobalPosition;
        }
    }
}


