using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro dropdowns
using UnityEngine.UI; // For RawImage components

public class InventorySelect : MonoBehaviour
{
    public TMP_Dropdown weaponDropdown;
    public TMP_Dropdown skinDropdown;
    public RawImage weaponImagePreview;
    public RawImage skinImagePreview;

    private List<string> ownedSkins;
    private List<string> ownedWeapons;




    public List<Texture> skinsTexture;
    public List<Texture> weaponsTexture;

    void Start()
    {
        weaponDropdown.onValueChanged.AddListener(delegate { UpdateWeaponPreview(); });
        skinDropdown.onValueChanged.AddListener(delegate { UpdateSkinPreview(); });
    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        // Get the user's inventory from wallet
        ownedSkins = getSkins();
        ownedWeapons = getWeapons();

        // Display the inventory
        PopulateDropdown(weaponDropdown, ownedWeapons);
        PopulateDropdown(skinDropdown, ownedSkins);

        UpdateSkinPreview();
        UpdateWeaponPreview();
    }

    void PopulateDropdown(TMP_Dropdown dropdown, List<string> items)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(items);
        dropdown.value = 0; // Ensure the first item is selected by default
        dropdown.RefreshShownValue();
    }

    void UpdateWeaponPreview()
    {
        string selectedWeapon = weaponDropdown.options[weaponDropdown.value].text;
        weaponImagePreview.texture = GetTextureForWeapon(selectedWeapon);
    }

    void UpdateSkinPreview()
    {
        string selectedSkin = skinDropdown.options[skinDropdown.value].text;
        skinImagePreview.texture = GetTextureForSkin(selectedSkin);
    }

    List<string> getSkins()
    {

        return new List<string> { "DefaultDon", "AngelicAmy", "ClowningCarl", "ChiefCop", "DistinguishedDevil" };
    }

    List<string> getWeapons()
    {
        return new List<string> { "EtherealEdge", "GnarlyGloves", "WatermellonWacker" };

    }

    Texture GetTextureForWeapon(string weaponName)
    {
        if (weaponName == "EtherealEdge") return weaponsTexture[0];
        if (weaponName == "GnarlyGloves") return weaponsTexture[1];
        if (weaponName == "WatermellonWacker") return weaponsTexture[2];

        return weaponsTexture[0];
    }

    Texture GetTextureForSkin(string skinName)
    {
        if (skinName == "DefaultDon") return skinsTexture[0];
        if (skinName == "AngelicAmy") return skinsTexture[1];
        if (skinName == "ClowningCarl") return skinsTexture[2];
        if (skinName == "ChiefCop") return skinsTexture[3];
        if (skinName == "DistinguishedDevil") return skinsTexture[4];


        return skinsTexture[0];
    }
}
