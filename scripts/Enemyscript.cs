using Godot;
using System;

public partial class Enemyscript : CharacterBody2D, IDamageable
{
	public Guid id { get;set; }
	public int enemySpeed { get; set; } = 2;
    public ProgressBar healthBar;
    public CustomSignals _customSignals;
    public PathFollow2D _follow;
    public Timer attackTimer;
	private Timer _timer;

    [Export] protected float enemyDamage { get; set; } = 5;
    [Export] protected float enemyHealth { get; set; } = 50;
    [Export] protected int enemyValue { get; set; } = 10;

	public override void _Ready()
	{
		_timer = GetTree().Root.GetChild(1).GetNode<Timer>("EnemyMoveTimer");
		_timer.Timeout += _on_move_timer_timeout;
		id = Guid.NewGuid();
		healthBar = GetNode<ProgressBar>("HealthBar");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.EnemyDamage += HandleEnemyDamage;
		_follow = GetParent<PathFollow2D>();
		attackTimer = GetNode<Timer>("AttackTimer");
		healthBar.MaxValue = enemyHealth;
		UpdateHealthBar();
	}

	public override void _PhysicsProcess(double delta) {
		if (_follow.ProgressRatio < 1){
			attackTimer.Start();
		}
	}

	public void SetDamage(float damage) {
        enemyDamage *= damage;
    }

	public void HandleEnemyDamage(int damageDealt, string Id){
		if (Id != id.ToString())
			return;
		enemyHealth -= damageDealt;
		UpdateHealthBar();
		if (enemyHealth <= 0){
			gameManager.GlobalValues.aliveEnemies -= 1;
			gameManager.GlobalValues.playerCurrency += enemyValue;
			gameManager.GlobalValues.killedEnemies += 1;
			QueueFree();
		}
	}

	public void UpdateHealthBar(){
        healthBar.Value = enemyHealth;
    }


	private void _on_move_timer_timeout(){
		_follow.Progress += 1 * enemySpeed;
	}

	public void OnAttackTimerTimeout(){
		gameManager.GlobalValues.playerHealth -= enemyDamage;
	}
}

public interface IDamageable
{
	void SetDamage(float damage);
}
