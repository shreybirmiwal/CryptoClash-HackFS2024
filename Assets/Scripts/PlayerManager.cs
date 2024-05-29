using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {

        GameObject spawn = GameObject.Find("SpawnPoint");
        if (spawn != null)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "photon_player2"), spawn.transform.position, spawn.transform.rotation);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "photon_player2"), Vector3.zero, Quaternion.identity);
        }

        Debug.Log("Instantiated Player Controller");
    }
}