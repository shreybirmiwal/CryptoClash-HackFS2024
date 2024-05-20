using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelect : MonoBehaviour
{
    public Dropdown weaponDropdown;
    public Dropdown skinDropdown;
    public Image weaponImagePreview;
    public Image skinImagePreview;

    private List<string> ownedSkins;
    private List<string> ownedWeapons;

    void Start()
    {
        weaponDropdown.onValueChanged.AddListener(delegate { UpdateWeaponPreview(); });
        skinDropdown.onValueChanged.AddListener(delegate { UpdateSkinPreview(); });
    }

    void Update()
    {

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
    }

    // Populate dropdown with items
    void PopulateDropdown(Dropdown dropdown, List<string> items)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(items);
        dropdown.value = 0; // Ensure the first item is selected by default
        dropdown.RefreshShownValue();
    }

    // Update the weapon image preview
    void UpdateWeaponPreview()
    {
        string selectedWeapon = weaponDropdown.options[weaponDropdown.value].text;
        weaponImagePreview.sprite = GetSpriteForWeapon(selectedWeapon);
    }

    // Update the skin image preview
    void UpdateSkinPreview()
    {
        string selectedSkin = skinDropdown.options[skinDropdown.value].text;
        skinImagePreview.sprite = GetSpriteForSkin(selectedSkin);
    }

    // Function to get the user's owned skins
    List<string> getSkins()
    {
        return new List<string> { "skin1", "skin2", "skin3" };
    }

    // Function to get the user's owned weapons
    List<string> getWeapons()
    {
        return new List<string> { "weapon1", "weapon2", "weapon3" };
    }

    Sprite GetSpriteForWeapon(string weaponName)
    {
        return Resources.Load<Sprite>($"Weapons/{weaponName}");
    }

    Sprite GetSpriteForSkin(string skinName)
    {
        return Resources.Load<Sprite>($"Skins/{skinName}");
    }
}
