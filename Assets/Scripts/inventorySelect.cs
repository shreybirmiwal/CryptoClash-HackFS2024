using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro dropdowns
using UnityEngine.UI; // For RawImage components
using Photon.Pun;
using Photon.Realtime;

public class InventorySelect : MonoBehaviour
{
    public TMP_Dropdown weaponDropdown;
    public TMP_Dropdown skinDropdown;
    public RawImage weaponImagePreview;
    public RawImage skinImagePreview;


    public TMP_InputField usernameInput;



    public List<Texture> skinsTexture;
    public List<Texture> weaponsTexture;

    void Start()
    {
        weaponDropdown.onValueChanged.AddListener(delegate { UpdateWeaponPreview(); });
        skinDropdown.onValueChanged.AddListener(delegate { UpdateSkinPreview(); });


        if (PhotonNetwork.NickName != "" || PhotonNetwork.NickName != null)
        {

            int asteriskIndex = PhotonNetwork.NickName.IndexOf('*');
            usernameInput.text = PhotonNetwork.NickName.Substring(asteriskIndex + 1);

        }
        else
        {
            usernameInput.text = "Player " + Random.Range(0, 1000).ToString("0000");
        }
    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        UpdateSkinPreview();
        UpdateWeaponPreview();
    }


    void UpdateWeaponPreview()
    {
        weaponImagePreview.texture = weaponsTexture[weaponDropdown.value];
    }

    void UpdateSkinPreview()
    {
        skinImagePreview.texture = skinsTexture[skinDropdown.value];
    }


    public void onSubmitInventory()
    {
        Debug.Log("Username: " + usernameInput.text);
        Debug.Log("Selected Weapon: " + weaponDropdown.options[weaponDropdown.value].text + "num " + weaponDropdown.value);
        Debug.Log("Selected Skin: " + skinDropdown.options[skinDropdown.value].text + "num " + skinDropdown.value);

        int weaponRETURN = weaponDropdown.value;
        int skinRETURN = skinDropdown.value;


        PhotonNetwork.NickName = weaponRETURN.ToString() + "-" + skinRETURN.ToString() + "*" + usernameInput.text;

        Debug.Log("PhotonNetwork.NickName: " + PhotonNetwork.NickName);

    }
}
