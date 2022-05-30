using UnityEngine;
using DilmerGames.Core.Singletons;
using Unity.Netcode;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine.UI;
using BackEnd;

public class RelayManager : Singleton<RelayManager>
{
    [SerializeField]
    private Text ServerjoinCode;

    [SerializeField]
    private string environment = "production";

    [SerializeField]
    private int maxConnection = 10;

    public bool IsRelayEnabled => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;
    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();

    public async Task<RelayHostData> SetupRelay() {
        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn) { 
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        }

        Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxConnection);

        RelayHostData relayHostData = new RelayHostData()
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            IPv4Address = allocation.RelayServer.IpV4,
            ConnectionData = allocation.ConnectionData
        };

        relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);

        Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port, relayHostData.AllocationIDBytes, relayHostData.Key, relayHostData.ConnectionData);

        ServerjoinCode.text = relayHostData.JoinCode;

        Param param = new Param();
        param.Add("ServerPort", relayHostData.JoinCode);
        Backend.GameData.Update("user", "2022-05-30T13:23:21.416Z", param);

        return relayHostData;
    }

    public async Task<RelayJoinData> JoinRelay(string joincode) {
        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        }

        JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joincode);

        RelayJoinData relayJoinData = new RelayJoinData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            IPv4Address = allocation.RelayServer.IpV4,
            JoinCode = joincode
        };
        Transport.SetRelayServerData(relayJoinData.IPv4Address, relayJoinData.Port, relayJoinData.AllocationIDBytes,
           relayJoinData.Key, relayJoinData.ConnectionData, relayJoinData.HostConnectionData);

        return relayJoinData;
    }
}
