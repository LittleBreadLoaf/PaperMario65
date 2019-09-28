using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MarioController : MonoBehaviour
{

    private CharacterController controller;
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
        controller = GetComponent<CharacterController>();
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
        Move(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.E))
            animator.SetBool("Hammer", true);
    }

    private void Move(float forwardMove, float sidewaysMove)
    {
        moveDirection = new Vector3(sidewaysMove, 0, forwardMove);
        moveDirection.Normalize();
        moveDirection = transform.TransformDirection(moveDirection);

        if (controller.isGrounded)
        {
            yVelocity = -0.1F;
            currentFloorPos = transform.position.y;
            isRunning = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical"))) == 0.0 ? false : true;
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                yVelocity = jumpSpeed;
        }
        else
        {
            moveDirection *= (1.0F * speed);
            yVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = yVelocity;
        controller.Move(moveDirection * Time.deltaTime);

        updateFlip = Input.GetAxis("Horizontal") == 0 ? false : true;
        if (updateFlip)
            renderer.flipX = Input.GetAxis("Horizontal") > 0 ? true : false;
        animator.SetBool("isRunning", isRunning);
        animator.SetFloat("yVelocity", moveDirection.y);
        animator.SetBool("Backwards", Input.GetAxis("Vertical") > 0 ? true : false);
        animator.SetBool("onGround", controller.isGrounded);
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