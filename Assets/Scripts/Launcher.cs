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
    private string map = "Spiky Sallon";



    // Start is called before the first frame update
    void Start()
    {
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
        PhotonNetwork.CreateRoom(roomNameInput.text, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });

        mainMenu.SetActive(false);
        loadingMenu.SetActive(true);
        createMenu.SetActive(false);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room with " + PhotonNetwork.CurrentRoom.PlayerCount + " players" + "room name : " + PhotonNetwork.CurrentRoom.Name);
    }


    public void handleDropDownUpdateMapSelect(int val)
    {
        Debug.Log("Map Selected: " + val);
        if (val == 0) map = "Spiky Sallon";
        if (val == 1) map = "1";
        if (val == 2) map = "2";

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed: " + message);
    }


    void Update()
    {

    }
}
