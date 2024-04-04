using Godot;
using System;
using System.Runtime.CompilerServices;


public partial class Enemyscript : CharacterBody2D
{
	public Guid id { get;set; }
	public ProgressBar healthBar;
    public CustomSignals _customSignals;
    public PathFollow2D _follow;
	public Timer _timer;
    public Timer attackTimer;

    [Export] public int enemyDamage = 1;
	[Export] public float enemySpeed = 2f;
    [Export] public float enemyHealth = 25;
    public int enemyValue = 25;
	private RemoteTransform2D _transform;

	public override void _Ready()
	{
		_transform = GetNode<Node>("Node").GetChild<RemoteTransform2D>(0);
		id = Guid.NewGuid();
		healthBar = GetNode<ProgressBar>("HealthBar");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.EnemyDamage += HandleEnemyDamage;
		_follow = GetParent<PathFollow2D>();
		attackTimer = GetNode<Timer>("AttackTimer");
		healthBar.Value = enemyHealth;
		_transform.RemotePath = healthBar.GetPath();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _PhysicsProcess(double delta)
	{
		if (_follow.ProgressRatio < 0.999)
		{
			attackTimer.Start();
		}
	}

	public void HandleEnemyDamage(int damageDealt, string Id)
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
