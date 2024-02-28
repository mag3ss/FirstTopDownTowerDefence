using Godot;
using System;

public partial class Progressbars : CanvasLayer
{
	private ProgressBar healthBar;

	private Label currentMoney;

	CustomSignals _customSignals;
	private int totalValue;
	public override void _Ready()
	{
        _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
        _customSignals.ChangedCurrency += HandleCurrentCurrency;
        healthBar = GetNode<ProgressBar>("HealthBar");
		currentMoney = GetNode<Label>("Money");
        currentMoney.Text = "Money: " + gameManager.GlobalValues.playerCurrency;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		healthBar.Value = gameManager.GlobalValues.playerHealth;

	}


	private void HandleCurrentCurrency()
	{
        currentMoney.Text = "Money " + gameManager.GlobalValues.playerCurrency.ToString();
	}
}
