using Godot;
using System;

public partial class Enemyscript : CharacterBody2D
{
	public Guid id { get;set; }
	public int enemySpeed = 2;
    public ProgressBar healthBar;
    public CustomSignals _customSignals;
    public PathFollow2D _follow;
    public Timer attackTimer;
	private Timer _timer;

    [Export] public int enemyDamage = 1;
    [Export] public float enemyHealth = 25;
    [Export] public int enemyValue = 10;
	private RemoteTransform2D _transform;

	public override void _Ready()
	{
		_timer = GetTree().Root.GetChild(1).GetNode<Timer>("EnemyMoveTimer");
		_timer.Timeout += _on_move_timer_timeout;
        _transform = GetNode<Node>("Node").GetChild<RemoteTransform2D>(0);
		id = Guid.NewGuid();
		healthBar = GetNode<ProgressBar>("HealthBar");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.EnemyDamage += HandleEnemyDamage;
		_follow = GetParent<PathFollow2D>();
		attackTimer = GetNode<Timer>("AttackTimer");
		healthBar.MaxValue = enemyHealth;
		UpdateHealthBar();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_follow.ProgressRatio < 0.999){
			attackTimer.Start();
		}
	}

	public void HandleEnemyDamage(int damageDealt, string Id){
		if (Id != id.ToString())
			return;
		enemyHealth -= damageDealt;
		UpdateHealthBar();
		if (enemyHealth <= 0){
			gameManager.GlobalValues.playerCurrency += enemyValue;
			QueueFree();
		}
	}

	public void UpdateHealthBar(){
        healthBar.Value = enemyHealth;
    }


	private void _on_move_timer_timeout(){
		_follow.Progress += 1 * enemySpeed;
	}

	public void _on_attack_timer_timeout(){
		gameManager.GlobalValues.playerHealth -= enemyDamage;
	}
}
