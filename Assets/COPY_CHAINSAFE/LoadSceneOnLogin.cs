using ChainSafe.Gaming.UnityPackage.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ChainSafe.Gaming.UnityPackage.Common
{
    /// <summary>
    /// Loads scene when Web3 Instance is initialized.
    /// </summary>
    public class LoadSceneOnLogin : MonoBehaviour, IWeb3InitializedHandler
    {




        public GameObject connectWalletText;
        public Button startNew;
        public Button joinGame;
        public Button inventory;
        public Button marketplace;



        public GameObject walletConnectUI;





        /// <summary>
        /// Login scene cached/saved for Logout since we can have more than one Login scene.
        /// </summary>

        public void Start()
        {
            connectWalletText.SetActive(true);
            startNew.interactable = false;
            joinGame.interactable = false;
            inventory.interactable = false;
            marketplace.interactable = false;

        }

        public void OnWeb3Initialized()
        {
            connectWalletText.SetActive(false);
            startNew.interactable = true;
            joinGame.interactable = true;
            inventory.interactable = true;
            marketplace.interactable = true;

            walletConnectUI.SetActive(false);

        }
    }
}
