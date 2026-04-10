using UnityEngine;
using Unity.Netcode;
public class CharacterStats : NetworkBehaviour
{
    public NetworkVariable<int> KillCount = new NetworkVariable<int>(0);
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            GetComponent<CharacterUIBinder>().BindKill(this);
        }
    }
}
