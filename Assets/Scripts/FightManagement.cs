using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Thirdweb;
using Photon.Realtime;
public class FightManagement : MonoBehaviour
{

    public float maxhealth;
    public float curHealth;
    public Transform healthBar;
    public PhotonView photonView;


    void refreshHealthBar()
    {
        float healthPct = curHealth / maxhealth;
        healthBar.localScale = Vector3.Lerp(healthBar.localScale, new Vector3(healthPct, 1, 1), Time.deltaTime * 8f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }



    public void TakeDamage(float damage)
    {

        photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);

    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        curHealth -= damage;
        if (curHealth <= 0)
        {
            curHealth = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        refreshHealthBar();
    }
}
