using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class GenerateEnemies : Path2D
{
    //Enemies
    //-----------Goblins-----------
    [Export] PackedScene SmallGoblin;
    [Export] PackedScene SmallGoblinEquipped;
    [Export] PackedScene MediumGoblin;
    [Export] PackedScene BigGoblin;
    //-----------Cyclops-----------
    [Export] PackedScene Cyclopse;
    //-----------Imps-----------
    [Export] PackedScene Imp;

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
    Random rand = new Random();

    public override void _Ready()
    {
        monsterCollection.Add(SmallGoblin);
        monsterCollection.Add(SmallGoblinEquipped);
        monsterCollection.Add(MediumGoblin);
        monsterCollection.Add(BigGoblin);
        monsterCollection.Add(Cyclopse);
        monsterCollection.Add(Imp);
        _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        timer = GetParent().GetNode<Timer>("GameTimer");
        wavePause = GetParent().GetNode<Timer>("WavePause");
        _customSignals.ChangeSpeed += ChangedEnemySpeed;
        StartRound();
    }

    public void StartRound() {
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

    public void _on_game_timer_timeout()
    {
        Random rand = new Random();
        int enemyIndex = rand.Next(1, 75);
        switch (enemyIndex)
        {
            case int n when n < 25:
                SpawnMonsters(0);
                break;
            case int n when n < 40:
                SpawnMonsters(1); 
                break;
            case int n when n < 55:
                SpawnMonsters(2);
                break;
            case int n when n < 65:
                SpawnMonsters(3);
                break;
            case int n when n < 70:
                SpawnMonsters(4);
                break;
            case int n when n < 75:
                SpawnMonsters(5);
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
        if (monsterCollection[index].CanInstantiate()) {
            var monster = monsterCollection[index].Instantiate();
            monster.AddToGroup("enemys");
            pathToFollow = new PathFollow2D();
            pathToFollow.Loop = false;
            AddChild(pathToFollow);
            pathToFollow.AddChild(monster);
        } else {
            GD.Print("Can't instantiate");
        }
        
    }
}
