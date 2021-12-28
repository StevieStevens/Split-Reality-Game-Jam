using UnityEngine;
using UnityEngine.Networking;

public class PlayerInteract : NetworkBehaviour {


    [SerializeField]
    private Camera cam;

    [SerializeField]
    private float rayDistance = 10f;

	// Use this for initialization
	void Start () {
		if(cam == null)
        {
            Debug.Log("you are missing the player camera reference in the player interact script");
            this.enabled = false;
        }
	}
	
    [Client]
    public void CmdInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayDistance))
        {
           if (hit.transform.tag == "Interact")
            {
                CmdInteractAllClients(hit.transform.gameObject);
            }
        }
    }

    [Command]
    private void CmdInteractAllClients(GameObject interactable)
    {
        interactable.BroadcastMessage("RpcTriggered");

    }
}
