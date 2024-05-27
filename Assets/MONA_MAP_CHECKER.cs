using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Thirdweb;
using TMPro;

public class MONA_MAP_CHECKER : MonoBehaviour
{

    // private ThirdwebSDK sdk;

    public GameObject auth_button;
    public TMP_Text errorTextCreateRoom;

    // Start is called before the first frame update
    void Start()
    {
        // sdk = ThirdwebManager.Instance.SDK;

    }

    // Update is called once per frame
    void Update()
    {

        checkIfWalletConnected();
    }


    async void checkIfWalletConnected()
    {
        // var data = await sdk.Wallet.IsConnected();
        // if (data == false)
        // {
        //     errorTextCreateRoom.text = "Please connect your wallet";
        //     return;
        // }

    }



}
