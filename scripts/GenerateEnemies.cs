using Godot;
using System;

public partial class GenerateEnemies : Path2D
{

    PackedScene packedScene = ResourceLoader.Load("res://entities/enemy.tscn") as PackedScene;

    private bool yesking = false;
    private PathFollow2D oldPath; 
    public override void _Ready()
	{
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{  
	}


    public void _on_game_timer_timeout()
    {
        PathFollow2D pathToFollow = new PathFollow2D();
        AddChild(pathToFollow);
        pathToFollow.Loop = false;
        CharacterBody2D enemyCharacter = packedScene.Instantiate() as CharacterBody2D;
        pathToFollow.AddChild(enemyCharacter);
        enemyCharacter.AddToGroup("enemys");

    }
}
