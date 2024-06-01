using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
public class MONA_MAP_ONPLAYER : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public PhotonView photonView;
    public GameObject playerObject;

    void Start()
    {
        //gen map

        if (!photonView.IsMine)
        {
            return;
        }


        GameObject spawn = GameObject.Find("SpawnPoint");
        if (spawn == null)
        {
            //CHECK TO SEE IF THIS THE MONA SCENE

            string customProperties = PhotonNetwork.CurrentRoom.CustomProperties.ToString();
            Debug.Log("Custom Game Photon Properties: " + customProperties);

            if (customProperties.Contains("GBLURL") == false)
            {
                return;
            }

            string gblUrl = PhotonNetwork.CurrentRoom.CustomProperties["GBLURL"].ToString();
            Debug.Log("GBLURL: " + gblUrl);

            GameObject spaceloader = GameObject.Find("SpaceLoader");
            var loader = spaceloader.gameObject.GetComponent<SpaceLoader>();
            loader.LoadSpace(gblUrl, "Space", true, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

            StartCoroutine(WaitForSpawnPointAndInstantiate());

        }


    }

    // Update is called once per frame
    void Update()
    {

    }


    private IEnumerator WaitForSpawnPointAndInstantiate()
    {
        bool loaded = false;
        GameObject spawn = null;

        while (!loaded)
        {
            spawn = GameObject.Find("SpawnPoint");
            if (spawn != null)
            {
                playerObject.transform.position = spawn.transform.position;
                loaded = true;
            }
            else
            {
                yield return new WaitForSeconds(2);
            }
        }
    }
}
