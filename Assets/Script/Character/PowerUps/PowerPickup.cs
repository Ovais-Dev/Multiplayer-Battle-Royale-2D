using UnityEngine;
using Unity.Netcode;
public class PowerPickup : NetworkBehaviour
{
    //this is a network object
    public PowerupData powerUpData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;
        var player = collision.GetComponent<PlayerPowerupController>();

        if (player != null)
        {
            player.AddData(powerUpData);
            NetworkObject.Despawn();
        }
    }
}
