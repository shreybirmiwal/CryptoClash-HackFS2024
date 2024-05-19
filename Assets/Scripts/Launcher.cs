using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;


public class Launcher : MonoBehaviourPunCallbacks
{

    //menus
    public GameObject loadingMenu;
    public GameObject mainMenu;
    public GameObject createMenu;


    public TMP_InputField roomNameInput;
    public TMP_InputField roomBetAmountInput;



    // Start is called before the first frame update
    void Start()
    {

        loadingMenu.SetActive(true);
        mainMenu.SetActive(false);
        createMenu.SetActive(false);

        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");

        loadingMenu.SetActive(false);
        mainMenu.SetActive(true);
        createMenu.SetActive(false);
    }

    public void ClickCreateRoom()
    {
        mainMenu.SetActive(false);
        loadingMenu.SetActive(false);
        createMenu.SetActive(true);
    }


    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text) || string.IsNullOrEmpty(roomBetAmountInput.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });

        mainMenu.SetActive(false);
        loadingMenu.SetActive(true);
        createMenu.SetActive(false);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed: " + message);
    }


    void Update()
    {

    }
}
