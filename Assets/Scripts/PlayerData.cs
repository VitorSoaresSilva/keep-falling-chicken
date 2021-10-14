using System;

[Serializable]
public class PlayerData
{
    public int gold;
    public int[] powerUpLevels;
    public PlayerData()
    {
        gold = 0;
        powerUpLevels = new int[(int)PowerUpTypes.COUNT];
    }
    public PlayerData(int gold, int[] powerUpLevels)
    {
        this.gold = gold;
        this.powerUpLevels = powerUpLevels;
    }
}
