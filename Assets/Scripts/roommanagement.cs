using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roommanagement : MonoBehaviour
{

    public GameObject roomJoinUI;
    public GameObject mainmenuUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

    }


    public void clickStartNew(){
        Debug.Log("Start New Room");
        roomJoinUI.SetActive(true);
        mainmenuUI.SetActive(false);
    }

}
