using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class HostGameUI : MonoBehaviour {

	private NetworkManager networkManager;

    private string roomName = "No name";

    private uint roomSize = 2;

    [SerializeField]
    private Canvas networkUI;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string _name)
    {

        roomName = _name;

    }




    public void CreateRoom()
    {
        if (roomName != "No name")
        {
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
            networkUI.gameObject.SetActive(false);
        }
    }


}
