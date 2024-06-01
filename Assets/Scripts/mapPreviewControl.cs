using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro dropdowns
using UnityEngine.UI; // For RawImage components
using Photon.Pun;
using Photon.Realtime;

public class mapPreviewControl : MonoBehaviour
{

    public TMP_Dropdown mapDropdown;
    public List<Texture> mapTextures;
    public RawImage mapImagePreview;


    public Button submitButton;
    public GameObject errorText;


    // Start is called before the first frame update
    void Start()
    {
        mapDropdown.onValueChanged.AddListener(delegate { UpdateMapPreview(); });

        UpdateMapPreview();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateMapPreview()
    {
        mapImagePreview.texture = mapTextures[mapDropdown.value];

        if (mapDropdown.value == 2 || mapDropdown.value == 4)
        {
            //bazaar bash or cemetary clash

            mapDropdown.GetComponent<Image>().color = Color.red;
            submitButton.interactable = false;
            errorText.SetActive(true);

            //mapdropdown color change
            //submitbutton disabled
            //error text displayed
        }
        else
        {
            mapDropdown.GetComponent<Image>().color = Color.white;
            submitButton.interactable = true;
            errorText.SetActive(false);
        }
    }
}
