using Godot;

public partial class Progressbars : CanvasLayer
{
	private TextureProgressBar healthBar;
	private Label timerLabel;
	private Timer timer;
	private Label WaveLabel;

	private Label currentMoney;
	public int seconds, minutes, hours;

	CustomSignals _customSignals;
	private int totalValue;
	private Path2D path2D;
	private Node2D towerSpawner;
	private Timer gameTimer;
	
	public override void _Ready()
	{
		gameTimer = GetTree().Root.GetNode<Timer>("/root/root/GameTimer");
		path2D = GetTree().Root.GetNode<Path2D>("/root/root/Path2D");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.NewWave += HandleNewWave;
		healthBar = GetNode<TextureProgressBar>("HealthBar");
		WaveLabel = GetNode<Label>("WaveNum");
		currentMoney = GetNode<Label>("Money");
		timerLabel = GetNode<Label>("Time");
		timer = GetNode<Timer>("Timer");
	}

	public override void _PhysicsProcess(double delta)
	{
		healthBar.Value = gameManager.GlobalValues.playerHealth;
		currentMoney.Text = "Money: " + gameManager.GlobalValues.playerCurrency;
	}

	private void HandleNewWave(int NewWave) {
		WaveLabel.Text = "Wave: " + NewWave;
		GD.Print("Wave: " + NewWave);
	}
	private void OnTimerTimeout(){
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

	// private void OnNormalSpeedPressed(){
	// 	if (gameManager.GlobalValues.gameSpeed != 1){
	// 		gameManager.GlobalValues.gameSpeed = 1;
	// 		UpdateSpeed(1, 1);
	// 	}
	// }

	// private void _on_x_pressed(){
	// 	GD.Print("1.5x");
	// 	if (gameManager.GlobalValues.gameSpeed != 2){
	// 		gameManager.GlobalValues.gameSpeed = 2;
	// 		UpdateSpeed(2, 0.5);
    //     }
    // }

    // private void On2xPressed(){
	// 	if (gameManager.GlobalValues.gameSpeed != 3){
	// 		gameManager.GlobalValues.gameSpeed = 3;
	// 		UpdateSpeed(4, 0.25);
    //     }
	// }


    

	// private void UpdateSpeed(int speedChange, double speedMultiplier){
    //     if (path2D.GetChildren().Count > 0){
    //         foreach (Node child in path2D.GetChildren()){
    //             Enemyscript enemy = (Enemyscript)child.GetChild(0);
	// 			GD.Print(enemy);
    //             enemy.enemySpeed = speedChange;
    //             timer.WaitTime = speedMultiplier;
    //         }
    //     } else {
	// 		return;
	// 	}
	// 	if (towerSpawner.GetChildren().Count > 0){
	// 		foreach (StaticBody2D tower in towerSpawner.GetChildren()){
    //         var timer = tower.GetNode<Timer>("AttackTimer");
    //         timer.WaitTime = speedMultiplier;
    //     }} else {
	// 		return;
	// 	}
    //     _customSignals.EmitSignal(nameof(CustomSignals.ChangeSpeed), speedChange);
    // }
}
