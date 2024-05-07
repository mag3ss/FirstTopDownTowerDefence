using Godot;
using System.Collections.Generic;

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

    private bool ongoingWave;
    private int WaveNum = 1;
    private float difficulty = 1;
    private int totalEnemies = 10;
    private int newEnemySpeed = 1;
    private CustomSignals _customSignals;

    private Dictionary<PackedScene, float> monsterSpawnRates = new Dictionary<PackedScene, float>();

    public override void _Ready()
    {
        monsterSpawnRates.Add(SmallGoblin, 1.9f);
        monsterSpawnRates.Add(SmallGoblinE, 0.8f);
        _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        timer = GetParent().GetNode<Timer>("GameTimer");
        wavePause = GetParent().GetNode<Timer>("WavePause");
        _customSignals.ChangeSpeed += ChangedEnemySpeed;
        timer.Start();
    }

    private void NewWave()
    {
        WaveNum++;
        totalEnemies += 10;
        gameManager.GlobalValues.aliveEnemies = totalEnemies; // Not sure about this line
        timer.Start();
        ongoingWave = true;
        difficulty *= 1.125f;
        GD.Print("Wave " + WaveNum);
    }

    private void _on_game_timer_timeout()
    {
        AdjustSpawnRates();
        SpawnMonsters();
        if (gameManager.GlobalValues.aliveEnemies <= 0){
            ongoingWave = false;
            timer.Stop();
            wavePause.Start();
        }
    }

    private void ChangedEnemySpeed(int speedChange)
    {
        newEnemySpeed = speedChange;
    }

    public void OnWavePauseTimeout()
    {
        NewWave();
        wavePause.Stop();
    }

    private void AdjustSpawnRates()
    {
        foreach (var monsterType in monsterSpawnRates.Keys)
        {
            monsterSpawnRates[monsterType] += 0.25f;
            monsterSpawnRates[monsterType] = Mathf.Min(0.9f, monsterSpawnRates[monsterType]);
        }
    }

    private void SpawnMonsters()
    {
        foreach (var monsterType in monsterSpawnRates.Keys)
        {
            float spawnRate = monsterSpawnRates[monsterType];
            int baseMonsterCount = (int)(totalEnemies * spawnRate);
            int monsterCount = baseMonsterCount + (int)(baseMonsterCount * 0.5 * (1 - spawnRate));
            for (int i = 0; i < monsterCount; i++)
            {
				pathToFollow = new PathFollow2D(); // Initialize pathToFollow
        		AddChild(pathToFollow); // Add pathToFollow as a child of the GenerateEnemies node
                var monster = (Enemyscript)monsterType.Instantiate();
                monster.enemyDamage *= 1 + (int)spawnRate;
                pathToFollow.AddChild(monster);
            }
        }
    }
}
