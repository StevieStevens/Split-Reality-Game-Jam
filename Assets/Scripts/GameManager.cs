using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public int playernumber = 0;


    [ClientRpc]
    public void RpcAssigned()
    {
        playernumber++;


    }
}
