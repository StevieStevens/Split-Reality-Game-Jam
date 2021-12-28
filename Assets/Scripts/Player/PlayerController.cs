using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerInteract))]
public class PlayerController : NetworkBehaviour {

    //player movement speed
    [SerializeField]
    private float walkingSpeed = 5;
    [SerializeField]
    private float sprintSpeed = 8;

    //Camera rotation sensitivity
    [SerializeField]
    private float horizontalSensitivity = 2;
    [SerializeField]
    private float verticalSensitivity = 2;

    //Jump Force
    [SerializeField]
    private float jumpForce = 400;

    public bool jumpDisabled = false;

    //player setup
    private PlayerSetup ps;

    //Player motor script reference
    private PlayerMotor pMotor;

    private PlayerInteract pInteract;

    private AudioSource jumpAndDeathSound;
    public AudioClip deathClip;
    public AudioClip jumpClip;

    public AudioClip music;
    public AudioSource musicSource;

    private Transform respawnPoint;

    public GameObject PauseMenu;

    private bool isDead = false;

	void Start () {
        ps = GetComponent<PlayerSetup>();
        pMotor = GetComponent<PlayerMotor>();
        pInteract = GetComponent<PlayerInteract>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        jumpAndDeathSound = GetComponent<AudioSource>();
        musicSource.clip = music;
        musicSource.Play();
    }

    public void SetRespawn(Transform newRespawnTransform)
    {
        respawnPoint = newRespawnTransform;
    }

    // Update is called once per frame
    void Update () {


        if (Input.GetButtonDown("Pause"))
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                PauseMenu.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PauseMenu.SetActive(true);
            }
        }

        if (isDead)
            return;
        



        //Setup player movement
        float movX = Input.GetAxisRaw("Horizontal");
        float movZ = Input.GetAxisRaw("Vertical");

        Vector3 horizontalMov = transform.right * movX;
        
        Vector3 verticalMov = transform.forward * movZ;
        
        Vector3 newPlayerMovement = horizontalMov + verticalMov;


        if (Input.GetButton("Sprint"))
        {
            newPlayerMovement *= sprintSpeed;
            
        }
        else
        {
            newPlayerMovement *= walkingSpeed;

        }


        //final calculation before applying movement
        //Send player movement to the PlayerMotor to apply movement
        pMotor.MovePlayer(newPlayerMovement);


        //Setup player rotation
        float rotHorizontal = Input.GetAxis("Mouse X") * horizontalSensitivity;

        float rotVertical= -Input.GetAxis("Mouse Y") * verticalSensitivity;

        //Send camera rotation to the PlayerMotor to apply movement
        pMotor.RotatePlayer(rotHorizontal, rotVertical);

        
        if (Input.GetButtonDown("Jump") && !jumpDisabled)
        {
            Jump();

        }

        //Interacting with objects
        if (Input.GetButtonDown("Interact"))
        {
            pInteract.CmdInteract();
        }


    }

    [Client]
    void Jump()
    {
        jumpDisabled = true;
        ps.emissionOn = true;
        pMotor.Jump(jumpForce);
        jumpAndDeathSound.clip = jumpClip;
        jumpAndDeathSound.Play();
    }

    [Command]
    void CmdSetEmissionState()
    {
        ps.RpcSetEmission(jumpDisabled);
    }


    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        jumpAndDeathSound.clip = deathClip;
        jumpAndDeathSound.Play();
        pMotor.enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
        isDead = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        transform.position = respawnPoint.position;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.rotation = respawnPoint.rotation;
        pMotor.enabled = true;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Ground")
        {
            ps.emissionOn = false;

            jumpDisabled = false;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        ps.emissionOn = true;
    }
}
