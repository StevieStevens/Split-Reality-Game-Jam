using UnityEngine;

public class KillBox : MonoBehaviour {

    [SerializeField]
    private float forceApplied = 1000f;



    private void OnTriggerEnter(Collider other)
    {
        
        switch (other.tag)
        {
            case "Player":
                other.GetComponent<PlayerController>().Die();
                Rigidbody playerRB = other.GetComponent<Rigidbody>();
                playerRB.AddTorque(Random.rotation.eulerAngles);
                playerRB.AddForce((transform.forward - other.transform.forward) * forceApplied);
                break;
        }        
    }
}
