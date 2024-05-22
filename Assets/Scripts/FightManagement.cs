using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class FightManagement : MonoBehaviour
{
    public float maxhealth = 100f;
    public float curHealth;
    public Transform healthBar;
    public PhotonView photonView;

    [SerializeField] Camera cam;

    public Animator animator_boxing;
    public Animator animator_watermellon;
    public Animator animator_moonsword;

    public GameObject looseUI;
    public GameObject winUI;

    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        curHealth = maxhealth;
        refreshHealthBar();
    }

    void refreshHealthBar()
    {
        Debug.Log("Refreshing Health Bar CALLED ,  CUR HEALTH IS: " + curHealth + " MAX HEALTH IS: " + maxhealth);
        float healthPct = curHealth / maxhealth;
        healthBar.localScale = Vector3.Lerp(healthBar.localScale, new Vector3(healthPct, 1, 1), Time.deltaTime * 8f);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        Debug.Log("RPC_TakeDamage called by " + info.Sender);

        if (!photonView.IsMine)
        {
            return;
        }

        curHealth -= damage;
        if (curHealth <= 0)
        {
            curHealth = 0;
            Debug.Log("Player is dead.");

            looseUI.SetActive(true);

            // Inform the opponent that they have won
            PhotonView opponentPhotonView = PhotonView.Find(info.Sender.ActorNumber);
            if (opponentPhotonView != null && opponentPhotonView.Owner != null)
            {
                opponentPhotonView.RPC("RPC_DisplayWinUI", opponentPhotonView.Owner);
            }
        }

        Debug.Log("Damage taken: " + damage + ", Current Health: " + curHealth);

        refreshHealthBar();
    }

    [PunRPC]
    void RPC_DisplayWinUI()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        winUI.SetActive(true);
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        refreshHealthBar();


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fired");

            if (animator_boxing != null) animator_boxing.SetTrigger("hit");
            if (animator_watermellon != null) animator_watermellon.SetTrigger("hit");
            if (animator_moonsword != null) animator_moonsword.SetTrigger("hit");

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            ray.origin = cam.transform.position - ray.direction * 0.1f;

            if (Physics.Raycast(ray, out RaycastHit hit, 3f))
            {
                Debug.Log("Raycast hit: " + hit.collider.name);

                PhotonView targetPhotonView = hit.collider.GetComponent<PhotonView>();
                if (targetPhotonView != null && !targetPhotonView.IsMine)
                {
                    Debug.Log("Calling TakeDamage on target");
                    targetPhotonView.RPC("RPC_TakeDamage", RpcTarget.All, 20f);
                }
                else
                {
                    Debug.Log("Hit object does not have a PhotonView or is the current player");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything");
            }
        }
    }
}
