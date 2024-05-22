using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Thirdweb;
using Photon.Realtime;
using System.Text.RegularExpressions;

public class skinApplier : MonoBehaviour
{


    public List<GameObject> skins;
    public List<GameObject> weapons;



    // Start is called before the first frame update
    void Start()
    {
        string nick = PhotonNetwork.NickName;
        if (nick.Contains("*"))
        {
            // Code to handle the case when nick contains *
            string pattern = @"\d+";
            MatchCollection matches = Regex.Matches(nick, pattern);


            string firstNumber = matches[0].Value;
            string secondNumber = matches[1].Value;

            string username = nick.Substring(nick.IndexOf("*") + 1);

            Debug.Log("FIRST NUMBER IS " + firstNumber);
            Debug.Log("SECOND NUMBER IS " + secondNumber);
            Debug.Log("USERNAME IS " + username);


            int weaponIndex = int.Parse(firstNumber);
            int skinIndex = int.Parse(secondNumber);

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
        else
        {
            Debug.Log("USE DEFUALTS");
            Debug.Log("NAME IS " + nick);
            // Use defaults
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
