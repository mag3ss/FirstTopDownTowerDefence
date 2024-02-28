using Godot;
using System;


public partial class Enemyscript : CharacterBody2D
{
    public Guid id { get;private set; }
    private ProgressBar healthBar;

    private PathFollow2D _follow;
	private Timer _timer;
	[Export] private int enemyDamage = 1;
	private Timer attackTimer;
	[Export] private int enemyHealth = 25;
	[Export] float enemySpeed = 5f;
	private CustomSignals _customSignals;
	private int enemyValue = 25;

	public override void _Ready()
	{
        id = Guid.NewGuid();
        healthBar = GetNode<ProgressBar>("HealthBar");
        _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        _customSignals.EnemyDamage += HandleEnemyDamage;
		_follow = GetParent<PathFollow2D>();
		attackTimer = GetNode<Timer>("AttackTimer");

		healthBar.Value = enemyHealth;
		
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta)
    {
        if (_follow.ProgressRatio < 0.999)
        {
            attackTimer.Start();
        }
    }

	private void HandleEnemyDamage(int damageDealt, string Id)
	{
		if (Id != id.ToString())
			return;
		
        enemyHealth -= damageDealt;
        healthBar.Value = enemyHealth;
		if (enemyHealth <= 0)
		{
			gameManager.GlobalValues.playerCurrency += enemyValue;
			_customSignals.EmitSignal(nameof(CustomSignals.ChangedCurrency));
			QueueFree();
		}
    }


    public void _on_move_timer_timeout()
	{
        _follow.Progress += 1 * enemySpeed;
	}

	public void _on_attack_timer_timeout()
	{
		gameManager.GlobalValues.playerHealth -= enemyDamage;
	}
}
