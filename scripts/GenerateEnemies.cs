using Godot;
using Godot.Collections;

public partial class GenerateEnemies : Path2D
{
	//Enemies
	//-----------Goblins-----------
	[Export] PackedScene SmallGoblin;
	[Export] PackedScene SmallGoblinE;
	//-----------Orcs-----------
	[Export] PackedScene SmallOrc;




	private PathFollow2D pathToFollow;
	private Timer timer;
	private Timer wavePause;
	private int firstWave = 10;

	private bool ongoingWave;
	private int WaveNum = 1;
	private float difficulty = 1;
	private int totalEnemies;
    private Array<PackedScene> enemies = new Array<PackedScene>();

    public override void _Ready()
	{
		enemies.Add(SmallGoblin);
		timer = GetParent().GetNode<Timer>("GameTimer");
        wavePause = GetParent().GetNode<Timer>("WavePause");
		timer.Start();
	}


	private void NewWave()
	{
        WaveNum++;
		timer.Start();
		ongoingWave = true;
		difficulty *= 1.125f;
		GD.Print("Wave " + WaveNum);
    }


	public void _on_game_timer_timeout()
	{
		PathFollow2D pathToFollow = new PathFollow2D();
		AddChild(pathToFollow);
		pathToFollow.Loop = false;
		var enemyCharacter = (Enemyscript)enemies[0].Instantiate();
		enemyCharacter.enemyHealth *= difficulty;
		pathToFollow.AddChild(enemyCharacter);
		enemyCharacter.AddToGroup("enemys");
		enemyCharacter.UpdateHealthBar();
		if (totalEnemies == firstWave + 10)
		{
            ongoingWave = false;
			timer.Stop();
            wavePause.Start();
        }
		totalEnemies++;
	}

	public void OnWavePauseTimeout()
	{
        NewWave();
		wavePause.Stop();
    }
}
