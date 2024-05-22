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


    public BoxCollider attackZone;


    public Animator animator_boxing;
    public Animator animator_watermellon;
    public Animator animator_moonsword;



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

            CheckHit();
        }
    }

    void CheckHit()
    {
        Collider[] hitColliders = Physics.OverlapBox(attackZone.center, attackZone.size / 2, attackZone.transform.rotation);

        foreach (Collider collider in hitColliders)
        {
            Debug.Log(collider.name);
            if (collider.CompareTag("hitbox"))
            {
                Debug.Log("Hitbox hit!");
                // Handle hit logic here, e.g., applying damage
                photonView.RPC("RPC_TakeDamage", RpcTarget.All, 10.0f); // example damage value
            }
        }
    }
}

