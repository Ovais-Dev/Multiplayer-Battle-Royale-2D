using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
public class PlayerPoweruController : NetworkBehaviour
{
    private List<IPowerup> activePowerUps = new List<IPowerup>();
    private void Update()
    {
        for (int i = 0; i < activePowerUps.Count; i++)
        {
            var p = activePowerUps[i];
            p.Tick(Time.deltaTime);
            if (p.IsFinished)
            {
                p.Deactivate(gameObject);
                activePowerUps.RemoveAt(i);
            }
        }
    }
    public void AddData(PowerupData pData)
    {
        var instance = pData.CreateInstance();

        instance.Activate(gameObject);
        activePowerUps.Add(instance);
    }
}
