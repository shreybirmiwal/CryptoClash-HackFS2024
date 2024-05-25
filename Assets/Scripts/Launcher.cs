using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher Instance;

    void Awake()
    {
        Instance = this;
    }

    //menus
    public GameObject loadingMenu;
    public GameObject mainMenu;
    public GameObject createMenu;
    public GameObject inventoryMenu;
    public GameObject joinedMenu;
    public GameObject findGameMenu;
    public GameObject marketplaceMenu;
    public GameObject lootboxMenu;

    public TMP_InputField roomNameInput;
    public TMP_Dropdown mapDropdown;

    public TMP_Text errorTextCreateRoom;
    public TMP_Text gameRoomName_joinedMenu;



    private int mapNum = 1;


    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;






    // Start is called before the first frame update
    void Start()
    {
        //connect to thirdweb

        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        ShowMenu(mainMenu);

        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }


    public async void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            errorTextCreateRoom.text = "Room Name cannot be empty";
            return;
        }

        mapNum = mapDropdown.value + 1;

        PhotonNetwork.CreateRoom(roomNameInput.text, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });

        ShowMenu(loadingMenu);
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        ShowMenu(loadingMenu);

    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(mapNum);
    }

    public override void OnJoinedRoom()
    {

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        Debug.Log("Joined Room with " + PhotonNetwork.CurrentRoom.PlayerCount + " players" + "room name : " + PhotonNetwork.CurrentRoom.Name);
        gameRoomName_joinedMenu.text = PhotonNetwork.CurrentRoom.Name;
        ShowMenu(joinedMenu);

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }


        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player player in players)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(player);
        }

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }



    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed: " + message);
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        foreach (RoomInfo roomInfo in roomList)
        {

            if (roomInfo.RemovedFromList)
            {
                continue;
            }

            GameObject newRoomItem = Instantiate(roomListItemPrefab, roomListContent);
            newRoomItem.GetComponent<roomListitem>().SetRoomInfo(roomInfo);
        }
    }


    public override void OnPlayerEnteredRoom(Player new_player)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(new_player);
    }

    private void ShowMenu(GameObject menuToShow)
    {
        loadingMenu.SetActive(menuToShow == loadingMenu);
        mainMenu.SetActive(menuToShow == mainMenu);
        createMenu.SetActive(menuToShow == createMenu);
        inventoryMenu.SetActive(menuToShow == inventoryMenu);
        joinedMenu.SetActive(menuToShow == joinedMenu);
        findGameMenu.SetActive(menuToShow == findGameMenu);
        marketplaceMenu.SetActive(menuToShow == marketplaceMenu);
        lootboxMenu.SetActive(menuToShow == lootboxMenu);
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
    public void ClickFindGame()
    {
        ShowMenu(findGameMenu);
    }

    public void ClickOpenMarketplace()
    {
        ShowMenu(marketplaceMenu);
    }
}
