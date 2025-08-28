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
        private MenuUIManagement menuUIManagement;
        [SerializeField] private string gameScene;

        [Header("Lobby Panel Buttons")]
        [SerializeField] private Button createLobbyButton;
        [SerializeField] private Button joinLobbyButton;


        [Header("Lobby List")]
        [SerializeField] private Transform lobbyListContainer;
        [SerializeField] private GameObject lobbyItemPrefab;
        [SerializeField] private Button refreshButton;
        [SerializeField] private Button backButton;

        [Header("Heartbeat and Poll")]
        [SerializeField] private float lobbyHeartbeat = 15f;
        [SerializeField] private float lobbyPollInterval = 2f;
        private Lobby currentLobby;
        private float heartbeatTimer;//keep track of when to send heartbeat
        private float pollTimer; // keep track of when to poll lobby changes

        

        public static LobbyUI Instance { get; private set; }

        private void Awake()
        {
            if(backButton == null)
            {
                backButton = GameObject.FindGameObjectWithTag("BackButton").GetComponent<Button>();
            }
            if (refreshButton == null)
            {
                refreshButton = GameObject.FindGameObjectWithTag("RefreshButton").GetComponent<Button>();
            }
            //if (joinLobbyButton == null)
            //{   
            //    joinLobbyButton = GameObject.FindGameObjectWithTag("JoinButton").GetComponent<Button>();
            //}
            //if (createLobbyButton == null)
            //{   
            //    createLobbyButton = GameObject.FindGameObjectWithTag("CreateButton").GetComponent<Button>();
            //}

            
            Debug.Log($"Refresh button is {(refreshButton == null ? "NULL" : "SET")}");
            Debug.Log($"Back button is {(backButton == null ? "NULL" : "SET")}");

            Debug.Log("RefreshButton activeSelf: " + refreshButton.gameObject.activeSelf);
            Debug.Log("BackButton activeSelf: " + backButton.gameObject.activeSelf);


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

            
        }

        public void SetCurrentLobby(Lobby lobby)
        {
            currentLobby = lobby;
        }

        public async void CreateGame()
        {
            Debug.Log("Create lobby button pressed");
            await Multiplayer.Instance.CreateLobby();
            //GameObject.Find("PanelsManager").GetComponent<MenuUIManagement>().ShowCharacterSelection();
            GameObject.Find("PanelsManager").GetComponent<MenuUIManagement>().ShowConnecting();
            
            //Loader.LoadNetwork(gameScene);
        }


        //Choose game from lobby list 
        public async void JoinGame()
        {
            Debug.Log("Join lobby button pressed");
            GameObject.Find("PanelsManager").GetComponent<MenuUIManagement>().ShowLobbies();
            
            LobbyUI.Instance.RefreshLobbyList();

        }


        public async void Back()
        {
            Debug.Log("Back button pressed");
            GameObject.Find("PanelsManager").GetComponent<MenuUIManagement>().ShowMenu();
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

            Debug.Log("Refresh button is pressed");
            var lobbies = await GetLobbiesAsync();

            //clear old list
            foreach(Transform child in lobbyListContainer)
                Destroy(child.gameObject);

            //add each lobby 
            foreach (var lobby in lobbies)
            {
                var item = Instantiate(lobbyItemPrefab, lobbyListContainer);

                //reset position and scale
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;

                var ui = item.GetComponent<LobbyItemUI>();

                ui.lobbyName.text = lobby.Id;

                ui.playerCounter.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";

                ui.joinButton.onClick.AddListener(() => JoinLobbyById(lobby.Id));
            }
            
        }


        public async void JoinLobbyById(string lobbyId)
        {
            try
            {
                var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
                Debug.Log("got the lobby by id");
                string relayJoinCode = lobby.Data["RelayJoinCode"].Value;
                Debug.Log("got the relay code for that specific lobby");
                var joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

                Debug.Log("Client trying to join with relay code: " + relayJoinCode);

                NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>()
                    .SetRelayServerData(joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData);

                NetworkManager.Singleton.StartClient();

                Debug.Log($"Successfully joined lobby with {relayJoinCode} relay code");
                
                //GameObject.Find("PanelsManager").GetComponent<MenuUIManagement>().ShowCharacterSelection();
                GameObject.Find("PanelsManager").GetComponent<MenuUIManagement>().ShowConnecting();
                
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
