using Godot;
using System;

public partial class DayNightCycle : CanvasModulate
{
	private Timer timer;
	private AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public override void _Process(double delta)
	{
		var timePassed = timer.WaitTime - timer.TimeLeft;
        double remappedTimePassed = Mathf.Remap(timePassed, 0, timer.WaitTime, 0, 24);
        animationPlayer.Play("day_night");
		animationPlayer.Seek(remappedTimePassed);
	}
}
