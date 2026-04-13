using UnityEngine;

[CreateAssetMenu(fileName = "ShootingData", menuName = "Scriptable Objects/ShootingValues")]
public class ShootingData : ScriptableObject
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    [Space(5)]
    public int bulletDamage = 10;
    public float bulletSpeed = 10f;

    [Header("Fire Properties")]
    public float fireRate = 0.5f;
    public float fireCooldownTime = 0.2f;
    
}
