using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private GameObject roomlistGUI;

    [SerializeField]
    private Transform roomListParent;

    [SerializeField]
    private Text status;

    private NetworkManager networkManager;

    public GameObject networkUI;


    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0,20,"", false, 0, 0, OnMatchList);
        status.text = "Loading...";


    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";

        if(matchList == null)
        {
            status.text = "Couldn't get room list.";
            return;
        }


        foreach(MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGO = Instantiate(roomlistGUI);
            _roomListItemGO.transform.SetParent(roomListParent);


            RoomItem roomItem = _roomListItemGO.GetComponent<RoomItem>();
            if(roomItem != null)
            {
                roomItem.Setup(match, JoinRoom);
            }
            roomList.Add(_roomListItemGO);
        }

        if(roomList.Count == 0)
        {
            status.text = "No rooms at the moment";
        }
    }

   void ClearRoomList()
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
        
    }


    public void JoinRoom(MatchInfoSnapshot _match)
    {
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "Joining...";
        networkUI.SetActive(false);
        

    }

}
