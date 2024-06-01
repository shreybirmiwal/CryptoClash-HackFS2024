using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {



        if (scene.buildIndex >= 0 && scene.buildIndex <= 7) //0-7, cuz the 'spaces' scene will instantiate 2 players
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }

        // if (scene.buildIndex == 7)
        // {

        //     string customProperties = PhotonNetwork.CurrentRoom.CustomProperties.ToString();
        //     Debug.Log("Custom Game Photon Properties: " + customProperties);

        //     string gblUrl = PhotonNetwork.CurrentRoom.CustomProperties["GBLURL"].ToString();
        //     Debug.Log("GBLURL: " + gblUrl);

        //     GameObject spaceloader = GameObject.Find("SpaceLoader");
        //     var loader = spaceloader.gameObject.GetComponent<SpaceLoader>();
        //     loader.LoadSpace(gblUrl, "Space", true, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        //     // Start a coroutine to wait for the SpawnPoint and instantiate the player
        //     StartCoroutine(WaitForSpawnPointAndInstantiate());
        // }
        // else if (scene.buildIndex >= 0 && scene.buildIndex <= 6) //0-6
        // {
        //     PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        // }

    }

    // private IEnumerator WaitForSpawnPointAndInstantiate()
    // {
    //     bool loaded = false;
    //     GameObject spawn = null;

    //     while (!loaded)
    //     {
    //         spawn = GameObject.Find("SpawnPoint");
    //         if (spawn != null)
    //         {
    //             PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
    //             Debug.Log("INSTANTIATE 1");
    //             loaded = true;
    //         }
    //         else
    //         {
    //             yield return new WaitForSeconds(2);
    //         }
    //     }
    // }
}
