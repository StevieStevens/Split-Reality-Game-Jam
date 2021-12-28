using UnityEngine.Networking;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Camera sceneCamera;

    [SerializeField]
    private Behaviour[] componentsToDisable;

    [SerializeField]
    private Camera playerCamera;

    private PlayerController pc;

    [SerializeField]
    private ParticleSystem ps;

    [SyncVar]
    public bool emissionOn = true;





    private string PlayerVisualLayer = "Player";

	// Use this for initialization
	void Start () {



        if (!isLocalPlayer)
        {

            DisableComponents();
        }
        else
        {

            sceneCamera = Camera.main;
            sceneCamera.gameObject.SetActive(false);

            AssignPlayerLayer();
            emissionOn = false;

        }

    }

    


    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }


    void AssignPlayerLayer()
    {

        int playerNumber = 1;
        if(NetworkManager.singleton.numPlayers == 0)
        {
            playerNumber = 2;
        }
        Debug.Log(playerNumber);
        PlayerVisualLayer = PlayerVisualLayer + playerNumber.ToString();

        playerCamera = GetComponentInChildren<Camera>();
        playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(PlayerVisualLayer));
    }

    [ClientRpc]
    public void RpcSetEmission(bool newEmissionState)
    {
       emissionOn = newEmissionState;
       ps.enableEmission = emissionOn;
       Debug.Log("emission should be on");

    }


    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }


}
