using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
public class PlayerPowerupController : NetworkBehaviour
{
    private List<IPowerup> activePowerups = new List<IPowerup>();
    private void Update()
    {
        for (int i = 0; i < activePowerups.Count; i++)
        {
            var p = activePowerups[i];
            p.Tick(Time.deltaTime);
            if (p.IsFinished)
            {
                p.Deactivate(gameObject);
                activePowerups.RemoveAt(i);
            }
        }
    }
    public void AddData(PowerupData pData)
    {
        var instance = pData.CreateInstance();

        instance.Activate(gameObject);
        activePowerups.Add(instance);

       
        NotifyClientsPowerupStartedClientRpc(pData.id);
    }

    [ClientRpc]
    void NotifyClientsPowerupStartedClientRpc(int id)
    {
        if (IsServer) return; // server side already knows
        var data = PowerupDatabaseHolder.Instance.Get(id);

        // why notifying all clients  if hit and damage controlled by server
        var instance = data.CreateInstance();
        instance.Activate(gameObject); // VISUAL + LOCAL EFFECT
        activePowerups.Add(instance);
        if (IsOwner)
        {
            PowerupUIHandler.Instance.IntializePowerupUIElement(data);
        }
    }
}
