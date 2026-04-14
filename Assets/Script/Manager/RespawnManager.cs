using UnityEngine;
using TMPro;
using Unity.Netcode;
public class RespawnManager : MonoBehaviour
{
    private static RespawnManager _instance;
    public static RespawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<RespawnManager>();
            }
            return _instance;
        }
    }


    public GameObject respawnPanel;
    public TMP_Text respawnCounterText;

    public float MaxRespawnTime = 5f;
    float respawnTimeCounter;
    bool isRespawning = false;
    private void Start()
    {
        isRespawning = false;
        respawnPanel.SetActive(false);
        respawnTimeCounter = 0;
        UpdateText(respawnTimeCounter);
    }
    public void RespawnOpen()
    {
        isRespawning = true;
        respawnPanel.SetActive(true);
        
        respawnTimeCounter = MaxRespawnTime;
        UpdateText(respawnTimeCounter);
        if (!NetworkManager.Singleton.IsServer) return;
        GameObject player = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        player.GetComponent<CharacterHealth>().IsDead.Value = true;
    }
    public void RespawnClose()
    {
        isRespawning = false;
        respawnPanel.SetActive(false);
        respawnTimeCounter = 0f;
        if (!NetworkManager.Singleton.IsServer) return;
        GameObject player = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        player.GetComponent<CharacterHealth>().RespawnServerRpc();
    }
    private void Update()
    {
        if (!isRespawning) return;
        respawnTimeCounter -= Time.deltaTime;
        UpdateText(respawnTimeCounter);
        if (respawnTimeCounter <= 0f)
        {
            RespawnClose();
        }
    }
    void UpdateText(float time)
    {
        respawnCounterText.text = time.ToString("0");
    }

    public void RespawnPlayer(bool oldValue, bool newValue)
    {
        if (newValue) return;
        CharacterHealth cR = Unity.Netcode.NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<CharacterHealth>();
        cR.gameObject.SetActive(true);
    }
}
