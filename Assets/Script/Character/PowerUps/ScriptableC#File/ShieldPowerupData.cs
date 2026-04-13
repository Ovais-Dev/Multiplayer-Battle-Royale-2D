using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Powerups/Shield")]
public class ShieldPowerupData : PowerupData
{
    public override IPowerup CreateInstance()
    {
        return new ShieldPowerUp(duration);
    }
}
