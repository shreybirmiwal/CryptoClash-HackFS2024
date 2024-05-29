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
        if (scene.buildIndex != 0) // We're in the game scene
        {

            if (scene.buildIndex == 7)
            {
                //we in a mona inport world

                //load the world
                string customProperties = PhotonNetwork.CurrentRoom.CustomProperties.ToString();
                Debug.Log("Custom Game Photon Properties: " + customProperties);

                string gblUrl = PhotonNetwork.CurrentRoom.CustomProperties["GBLURL"].ToString();
                Debug.Log("GBLURL: " + gblUrl);


                GameObject glbLoader = GameObject.Find("glbLoader");
                var loader = glbLoader.gameObject.GetComponent<GlbLoader>();

                loader.Load(gblUrl, true, (GameObject obj) =>
                {
                    obj.transform.position = new Vector3(0, 0, 0);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.transform.rotation = Quaternion.Euler(0, 0, 0);
                    Debug.Log("Loaded Map");
                });




                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);

            }
        }
    }
}