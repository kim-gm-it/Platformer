using Unity.Netcode;
using UnityEngine;

public class PlayerAssignment : NetworkBehaviour
{
    public GameObject hostPlayer;
    public GameObject clientPlayer;

    public override void OnNetworkSpawn()
    {
        if(IsServer)// host runs this 
        {
            //give the host ownership of the host player
            NetworkObject hostNetObj = hostPlayer.GetComponent<NetworkObject>();
            hostNetObj.ChangeOwnership(NetworkManager.Singleton.LocalClientId);

            //this event tells us when a client joins 
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        }
    }

    private void OnClientConnected(ulong clientId)
    {
        //when a client connects give them ownership of the client player
        NetworkObject clientNetObj = clientPlayer.GetComponent<NetworkObject>();
        clientNetObj.ChangeOwnership(clientId);
    }
}
