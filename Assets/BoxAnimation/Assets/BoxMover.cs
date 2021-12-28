using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class BoxMover : NetworkBehaviour {

    public Animator[] anim;
    [Header("Parameter to trigger")]
    public string paramName = "move";


    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Button.active)
        {
            RpcTriggered();
        }
    }


    [ClientRpc]
    public void RpcTriggered()
    {
        Debug.Log("fdsa");

        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(paramName);
        }
    }
}
