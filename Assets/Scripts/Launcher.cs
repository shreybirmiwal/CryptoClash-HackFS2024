using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{

    public GameObject loadingMenu;
    public GameObject mainMenu;

    // Start is called before the first frame update
    void Start()
    {   

        loadingMenu.SetActive(true);
        mainMenu.SetActive(false);

        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    } 


    public override void OnConnectedToMaster(){
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby(){
        Debug.Log("Joined Lobby");

        loadingMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
