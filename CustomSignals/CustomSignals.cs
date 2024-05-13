using Godot;
using System;


public partial class CustomSignals : Node
{
    [Signal] public delegate void EnemyDamageEventHandler(int damageDealt, string Id);

    [Signal] public delegate void EnemyKillEventHandler(int IDnumber);

    [Signal] public delegate void ChangeSpeedEventHandler(int speedChange);

    [Signal] public delegate void NewWaveEventHandler(int waveNumber);

    


}
