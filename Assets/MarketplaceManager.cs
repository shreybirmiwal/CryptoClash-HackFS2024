using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;

public class MarketplaceManager : MonoBehaviour
{
    public string projectID;
    public string marketplaceID;
    public long priceMultiplier;
    private string apiURL = "https://api.gaming.chainsafe.io/v1/projects/";
    public GameObject itemTemplate;
    public Transform contentTransform;

    void Start()
    {
        Debug.Log("Marketplace opened");
        StartCoroutine(GetMarketplaceItems());
    }

    IEnumerator GetMarketplaceItems()
    {
        string url = $"{apiURL}{projectID}/marketplaces/{marketplaceID}/items";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            ItemsResponse itemsResponse = JsonConvert.DeserializeObject<ItemsResponse>(jsonResponse);
            PopulateItems(itemsResponse.items);
        }
    }

    void PopulateItems(List<Item> items)
    {
        foreach (var item in items)
        {
            GameObject itemGO = Instantiate(itemTemplate, contentTransform);
            TMP_Text nameText = itemGO.transform.Find("ItemName").GetComponent<TMP_Text>();
            TMP_Text priceText = itemGO.transform.Find("ItemPrice").GetComponent<TMP_Text>();
            Image itemImage = itemGO.transform.Find("ItemImage").GetComponent<Image>();
            Button purchaseBtn = itemGO.transform.Find("PurchaseButton").GetComponent<Button>();

            nameText.text = item.token.metadata["name"].ToString();
            priceText.text = convertToEth(item.price);
            StartCoroutine(LoadImage(item.token.metadata["image"].ToString(), itemImage));
            purchaseBtn.onClick.AddListener(() => PurchaseItem(item.id));
        }
    }


    string convertToEth(string value)
    {
        value = value.Substring(0, value.Length - 13);
        long intValue = long.Parse(value);
        float ethValue = (float)intValue / (float)100000;
        string ethString = ethValue.ToString("F4") + " eth";
        return ethString;
    }



    IEnumerator LoadImage(string imageUrl, Image itemImage)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            itemImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    void PurchaseItem(string itemID)
    {
        Debug.Log("Purchasing item: " + itemID);
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
