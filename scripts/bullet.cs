using Godot;

public partial class Bullet : CharacterBody2D
{

	// Called when the node enters the scene tree for the first time.
	[Export] int bulletDamage = 1;
	private CustomSignals _customSignals;
	public float speed = 300;
	private ulong previousName;
	private float speedMultiplier = 1f;
	
	
	public override void _Ready()
	{
		//Checks if parent (Tower) is valid to then get the target
		var parentTower = (DefenceTower)GetParent().GetParent();
		if (parentTower.target != null){
			previousName = parentTower.target.GetInstanceId();
		}
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		Timer timer = GetNode<Timer>("BulletTimer");
		timer.Timeout += () => QueueFree();
        _customSignals.ChangeSpeed += ChangedEnemySpeed;

    }

	//Called when a enemy entered the bullet area
    public void OnArea2dBodyEntered(Enemyscript body){
		if (body.IsInGroup("enemys")){
			_customSignals.EmitSignal(nameof(CustomSignals.EnemyDamage), bulletDamage, body.id.ToString());
			QueueFree();
		}
	}

	// Constantly moves the bullet towards the target, if target changes. Then it stops rotating ()
	public override void _PhysicsProcess(double delta){
		var parent = (DefenceTower)GetParent().GetParent();
		if (parent.target != null){
			var tempName = parent.target.GetInstanceId();
			if (tempName == previousName){
				LookAt(parent.target.GlobalPosition);
			}
		}
        Velocity = Transform.X * speed * speedMultiplier;
        MoveAndSlide();
	}

	//Changes the speed of the bullet
    private void ChangedEnemySpeed(int speedChange){
        speedMultiplier = speedChange;
    }
}
