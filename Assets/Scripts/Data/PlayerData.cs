using System;

[Serializable]
public class PlayerData
{
    public int level;
    public int gold;
    public int diamond;
    public int highScore;
    public int[] powerUpsLevels;
    public PlayerData()
    {
        level = 0;
        gold = 0;
        diamond = 0;
        highScore = 0;
        powerUpsLevels = new int [4];
    }
}
