using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    public Transform[] spawnPoints;
    public GameObject playerPrefab;


    private int nextSpawnPointIndex = 0;
   // private int nextColorIndex = 0;
    private void Awake()
    {
    }
    private void OnEnable()
    {
        if (networkManager == null)
        {
            Debug.LogError("Is is null");
            return;
        }
        networkManager.ConnectionApprovalCallback += ApprovalCheck;
        networkManager.OnClientConnectedCallback += OnClientConnected;
        Debug.Log("Name: " + gameObject.name);
    }
    
    private void Start()
    {
        
    }
    private void OnDisable()
    {
        if(networkManager != null)
        {
            networkManager.OnClientConnectedCallback -= OnClientConnected;
            networkManager.ConnectionApprovalCallback -= ApprovalCheck;
        }
    }
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request,
                               NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;

        // ❗ THIS is the key line (disables auto spawn)
        response.CreatePlayerObject = false;

        response.Pending = false;
    }
    void OnClientConnected(ulong clientId)
    {
        if (!networkManager.IsServer) return;
        
            SpawnCharacter(clientId);
        
    }
    public void SpawnCharacter(ulong clientId)
    {
        Vector3 spawnPosition= spawnPoints[nextSpawnPointIndex].position;
        nextSpawnPointIndex = (nextSpawnPointIndex + 1) % spawnPoints.Length;

        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
       
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

        Debug.Log($"Spawned player for client {clientId} at {spawnPosition}");
    }
}
