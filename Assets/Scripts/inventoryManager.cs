using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;

public class inventoryManager : MonoBehaviour
{

    public GameObject connectedWalletUI;
    public GameObject noConnectedWalletUI;

    private ThirdwebSDK sdk;

    // Start is called before the first frame update
    void Start()
    {
        sdk = ThirdwebManager.Instance.SDK;
    }

    // Update is called once per frame
    void Update()
    {
        checkIfConnected();
    }



    async void checkIfConnected()
    {
        var data = await sdk.Wallet.IsConnected();


        if (data == false)
        {
            connectedWalletUI.SetActive(false);
            noConnectedWalletUI.SetActive(true);
        }
        else
        {
            connectedWalletUI.SetActive(true);
            noConnectedWalletUI.SetActive(false);
        }
    }
}
