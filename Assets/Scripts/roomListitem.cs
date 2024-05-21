using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class roomListitem : MonoBehaviour
{

    [SerializeField] TMP_Text text;
    RoomInfo info;


    public void SetRoomInfo(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;
    }


    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
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
