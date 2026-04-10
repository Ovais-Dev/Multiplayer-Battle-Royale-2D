using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Collections;
public class CharacterUIBinder : NetworkBehaviour
{
    public TMP_Text nameText;
    private NetworkVariable<FixedString32Bytes> playerName =
       new NetworkVariable<FixedString32Bytes>(
           default,
           NetworkVariableReadPermission.Everyone,
           NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            SetNameServerRpc(PlayerData.PlayerName);
        }
        playerName.OnValueChanged += OnNameChanged;
            OnNameChanged(default, playerName.Value);
    }
    void OnNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
    {
        nameText.text = newName.ToString();
    }
    [ServerRpc]
    void SetNameServerRpc(string name)
    {
        playerName.Value = name;
    }

    //public void BindHealth(CharacterHealth healthStats)
    //{
    //    healthStats.health.OnValueChanged += UpdateHealthUI;
    //    UpdateHealthUI(0, healthStats.health.Value);
    //}
    //void UpdateHealthUI(int oldValue, int newValue)
    //{
    //    UIManager.Instance.SetHealth(newValue);
    //}
    public void BindKill(CharacterStats killStats)
    {
        killStats.KillCount.OnValueChanged += UpdateKillUI;
    }
    void UpdateKillUI(int oldValue, int newValue)
    {
        UIManager.Instance.SetKill(newValue);
    }
    public void SetName(string nameStr)
    {
        nameText.text = nameStr;
    }
}
