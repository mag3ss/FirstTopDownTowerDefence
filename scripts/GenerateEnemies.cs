using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class GenerateEnemies : Path2D
{
    //Enemies
    //-----------Goblins-----------
    [Export] PackedScene SmallGoblin;
    private float smallGoblinRate = 1.1f;
    [Export] PackedScene SmallGoblinE;
    private float smallGoblinERate = 1.6f;
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
    private Array<PackedScene> monsterCollection = new Array<PackedScene>();

    public override void _Ready()
    {
        monsterCollection.Add(SmallGoblin);
        monsterCollection.Add(SmallGoblinE);
        _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        timer = GetParent().GetNode<Timer>("GameTimer");
        wavePause = GetParent().GetNode<Timer>("WavePause");
        _customSignals.ChangeSpeed += ChangedEnemySpeed;
        StartRound();
    }

    private void StartRound() {
        WaveNum = 0;
        totalEnemies = 10;
        gameManager.GlobalValues.aliveEnemies = totalEnemies;
        NewWave();
        timer.Start();
    }

    private void NewWave() {
        WaveNum++;
        _customSignals.EmitSignal(nameof(CustomSignals.NewWave), WaveNum);
        totalEnemies += 10;
        gameManager.GlobalValues.aliveEnemies = totalEnemies;
        timer.Start();
        ongoingWave = true;
        difficulty *= 1.125f;
    }

    private void _on_game_timer_timeout()
    {
        Random rand = new Random();
        int enemyIndex = rand.Next(0, 50);
        switch (enemyIndex)
        {
            case int n when (n <= 25 && n >= 0):
                SpawnMonsters(0);
                break;
            case int n when (n < 50 && n > 25):
                SpawnMonsters(1);
                break;
            case int n when (n < 100 && n >= 75):
                GD.Print("Spawning Nothing");
                // SpawnMonsters(2);
                break;
        }
        if (gameManager.GlobalValues.aliveEnemies <= 0){
            ongoingWave = false;
            timer.Stop();
            wavePause.Start();
        }
    }

    private void ChangedEnemySpeed(int speedChange) {
        newEnemySpeed = speedChange;
    }

    public void OnWavePauseTimeout() {
        NewWave();
        wavePause.Stop();
    }

    private void SpawnMonsters(int index)
    {
        var monster = (Enemyscript)monsterCollection[index].Instantiate();
        monster.AddToGroup("enemys");
        pathToFollow = new PathFollow2D();
        AddChild(pathToFollow);
        pathToFollow.AddChild(monster);
    }
}
