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
    public GameObject joinedMenu;
    public GameObject joinMenu;

    public TMP_InputField roomNameInput;
    public TMP_InputField roomBetAmountInput;
    private string map = "Spiky Sallon";

    public TMP_Text errorTextCreateRoom;
    public TMP_Text gameRoomName_joinedMenu;

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
        ShowMenu(mainMenu);
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

        ShowMenu(loadingMenu);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room with " + PhotonNetwork.CurrentRoom.PlayerCount + " players" + "room name : " + PhotonNetwork.CurrentRoom.Name);
        gameRoomName_joinedMenu.text = PhotonNetwork.CurrentRoom.Name;
        ShowMenu(joinedMenu);
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


    private void ShowMenu(GameObject menuToShow)
    {
        loadingMenu.SetActive(menuToShow == loadingMenu);
        mainMenu.SetActive(menuToShow == mainMenu);
        createMenu.SetActive(menuToShow == createMenu);
        inventoryMenu.SetActive(menuToShow == inventoryMenu);
        joinedMenu.SetActive(menuToShow == joinedMenu);
        joinMenu.SetActive(menuToShow == joinMenu);
    }

    void Update()
    {

    }












    ///button clicks
    public void QuitGame()
    {
        Application.Quit();
    }

    public void openInventory()
    {
        ShowMenu(inventoryMenu);
    }

    public void ClickCreateRoom()
    {
        ShowMenu(createMenu);
    }
    public void backToMainMenu()
    {
        ShowMenu(mainMenu);
    }
    public void ClickJoinRoom()
    {
        ShowMenu(joinMenu);
    }


}
