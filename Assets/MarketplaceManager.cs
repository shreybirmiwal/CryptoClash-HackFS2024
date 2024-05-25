using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;

public class MarketplaceManager : MonoBehaviour
{
    public string projectID = "0e014a1b-1024-4027-a253-0138e863480e";
    public string apiURL = "https://api.gaming.chainsafe.io/v1/projects/";
    public GameObject itemTemplate;
    public Transform contentTransform;

    void Start()
    {
        StartCoroutine(GetMarketplaceItems());
    }

    IEnumerator GetMarketplaceItems()
    {
        string url = apiURL + projectID + "/items?chainId=1"; // Assuming chainId is 1 for example
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            ItemsResponse itemsResponse = JsonConvert.DeserializeObject<ItemsResponse>(jsonResponse);
            PopulateItems(itemsResponse.items);
        }
    }

    void PopulateItems(List<Item> items)
    {
        foreach (var item in items)
        {
            GameObject itemGO = Instantiate(itemTemplate, contentTransform);
            itemGO.transform.Find("ItemName").GetComponent<Text>().text = item.token.metadata["name"].ToString();
            itemGO.transform.Find("ItemPrice").GetComponent<Text>().text = item.price;
            Button purchaseButton = itemGO.transform.Find("PurchaseButton").GetComponent<Button>();
            purchaseButton.onClick.AddListener(() => PurchaseItem(item.id));
        }
    }

    void PurchaseItem(string itemID)
    {
        Debug.Log("Purchasing item: " + itemID);
        // Implement purchase logic here
    }

    [System.Serializable]
    public class ItemsResponse
    {
        public List<Item> items;
    }

    [System.Serializable]
    public class Item
    {
        public string id;
        public Token token;
        public string price;
    }

    [System.Serializable]
    public class Token
    {
        public Dictionary<string, object> metadata;
    }
}
