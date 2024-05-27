using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class importWorldManager : MonoBehaviour
{

    public TMP_InputField roomNameInput;
    public TMP_Dropdown mapDropdown;


    public void updateDropdown(List<string> mapNames)
    {
        mapDropdown.options.Clear();
        mapDropdown.AddOptions(mapNames);

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
