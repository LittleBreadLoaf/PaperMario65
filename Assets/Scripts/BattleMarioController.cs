using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleMarioController : MonoBehaviour
{

    private BoxCollider collisionBox;
    private Animator animator;
    private SpriteRenderer renderer;
    public static float speed = 2.0F;
    public static float jumpSpeed = 5.0F;
    public static float gravity = 20.0F;
    public static float currentFloorPos = 0.275F;
    private bool isRunning = false;
    private bool updateFlip = false;
    private Vector3 moveDirection = Vector3.zero;
    private float yVelocity = 0;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        collisionBox = GetComponent<BoxCollider>();

    }

    void FixedUpdate()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SceneManager.LoadScene("Debug_Map");
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //TODO: Change this to be the "Enemy" superclass, pass in params for battle.
        //Maybe move this logic into that enemy superclass
        if (hit.gameObject.name.Equals("Goomba"))
        {
            SceneManager.LoadScene("Debug_Fight_Scene");
        }
    }
}