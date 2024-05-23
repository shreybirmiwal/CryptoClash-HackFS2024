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
    }
}
