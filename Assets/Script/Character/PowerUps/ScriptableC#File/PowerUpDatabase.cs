using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Scriptable Objects/Powerups/Database")]
public class PowerupDatabase : ScriptableObject
{
    public List<PowerupData> powerups;

    private Dictionary<int, PowerupData> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<int, PowerupData>();

        foreach (var p in powerups)
        {
            lookup[p.id] = p;
        }
    }

    public PowerupData Get(int id)
    {
        return lookup[id];
    }
}
