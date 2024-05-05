using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class defence_tower : StaticBody2D
{
	[Export] private int towerValue;
	[Export] PackedScene bulletScene;
	private bool isShooting;
	private CharacterBody2D bullet;
	public int speedMultiplier = 1;
	private Node bulletContainer;
	private Marker2D Aim;
	private bool onMenu;
	private Panel attackRangeVisiual;

	public float fire_rate;
	public Node2D target;

	public Array<Node2D> enemies;

	private Timer attackTimer;
	private Timer followTimer;

	private Area2D attackArea;

	private CanvasLayer upgradeMenu;

	//private CustomSignals _customSignals;

	private bool lookAtClosestEnemy;

	private CollisionShape2D attackRange;

	private AnimatedSprite2D towerSprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		towerSprite = GetNode<AnimatedSprite2D>("TowerSprite");
		attackRangeVisiual = GetNode<Panel>("Range");
		Aim = GetNode<Marker2D>("Aim");
		bulletContainer = GetNode<Node>("BulletContainer");
		upgradeMenu = GetNode<CanvasLayer>("UpgradeMenu");
		attackTimer = GetNode<Timer>("AttackTimer");
		attackArea = GetNode<Area2D>("AttackRange");


		enemies = new Array<Node2D>();
		gameManager.defenceTowerInstance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta){
		if (lookAtClosestEnemy) {
			LookAtClosestEnemy();
		} else {
			LookAtHighestProgress();
		}
	}

	public void OnSellButtonPressed(){
		gameManager.GlobalValues.playerCurrency += towerValue;
		QueueFree();
	}

	private void LookAtClosestEnemy(){
		target = null;
		float minDistance = float.MaxValue;
		foreach (Node2D enemy in enemies) {
			float distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
			if (distance < minDistance) {
				target = enemy;
				minDistance = distance;
			}
		}

		if (target != null) {
			LookAt(target.GlobalPosition);
		}
	}

	private void LookAtHighestProgress(){
		target = null; 
		float maxProgress;
		foreach (Node2D enemy in enemies) {
			PathFollow2D path = (PathFollow2D)enemy.GetParent();
			float progress = path.ProgressRatio;
			if (progress < 100 && (target == null || progress > ((PathFollow2D)target.GetParent()).ProgressRatio)) {
				target = enemy;
				maxProgress = progress;
			}
		}
		if (target != null) {
			LookAt(target.GlobalPosition);
		}
	}


	private void _on_attack_range_body_entered(Node2D body) {
		if (body.IsInGroup("enemys")) {
			enemies.Add(body);
			attackTimer.Start();
		}
	}

	private void _on_attack_range_body_exited(Node2D body) {
		List<Node2D> enemiesToRemove = new List<Node2D>();
		foreach (Node2D child in enemies) {
			if (child == body) {
				enemiesToRemove.Add(child);
			}
		}

		if (enemies.Count <= 0) {
			attackTimer.Stop();
		}

		//Temp Array 
		foreach (Node2D enemyToRemove in enemiesToRemove) {
			enemies.Remove(enemyToRemove);
		}
	}

	private void _on_attack_timer_timeout(){
		isShooting = true;
		if (gameManager.defenceTowerInstance.enemies.Count >= 0 && target != null){
			towerSprite.Play("Attack");
			bullet = bulletScene.Instantiate<CharacterBody2D>();
			bulletContainer.AddChild(bullet);
			bullet.GlobalPosition = Aim.GlobalPosition;
			bullet.LookAt(target.GlobalPosition);
			bullet.Velocity = bullet.Transform.X * 300 * speedMultiplier;
		}
	}

	private void LookAtNearestButton(){
		lookAtClosestEnemy = true;
		GD.Print("Look at nearest");
	}

	private void LookAtFirstEnemyButton(){
		lookAtClosestEnemy = false;
		GD.Print("Look at first");
	}

	public void OnInputEvent(Viewport viewport, InputEvent @event, int shapeIdx){
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonMask == (MouseButtonMask)1){
			var towerPath = GetTree().Root.GetNode<Node2D>("root/TowerSpawner");
			foreach (StaticBody2D towers in towerPath.GetChildren()){
				if (towers.Name != this.Name){
					towers.GetNode<CanvasLayer>("UpgradeMenu").Hide();
					towers.GetNode<Panel>("Range").Hide();
				}
			}
			upgradeMenu.Visible = !upgradeMenu.Visible;
			attackRangeVisiual.Visible = !attackRangeVisiual.Visible;
			onMenu = !onMenu;
			GD.Print("Clicked");
		}
	}
}


