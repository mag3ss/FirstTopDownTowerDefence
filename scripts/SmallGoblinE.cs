using Godot;
using System;

public partial class SmallGoblinE : Enemyscript
{
    public override void _Ready()
    {
        id = Guid.NewGuid();
        healthBar = GetNode<ProgressBar>("HealthBar");
        _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        _customSignals.EnemyDamage += HandleEnemyDamage;
        _follow = GetParent<PathFollow2D>();
        attackTimer = GetNode<Timer>("AttackTimer");
        healthBar.Value = enemyHealth;

        enemyHealth = 25;
    }
}
