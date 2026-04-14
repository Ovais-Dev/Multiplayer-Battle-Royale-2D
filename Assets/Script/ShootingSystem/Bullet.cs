using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{

    public NetworkVariable<ulong> ShooterClientId = new NetworkVariable<ulong>();

    [Header("Settings")]
    public float lifeTime = 3f; // Auto-despawn after this time
    public int damage = 10;

    [Header("Color Setup")]
    public SpriteRenderer spriteRenderer; 
    public Color playerColor;
    public Color enemyColor;

    [Header("Effects")]
    public GameObject hitEffectPrefab; // Optional visual effect

    public void InitializeBullet(ulong ownerClientId, int dmg)
    {
        ShooterClientId.Value = ownerClientId;
        damage = dmg;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {

            // Server schedules despawning after lifetime expires
            Invoke(nameof(DespawnBullet), lifeTime);
        }
       
            if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (NetworkManager.Singleton.LocalClientId == ShooterClientId.Value)
            {
                spriteRenderer.color = playerColor;
            }
            else
            {
                spriteRenderer.color = enemyColor;
            }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only the server handles collision logic to avoid conflicts
        if (!IsServer) return;

        NetworkObject obj = other.gameObject.GetComponent<NetworkObject>();
        if (obj == null) { DespawnBullet();return; }
        if (obj.tag != "Player") { DespawnBullet(); return; }
        if (obj!=null && obj.OwnerClientId == ShooterClientId.Value) return;
       

        var health = other.GetComponent<CharacterHealth>();
        if(health != null)
        {
            health.ApplyDamage(damage,ShooterClientId.Value);
        }

        // Spawn a hit effect that syncs across clients
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            effect.GetComponent<NetworkObject>().Spawn();
        }

        // Despawn the bullet on the server (happens on all clients)
        DespawnBullet();
    }

    private void DespawnBullet()
    {
        if (IsServer)
        {
            // This safely removes the object from all clients
            GetComponent<NetworkObject>().Despawn();
            // Optional: Destroy(gameObject) if not using pooling
        }
    }

    
}