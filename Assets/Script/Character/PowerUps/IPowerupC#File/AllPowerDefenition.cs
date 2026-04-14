using UnityEngine;

public class ShieldPowerUp: IPowerup
{
    float timer;
    public bool IsFinished => timer <= 0f;
    public ShieldPowerUp(float duration)
    {
        timer = duration;
    }
    public void Activate(GameObject target)
    {
        var health = target.GetComponentInChildren<CharacterHealth>();
        health.SetInvincible(true);
        Debug.Log("Shield Is Activated");
    }
    public void Tick(float tR)
    {
        timer -= tR;
    }
    public void Deactivate(GameObject target)
    {
        var health = target.GetComponentInChildren<CharacterHealth>();
        health.SetInvincible(false);
        Debug.Log("Shield Is Deactivated");
    }
}
public class FirePowerUp : IPowerup
{
    float timer;
    public bool IsFinished => timer <= 0f;

    ShootingData normalShootingData, powerShootingData;
    public FirePowerUp(float duration)
    {
        timer = duration;
    }
    public FirePowerUp(float duration, ShootingData normalShootingData, ShootingData powerShootingData)
    {
        timer = duration;
        this.normalShootingData = normalShootingData;
        this.powerShootingData = powerShootingData;
    }
    public void Activate(GameObject target)
    {
        var shooter = target.GetComponentInChildren<PlayerShooting>();
        
    }
    public void Tick(float tR)
    {
        timer -= tR;
    }
    public void Deactivate(GameObject target)
    {
        var shooter = target.GetComponentInChildren<PlayerShooting>();

    }
}