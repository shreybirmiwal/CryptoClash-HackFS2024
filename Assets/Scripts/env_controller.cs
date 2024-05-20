using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class env_controller : MonoBehaviour
{

    public GameObject TempleTussle;
    public GameObject BazaarBash;
    public GameObject ShipScuttle;
    public GameObject CementaryClash;
    public GameObject SpaceSkirmish;


    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShowOnly(TempleTussle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShowOnly(BazaarBash);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ShowOnly(ShipScuttle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ShowOnly(CementaryClash);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ShowOnly(SpaceSkirmish);
        }
    }

    void ShowOnly(GameObject gameObjectToShow)
    {
        TempleTussle.SetActive(false);
        BazaarBash.SetActive(false);
        ShipScuttle.SetActive(false);
        CementaryClash.SetActive(false);
        SpaceSkirmish.SetActive(false);

        gameObjectToShow.SetActive(true);


        player.transform.position = new Vector3(0, 0, 0);
    }
}
