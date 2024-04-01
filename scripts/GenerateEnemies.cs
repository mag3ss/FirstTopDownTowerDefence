using Godot;

public partial class GenerateEnemies : Path2D
{

	PackedScene packedScene = ResourceLoader.Load("res://entities/enemies/enemy.tscn") as PackedScene;
	private PathFollow2D pathToFollow;
	private Timer timer;
	private Timer wavePause;

	private bool ongoingWave;
	private int WaveNum = 1;
	private float difficulty = 1;
	private int totalEnemies;


	public override void _Ready()
	{
		timer = GetParent().GetNode<Timer>("GameTimer");
        wavePause = GetParent().GetNode<Timer>("WavePause");
        NewWave();
	}


	private void NewWave()
	{
        WaveNum++;
		timer.Start();
		ongoingWave = true;
		difficulty *= 1.075f;
    }


	public void _on_game_timer_timeout()
	{
		PathFollow2D pathToFollow = new PathFollow2D();
		AddChild(pathToFollow);
		pathToFollow.Loop = false;
		var enemyCharacter = (Enemyscript)packedScene.Instantiate();
		//enemyCharacter.enemyHealth = (int)(enemyCharacter.enemyHealth);
		pathToFollow.AddChild(enemyCharacter);
		enemyCharacter.AddToGroup("enemys");
		totalEnemies++;
		if (totalEnemies == 0)
		{
            timer.Stop();
        }
	}

	public void OnWavePauseTimeout()
	{
        NewWave();
		timer.Start();
    }
}
