using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NetworkIdentity))]
public class Button : NetworkBehaviour {

    //[SerializeField]
    //private GameObject triggerObject;

    [Header("Starting value of the button")]
    public bool startingValue = true;
    static public bool active;

    [Header("Time delay between button switches")]
    public float timeDelay = 0.1f;
    private float timer;

    AudioSource buttonClick;

    private void Start()
    {
        buttonClick = GetComponent<AudioSource>();
        active = startingValue;
        timer = timeDelay;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

   [ClientRpc]
    public void RpcTriggered()
    {
        buttonClick.Play();

        if (timer < 0f)
        {
            active = !active;
            timer = timeDelay;
        }


    }
}
