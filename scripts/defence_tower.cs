using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

public partial class defence_tower : CharacterBody2D
{
    public Node2D target;

    public Array<Node2D> enemies;

    private Timer attackTimer;

    private Area2D attackArea;

    //private CustomSignals _customSignals;


    private CollisionShape2D attackRange;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
        attackTimer = GetNode<Timer>("AttackTimer");
        attackArea = GetNode<Area2D>("AttackRange");

        enemies = new Array<Node2D>();

        // Connect the signals from the attack area
        gameManager.defenceTowerInstance = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        // Find the nearest enemy
        Node2D nearestEnemy = null;
        float shortestDistance = float.MaxValue;

        foreach (Node2D enemy in enemies)
        {
            // Check if the enemy is valid
            if (IsInstanceValid(enemy))
            {
                
                // Get the distance to the enemy
                float distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);

                // Compare the distance to the shortest distance
                if (distance < shortestDistance)
                {
                    // Update the nearest enemy and the shortest distance
                    nearestEnemy = enemy;
                    shortestDistance = distance;
                    LookAt(nearestEnemy.GlobalPosition);
                }
            }
        }

        // Set the target to the nearest enemy
        target = nearestEnemy;
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
        if (body.IsInGroup("enemys"))
        {
            enemies.Remove(body);
            if (enemies.Count == 0)
            {
                attackTimer.Stop();
            }
        }
    }
}
