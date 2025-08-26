using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using System;
using Unity.Services.Authentication;


namespace Kart
{
    public class LobbyUI : MonoBehaviour
    {
        [Header("Panel Management")]
        [SerializeField] private MenuUIManagement menuUIManagement;
        [SerializeField] private string gameScene;

        [Header("Lobby Panel Buttons")]
        [SerializeField] private Button createLobbyButton;
        [SerializeField] private Button joinLobbyButton;


        [Header("Lobby List")]
        [SerializeField] private Transform lobbyListContainer;
        [SerializeField] private GameObject lobbyItemPrefab;

        [Header("Heartbeat and Poll")]
        [SerializeField] private float lobbyHeartbeat = 15f;
        [SerializeField] private float lobbyPollInterval = 2f;
        private Lobby currentLobby;
        private float heartbeatTimer;//keep track of when to send heartbeat
        private float pollTimer; // keep track of when to poll lobby changes

        

        public static LobbyUI Instance { get; private set; }

        private void Awake()
        {

            Debug.Log($"Create button is {(createLobbyButton == null ? "NULL" : "SET")}");
            Debug.Log($"Join button is {(joinLobbyButton == null ? "NULL" : "SET")}");

            Debug.Log("CreateButton activeSelf: " + createLobbyButton.gameObject.activeSelf);
            Debug.Log("JoinButton activeSelf: " + joinLobbyButton.gameObject.activeSelf);


            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            //createLobbyButton.onClick.AddListener(CreateGame);
            //joinLobbyButton.onClick.AddListener(JoinGame);
        }

        public void SetCurrentLobby(Lobby lobby)
        {
            currentLobby = lobby;
        }

        public async void CreateGame()
        {
            Debug.Log("Create lobby button pressed");
            await Multiplayer.Instance.CreateLobby();
            menuUIManagement.ShowCharacterSelection();
            //Loader.LoadNetwork(gameScene);
        }


        //Choose game from lobby list 
        public async void JoinGame()
        {
            Debug.Log("Join lobby button pressed");
            GameObject.Find("PanelsManager").GetComponent<MenuUIManagement>().ShowLobbies();
            menuUIManagement.ShowLobbies();
            LobbyUI.Instance.RefreshLobbyList();

        }

        private void HandleLobbyHeartbeat()
        {
            if (currentLobby != null && IsLobbyHost())
            {
                heartbeatTimer -= Time.deltaTime;
                if (heartbeatTimer <= 0)
                {
                    heartbeatTimer = lobbyHeartbeat;
                    LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);//send a ping to keep the lobby alive every second 
                }

            }

        }

        private void Update()
        {
            HandleLobbyHeartbeat();
            HandleLobbyPoll();
        }

        public void HandleLobbyPoll()
        {
            if (currentLobby != null)
            {
                pollTimer -= Time.deltaTime;
                if (pollTimer <= 0)
                {
                    pollTimer = lobbyPollInterval;
                    LobbyService.Instance.GetLobbyAsync(currentLobby.Id);//refresh lobby data
                }

            }

        }

        private bool IsLobbyHost()
        {
            return currentLobby != null && currentLobby.HostId == AuthenticationService.Instance.PlayerId;
        }

        public static class Loader
        {
            public static void LoadNetwork(string scene)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
        }

        public async void RefreshLobbyList()
        {
            var lobbies = await GetLobbiesAsync();

            //clear old list
            foreach(Transform child in lobbyListContainer)
                Destroy(child.gameObject);

            //add each lobby 
            foreach (var lobby in lobbies)
            {
                var item = Instantiate(lobbyItemPrefab, lobbyListContainer);
                var ui = item.GetComponent<LobbyItemUI>();

                ui.lobbyName.text = lobby.Name;

                ui.playerCounter.text = $"{lobby.Name}({lobby.Players.Count}/{lobby.MaxPlayers})";

                ui.joinButton.onClick.AddListener(() => JoinLobbyById(lobby.Id));
            }
            
        }


        public async void JoinLobbyById(string lobbyId)
        {
            try
            {
                var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
                string relayJoinCode = lobby.Data["RelayJoinCode"].Value;

                var joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);
                NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>()
                    .SetRelayServerData(joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData);

                NetworkManager.Singleton.StartClient();
                menuUIManagement.ShowCharacterSelection();
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to join: {e}");
            }
        }


        
        //get a list of all lobbies 
        public async Task<List<Lobby>> GetLobbiesAsync()
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions
            {
                Count = 10, // returns 10 lobbies and less

                //filter: only lobbies that have at least 1 free slot
                Filters = new List<QueryFilter>//narrow down which lobbies we want
                {
                    new QueryFilter(
                        field: QueryFilter.FieldOptions.AvailableSlots, // how many open player slots each lobby has
                        op: QueryFilter.OpOptions.GT, // greater than
                        value: "0")//compare to 0

                }

            };

            var response = await LobbyService.Instance.QueryLobbiesAsync(options);//call unity lobby service
            
            return response.Results;

        }

    }

    
    }
