using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Thirdweb;


public class Launcher : MonoBehaviourPunCallbacks
{

    //menus
    public GameObject loadingMenu;
    public GameObject mainMenu;
    public GameObject createMenu;
    public GameObject inventoryMenu;


    public TMP_InputField roomNameInput;
    public TMP_InputField roomBetAmountInput;
    private string map = "Spiky Sallon";


    public TMP_Text errorTextCreateRoom;

    private ThirdwebSDK sdk;


    // Start is called before the first frame update
    void Start()
    {

        //connect to thirdweb
        sdk = ThirdwebManager.Instance.SDK;



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
        inventoryMenu.SetActive(false);
    }

    public void ClickCreateRoom()
    {
        mainMenu.SetActive(false);
        loadingMenu.SetActive(false);
        createMenu.SetActive(true);
        inventoryMenu.SetActive(false);
    }


    public async void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text) || string.IsNullOrEmpty(roomBetAmountInput.text))
        {
            errorTextCreateRoom.text = "Room Name and Bet Amount cannot be empty";
            return;
        }


        //wallet
        var data = await sdk.Wallet.IsConnected();
        if (data == false)
        {
            errorTextCreateRoom.text = "Please connect your wallet";
            return;
        }


        PhotonNetwork.CreateRoom(roomNameInput.text, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });

        mainMenu.SetActive(false);
        loadingMenu.SetActive(true);
        createMenu.SetActive(false);
        inventoryMenu.SetActive(false);
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

    public void QuitGame()
    {
        Application.Quit();
    }
    public void openInventory()
    {
        mainMenu.SetActive(false);
        loadingMenu.SetActive(false);
        createMenu.SetActive(false);
        inventoryMenu.SetActive(true);
    }

    void Update()
    {

    }




}
