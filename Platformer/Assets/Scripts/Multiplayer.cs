using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;// initializing services
using Unity.Netcode;// hosting / clients
using Unity.Services.Authentication;//playFab and Unity authentication
using Unity.Services.Lobbies; // lobby system
using Unity.Services.Relay;// relay allocations
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using UnityEngine;
using Kart;

public class Multiplayer : MonoBehaviour
{
    [SerializeField] private string lobbyName = "Default Lobby";
    [SerializeField] private int maxPlayers = 2;
    [SerializeField] private float lobbyHeartbeat = 15f;
    [SerializeField] private float lobbyPollInterval = 2f;

    private Lobby currentLobby;
    private float heartbeatTimer;//keep track of when to send heartbeat
    private float pollTimer; // keep track of when to poll lobby changes

    public static Multiplayer Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();//assigns random playerID

        Debug.Log("Signed in as Player " + AuthenticationService.Instance.PlayerId);
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPoll();
    }

    public async Task CreateLobby()
    {
        Debug.Log("Creating a lobby");
        try
        {
            //relay allocation so others can connect
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1);//allocation for other players minus the host 

            //get join code from relay
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);//join code for other players to connect to you

            Debug.Log("Relay Join Code: " + relayJoinCode);

            //storing relay join code in the labby data so other players can find it when they join
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = false,
                Data = new Dictionary<string, DataObject>
                {
                    {"RelayJoinCode"  , new DataObject(DataObject.VisibilityOptions.Public, relayJoinCode) }
                }
            };

            currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName , maxPlayers, options);

            LobbyUI ui = FindObjectOfType<LobbyUI>();
            if (ui != null) 
                ui.SetCurrentLobby(currentLobby);

            Debug.Log("Lobby Created: " +  currentLobby.Id);
            
            //telling network transport to use unity relay 
            NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetRelayServerData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port , allocation.AllocationIdBytes , allocation.Key, allocation.ConnectionData , default , true);

            NetworkManager.Singleton.StartHost();


        }
        catch(Exception e)
        {
            Debug.Log("Failed to create lobby! " + e);
        }
    }

    //public async Task QuickJoinLobby()
    //{
    //    try
    //    {
    //        //try to join any available lobby
    //        Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();

    //        LobbyUI ui = FindObjectOfType<LobbyUI>();
    //        if (ui != null)
    //        {
    //            ui.SetCurrentLobby(lobby);
    //            currentLobby = lobby;
    //        }

    //        //get relay join code from that lobby data
    //        string relayJoinCode = lobby.Data["RelayJoinCode"].Value;

    //        //join relay using that code
    //        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

    //        // start a client (joining a relay-hosted game)
    //        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetRelayServerData(joinAllocation.RelayServer.IpV4 ,(ushort)joinAllocation.RelayServer.Port , joinAllocation.Key , joinAllocation.ConnectionData , joinAllocation.HostConnectionData);
            
    //        NetworkManager.Singleton.StartClient();
    //    }
    //    catch (Exception e) 
    //    {
    //        Debug.Log("Failed to join lobby: " + e);
    //    }
    //}



    private async Task HandleLobbyHeartbeat()
    {
        if (currentLobby != null && IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer <= 0)
            {
                heartbeatTimer = lobbyHeartbeat;
                await LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);//send a ping to keep the lobby alive every second 

                Debug.Log("Sent heartbeat to keep the lobby alive");
            }

        }

    }

    public async Task HandleLobbyPoll()
    {
        if (currentLobby != null)
        {
            pollTimer -= Time.deltaTime;
            if (pollTimer <= 0)
            {
                pollTimer = lobbyPollInterval;
                currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);//refresh lobby data
                LobbyUI lobbyUi = FindAnyObjectByType<LobbyUI>();
                if(lobbyUi != null)
                {
                    lobbyUi.SetCurrentLobby(currentLobby);
                }

                Debug.Log("Lobby refreshed");
            }

        }

    }


    private bool IsLobbyHost()
    {
        return currentLobby != null && currentLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

}
