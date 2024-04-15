using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Progressbars : CanvasLayer
{
	private TextureProgressBar healthBar;
	private Label timerLabel;
	private Timer timer;

	private Label currentMoney;
	private int seconds, minutes, hours;

	CustomSignals _customSignals;
	private int totalValue;
	private Path2D path2D;
	private Node2D towerSpawner;
	private Timer gameTimer;
	public override void _Ready()
	{
		gameTimer = GetTree().Root.GetNode<Timer>("/root/root/GameTimer");
		path2D = GetParent().GetParent().GetNode<Path2D>("Path2D");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.ChangedCurrency += HandleCurrentCurrency;
		healthBar = GetNode<TextureProgressBar>("HealthBar");
		currentMoney = GetNode<Label>("Money");
		timerLabel = GetNode<Label>("Time");
		timer = GetNode<Timer>("Timer");
		towerSpawner = GetParent().GetParent().GetNode<Node2D>("TowerSpawner");
		currentMoney.Text = "Money: " + gameManager.GlobalValues.playerCurrency;
	}

	public override void _Process(double delta)
	{
		healthBar.Value = gameManager.GlobalValues.playerHealth;

	}

	public void OnTimerTimeout(){
		if (seconds < 59){
            seconds++;
        }else{
            seconds = 0;
            minutes++;
        }if (minutes == 59 && seconds == 59){
            seconds = 0;
            minutes = 0;
            hours++;
        }
        UpdateTimer();
	}

	private void UpdateTimer()
	{
		timerLabel.Text = "Time: " + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
	}

	private void OnNormalSpeedPressed(){
		if (gameManager.GlobalValues.gameSpeed != 1){
			gameManager.GlobalValues.gameSpeed = 1;
			UpdateSpeed(1, 1);
		}
	}

    private void On2xPressed(){
		if (gameManager.GlobalValues.gameSpeed != 3){
			gameManager.GlobalValues.gameSpeed = 3;
			UpdateSpeed(4, 0.25);
        }
	}


    private void _on_x_pressed(){
		if (gameManager.GlobalValues.gameSpeed != 2){
			gameManager.GlobalValues.gameSpeed = 2;
			UpdateSpeed(2, 0.5);
        }
    }

	private void UpdateSpeed(int speedChange, double speedMultiplier){
        if (path2D.GetChildren().Count > 0){
            foreach (Node child in path2D.GetChildren()){
                Enemyscript enemy = (Enemyscript)child.GetChild(0);
                enemy.enemySpeed = speedChange;
                timer.WaitTime = speedMultiplier;
            }
        }
        foreach (StaticBody2D tower in towerSpawner.GetChildren()){
            var timer = tower.GetNode<Timer>("AttackTimer");
            timer.WaitTime = speedMultiplier;
        }
        _customSignals.EmitSignal(nameof(CustomSignals.ChangeSpeed), speedChange);
    }

    private void HandleCurrentCurrency()
	{
		currentMoney.Text = "Money " + gameManager.GlobalValues.playerCurrency.ToString();
	}
}
