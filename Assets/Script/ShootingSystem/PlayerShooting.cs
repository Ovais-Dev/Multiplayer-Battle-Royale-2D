using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerShooting : NetworkBehaviour
{
    [SerializeField]private GameObject bulletPrefab;
    [SerializeField]private float bulletSpeed = 10f;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float fireCooldDownTime = 0.2f;

    [Header("Slider Configuration")]
    [SerializeField] private Slider shootingSlider;
    [SerializeField] private int energyDivision = 3;

    float currentEnergyFill = 0f;
    float lastFireTime = 0f;
    float coolDownTimeCounter = 0f;
    float energyDivisionFactor;
    private void Start()
    {
        if (firePoint == null)
        {
            firePoint = transform;
        }
        lastFireTime = Time.time;
        if (shootingSlider)
        {
            shootingSlider.maxValue = 1f;
            currentEnergyFill = 1f / energyDivision;
            shootingSlider.value = currentEnergyFill;
        }
        energyDivisionFactor = 1f / energyDivision;
    }
    void Update()
    {
        if (!IsSpawned) return;
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!IsOwner) return;
            if(currentEnergyFill>=energyDivisionFactor && coolDownTimeCounter<=0f)
            {
                coolDownTimeCounter = fireCooldDownTime;
                currentEnergyFill -= energyDivisionFactor;
                Vector2 shootDir = CursorObj.Instance.GetMouseWorldPosition() - (Vector2)transform.position;
                FireServerRpc(shootDir.normalized);
            }
        }
        if (coolDownTimeCounter > 0f)
        {
            coolDownTimeCounter -= Time.deltaTime;
        }
        if (currentEnergyFill < 1f)
        {
            currentEnergyFill = Mathf.Clamp01(currentEnergyFill + energyDivisionFactor * fireRate * Time.deltaTime);
            if (shootingSlider)
            {
                shootingSlider.value = currentEnergyFill;
            }
        }

    }
    [ServerRpc]
    void FireServerRpc(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        bullet.GetComponent<Bullet>().ShooterClientId.Value = OwnerClientId;

        NetworkObject netObj = bullet.GetComponent<NetworkObject>();
        netObj.Spawn(true);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;
    }
}
