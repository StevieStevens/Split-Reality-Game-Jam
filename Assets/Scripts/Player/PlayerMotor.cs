using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : NetworkBehaviour {

    float _currentCamRot = 0;
    Camera cam;
    Rigidbody rb;
    // Use this for initialization

    private float rotLimit = 85f;


	void Start () {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
	}



    public void MovePlayer(Vector3 newMovement)
    {
        rb.MovePosition(rb.position + newMovement * Time.fixedDeltaTime);
    }

    public void RotatePlayer(float newHorizontalRot, float newVerticalRot)
    {
        transform.Rotate(0, newHorizontalRot, 0);


        _currentCamRot -= newVerticalRot;

        _currentCamRot =  Mathf.Clamp(_currentCamRot, -rotLimit, rotLimit);
        cam.transform.localEulerAngles = new Vector3(-_currentCamRot, 0, 0);


    }

    public void Jump(float newForce)
    {
        rb.AddForce(0f, newForce, 0);
    }

}
