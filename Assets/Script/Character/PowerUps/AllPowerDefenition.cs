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
    }
    public void Tick(float tR)
    {
        timer -= tR;
    }
    public void Deactivate(GameObject target)
    {
        var health = target.GetComponentInChildren<CharacterHealth>();

    }
}
public class FirePowerUp : IPowerup
{
    float timer;
    public bool IsFinished => timer <= 0f;
    public FirePowerUp(float duration)
    {
        timer = duration;
    }
    public void Activate(GameObject target)
    {
        var shooter = target.GetComponentInChildren<PlayerShooting>();
        //health.SetInvincible(true);
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