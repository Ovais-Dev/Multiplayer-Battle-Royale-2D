using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RemoteConnection : MonoBehaviour
{
    [SerializeField] private int maxConnection = 2;
    public TMP_InputField inputField;
    public GameObject canvas;

    public async Task Authenticate(string customId)
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            //await AuthenticationService.Instance.SignInWithCustomIdAsync(
            //    customId,
            //    new SignInOptions { CreateAccount = true }
            //);
        }

        Debug.Log("Authenticated PlayerID: " + AuthenticationService.Instance.PlayerId);
    }
    
    public async Task<string> CreateRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        transport.SetRelayServerData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData
        );

        NetworkManager.Singleton.StartHost();

        return joinCode;
    }
    public async Task JoinRelay(string joinCode)
    {
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        transport.SetRelayServerData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData,
            allocation.HostConnectionData
        );

        NetworkManager.Singleton.StartClient();
    }

    public async void StartHost()
    {
        await Authenticate("HostPlayer");
        string code = await CreateRelay();
        Debug.Log("Join Code: " + code);
        canvas.SetActive(false);
    }
    public async void JoinClient()
    {
        await Authenticate("ClientPlayer");
        string code = inputField.text;
        await JoinRelay(code);
        canvas.SetActive(false);
    }
}
