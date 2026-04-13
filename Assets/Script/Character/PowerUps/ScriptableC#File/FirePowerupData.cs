using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Powerups/FirePower")]
public class FirePowerData : PowerupData
{
    public ShootingData normalShootingData, powerShootingData;
    public override IPowerup CreateInstance()
    {
        return new FirePowerUp(duration,normalShootingData,powerShootingData);
    }
}