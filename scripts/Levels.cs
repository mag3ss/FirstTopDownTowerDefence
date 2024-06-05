using System.Collections.Generic;

public enum LevelEnum
{
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Level4 = 4,
    Level5 = 5,
    Level6 = 6,
    Level7 = 7,
    Level8 = 8,
    Level9 = 9,
    Level10 = 10
}

public class LevelData
{
    public int Score { get; }
    public int Wave { get; }
    public int Time { get; }

    public LevelData(int score, int wave, int time)
    {
        Score = score;
        Wave = wave;
        Time = time;
    }
}


public static class Levels
{
    public static readonly IReadOnlyDictionary<LevelEnum, LevelData> LevelInfo = new Dictionary<LevelEnum, LevelData>
    {
        { LevelEnum.Level1, new LevelData(10, 5, 300) },
        { LevelEnum.Level2, new LevelData(20000, 20, 120) },
        { LevelEnum.Level3, new LevelData(30000, 30, 180) },
        { LevelEnum.Level4, new LevelData(40000, 40, 240) },
        { LevelEnum.Level5, new LevelData(50000, 50, 300) },
        { LevelEnum.Level6, new LevelData(60000, 60, 360) },
        { LevelEnum.Level7, new LevelData(70000, 70, 420) },
        { LevelEnum.Level8, new LevelData(80000, 80, 480) },
        { LevelEnum.Level9, new LevelData(90000, 90, 540) },
        { LevelEnum.Level10, new LevelData(100000, 100, 600) }
    };
}
