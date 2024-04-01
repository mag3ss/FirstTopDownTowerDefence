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
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.ChangedCurrency += HandleCurrentCurrency;
		healthBar = GetNode<TextureProgressBar>("HealthBar");
		currentMoney = GetNode<Label>("Money");
		timerLabel = GetNode<Label>("Time");

		timer = GetNode<Timer>("Timer");

		currentMoney.Text = "Money: " + gameManager.GlobalValues.playerCurrency;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		healthBar.Value = gameManager.GlobalValues.playerHealth;

	}

	public void OnTimerTimeout()
	{
		if (seconds < 59)
		{
            seconds++;
        }
        else
		{
            seconds = 0;
            minutes++;
        }
        if (minutes == 59 && seconds == 59)
        {
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


    private void HandleCurrentCurrency()
	{
		currentMoney.Text = "Money " + gameManager.GlobalValues.playerCurrency.ToString();
	}
}
