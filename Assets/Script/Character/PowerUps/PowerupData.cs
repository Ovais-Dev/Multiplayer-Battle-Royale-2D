using UnityEngine;

public abstract class PowerupData : ScriptableObject
{
    public float duration;
    public abstract IPowerup CreateInstance();
}

[CreateAssetMenu(menuName = "Powerups/Shield")]
public class ShieldData: PowerupData
{
    public override IPowerup CreateInstance()
    {
        return new ShieldPowerUp(duration);
    }
}
[CreateAssetMenu(menuName = "Powerups/FirePower")]
public class FirePowerData : PowerupData
{
    public override IPowerup CreateInstance()
    {
        return new FirePowerUp(duration);
    }
}
