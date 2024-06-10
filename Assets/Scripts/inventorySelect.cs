using ChainSafe.Gaming.UnityPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro dropdowns
using UnityEngine.UI; // For RawImage components
using Photon.Pun;
using Photon.Realtime;
using ChainSafe.Gaming.Evm.Contracts.BuiltIn;
using Scripts.EVM.Token;

public class InventorySelect : MonoBehaviour
{
    public TMP_Dropdown weaponDropdown;
    public TMP_Dropdown skinDropdown;
    public RawImage weaponImagePreview;
    public RawImage skinImagePreview;


    public TMP_InputField usernameInput;


    public GameObject loader;

    public List<Texture> skinsTexture;
    public List<Texture> weaponsTexture;


    private string account;

    private string WeaponcontractAddress = "0x93b9a7f44acd5827c1a438cf21785f9321bc382c";
    private string SkinsContractAddress = "0xd8ba4cd13542915a6de2402b8f61d510baae0890";


    public Button submitButton;


    public GameObject mustOwnItemText;



    private bool ownsWeapon = true;
    private bool ownSkin = true;


    void Start()
    {
        loader.SetActive(false);

        account = Web3Accessor.Web3.Signer.PublicAddress;
        weaponDropdown.onValueChanged.AddListener(delegate { UpdateWeaponPreview(); });
        skinDropdown.onValueChanged.AddListener(delegate { UpdateSkinPreview(); });

        UpdateSkinPreview();
        UpdateWeaponPreview();

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



    private int getWeaponTokenID(int weaponIndex)
    {
        if (weaponIndex == 0) return -1;
        if (weaponIndex == 1) return 7; //watermellon wacker
        if (weaponIndex == 2) return 1; //poopy
        if (weaponIndex == 3) return 0; //etherual

        return -2;
    }

    private int getSkinID(int skinIndex)
    {
        if (skinIndex == 0) return -1;
        if (skinIndex == 1) return 2;
        if (skinIndex == 2) return 3;
        if (skinIndex == 3) return 11;
        if (skinIndex == 4) return 10;
        if (skinIndex == 5) return 5;
        if (skinIndex == 6) return 13;
        if (skinIndex == 7) return 6;
        if (skinIndex == 8) return 9;
        if (skinIndex == 9) return 14;
        if (skinIndex == 10) return 12;
        if (skinIndex == 11) return 4;

        return -1;

    }



    async void UpdateWeaponPreview()
    {
        weaponImagePreview.texture = weaponsTexture[weaponDropdown.value];


        var tokenID = getWeaponTokenID(weaponDropdown.value);
        if (tokenID == -1)
        {
            ownsWeapon = true;
            return;
        }

        loader.SetActive(true);

        var owner = await Web3Accessor.Web3.Erc721.GetOwnerOf(WeaponcontractAddress, tokenID);
        loader.SetActive(false);

        Debug.Log("Owner of weapon: " + owner);
        if (owner == account)
        {
            ownsWeapon = true;
        }
        else
        {
            ownsWeapon = false;
        }


    }

    async void UpdateSkinPreview()
    {
        skinImagePreview.texture = skinsTexture[skinDropdown.value];

        var tokenID = getSkinID(skinDropdown.value);
        if (tokenID == -1)
        {
            ownSkin = true;
            return;
        }

        loader.SetActive(true);
        var owner = await Web3Accessor.Web3.Erc721.GetOwnerOf(SkinsContractAddress, tokenID);
        loader.SetActive(false);

        Debug.Log("Owner: " + owner);

        if (owner == account)
        {
            ownSkin = true;
        }
        else
        {
            ownSkin = false;
        }

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


    void Update()
    {
        if (ownsWeapon && ownSkin)
        {
            mustOwnItemText.SetActive(false);
            submitButton.interactable = true;
        }
        else
        {
            mustOwnItemText.SetActive(true);
            submitButton.interactable = false;
        }
    }
}
