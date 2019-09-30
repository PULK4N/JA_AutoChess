using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Prototype.NetworkLobby
{
    // Subclass this and redefine the function you want
    // then add it to the lobby prefab
    public class LobbyHook : MonoBehaviour
    {


        public  void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {

            GameObject gameObject = new GameObject();
            LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
            string name = lobby.playerName;

            PlayerNames.Instance.AddToPlayerNameString(name);

        }
    }

}

