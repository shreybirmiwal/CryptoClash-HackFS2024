using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Thirdweb;
using Photon.Realtime;
using System.Text.RegularExpressions;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class skinApplier : MonoBehaviourPunCallbacks
{

    public PhotonView photonView;

    public List<GameObject> skins;
    public List<GameObject> weapons;



    void setGunWeaponIndex(int skinIndex, int weaponIndex)
    {
        foreach (var skin in skins)
        {
            skin.SetActive(false);
        }
        skins[skinIndex].SetActive(true);

        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }
        weapons[weaponIndex].SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {

        if (!photonView.IsMine)
        {
            return;
        }
        else
        {

            string nick = PhotonNetwork.NickName;
            int weaponIndex = 0;
            int skinIndex = 0;
            string username = "";

            if (nick.Contains("*"))
            {
                string pattern = @"\d+";
                MatchCollection matches = Regex.Matches(nick, pattern);
                weaponIndex = int.Parse(matches[0].Value);
                skinIndex = int.Parse(matches[1].Value);
                username = nick.Substring(nick.IndexOf("*") + 1);
            }

            Hashtable hash = new Hashtable();
            hash.Add("skinIndex", skinIndex);
            hash.Add("weaponIndex", weaponIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            setGunWeaponIndex(skinIndex, weaponIndex);

        }
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!photonView.IsMine && targetPlayer == photonView.Owner)
        {
            int skinIndex = 0;
            int weaponIndex = 0;

            if (changedProps.ContainsKey("skinIndex"))
            {
                skinIndex = (int)changedProps["skinIndex"];
            }

            if (changedProps.ContainsKey("weaponIndex"))
            {
                weaponIndex = (int)changedProps["weaponIndex"];
            }

            setGunWeaponIndex(skinIndex, weaponIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
