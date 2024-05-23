using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FightManagement : MonoBehaviourPunCallbacks
{
    public float maxhealth = 100f;
    public float curHealth;
    public Transform healthBar;
    public PhotonView photonView;

    [SerializeField] Camera cam;
    public float detectionRadius = 3f; // Radius for detecting targets
    public float raycastDistance = 3f; // Maximum distance for the raycast

    public Animator animator_boxing;
    public Animator animator_watermellon;
    public Animator animator_moonsword;
    public Animator animator_plunger;

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
        // Debug.Log("Refreshing Health Bar CALLED, CUR HEALTH IS: " + curHealth + " MAX HEALTH IS: " + maxhealth);
        float healthPct = curHealth / maxhealth;
        healthBar.localScale = Vector3.Lerp(healthBar.localScale, new Vector3(healthPct, 1, 1), Time.deltaTime * 8f);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        Debug.Log("RPC_TakeDamage called by " + info.Sender);

        if (photonView.IsMine)
        {
            curHealth -= damage;
            if (curHealth <= 0)
            {
                curHealth = 0;
                Debug.Log("Player is dead.");

                looseUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;

                Hashtable hash = new Hashtable();
                hash.Add("GameEnded", true);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            }

            Debug.Log("Damage taken: " + damage + ", Current Health: " + curHealth);

            refreshHealthBar();
        }
    }

    public void returnToLobby()
    {
        PhotonNetwork.LoadLevel(0);
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
            if (animator_plunger != null) animator_plunger.SetTrigger("hit");

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            ray.origin = cam.transform.position - ray.direction * 0.1f;

            // Find all colliders within the detection radius
            Collider[] hitColliders = Physics.OverlapSphere(cam.transform.position, detectionRadius);
            foreach (var hitCollider in hitColliders)
            {
                // Check if the hit collider has a PhotonView component
                PhotonView targetPhotonView = hitCollider.GetComponent<PhotonView>();
                if (targetPhotonView != null && !targetPhotonView.IsMine)
                {
                    // Perform a raycast to ensure there's a clear line of sight
                    if (Physics.Raycast(ray.origin, (hitCollider.transform.position - ray.origin).normalized, out RaycastHit hit, raycastDistance))
                    {
                        if (hit.collider == hitCollider)
                        {
                            Debug.Log("Raycast hit: " + hit.collider.name);
                            Debug.Log("Calling TakeDamage on target");
                            targetPhotonView.RPC("RPC_TakeDamage", RpcTarget.All, 20f);
                        }
                        else
                        {
                            Debug.Log("Line of sight blocked to " + hitCollider.name);
                        }
                    }
                }
                else
                {
                    Debug.Log("Hit object does not have a PhotonView or is the current player");
                }
            }
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("GameEnded") && (bool)propertiesThatChanged["GameEnded"])
        {
            if (!looseUI.activeSelf) // Ensure the looseUI is not active to show winUI
            {

                if (winUI != null)
                {
                    winUI.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;

                }
            }
        }
    }
}
