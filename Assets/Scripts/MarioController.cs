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

    private bool facingRight = false;
    private bool playerControl = true;
    private bool playerAttacking = false;
    private float animationLength;

    public Material mat;
    private GameObject shape;

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

        HitCheck();

        //Will likely need to make this it's own script, or at least move most of the logic out of Update()
        if(playerControl)
        {
            Move(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));

            if (Input.GetKeyDown(KeyCode.E) && controller.isGrounded)
            {
                animator.SetBool("Hammer", true);
                StartCoroutine(SetMarioControl(false, true, GetAnimationClipLength("Hammer")));



            }
                
        }
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
        {
            renderer.flipX = Input.GetAxis("Horizontal") > 0 ? true : false;
            facingRight = Input.GetAxis("Horizontal") > 0 ? true : false;
        }
            
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
            Debug.Log("Touched a Goombs");
            StartCoroutine(SetMarioControl(false, false, 2.0f));
        }
    }

    public bool GetPlayerControlState()
    {
        return playerControl;
    }

    //Coroutine to add in a delay variable
    IEnumerator SetMarioControl(bool state, bool isAttackAnim, float delay)
    {
        //Remove player control
        playerControl = state;
        if (isAttackAnim)
            playerAttacking = true;

        //Wait for delay-length of time
        yield return new WaitForSeconds(delay);

        //Return player control
        playerControl = !state;
        if (isAttackAnim)
            playerAttacking = false;
    }


    //Get time length of animation clips
    float GetAnimationClipLength(string animationName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
                return clip.length;
        }
        return 0.0f;
    }

    void HitCheck()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position + controller.center + Vector3.up * -controller.height * 0.5F;
        Vector3 p2 = p1 + Vector3.up * controller.height;
        float distanceToObstacle = 0;
        float range = 0.5f;

        if (facingRight)
        {

            // Cast character controller shape 10 meters forward to see if it is about to hit anything.
            if (Physics.CapsuleCast(p1, p2, controller.radius, transform.right, out hit, range))
                distanceToObstacle = hit.distance;

            //Visualize cast hitbox
            //RenderCastVolume(p1, p2, controller.radius, transform.right, range);

 
        }
        //If facing LEFT
        else
        {
            // Cast character controller shape 10 meters forward to see if it is about to hit anything.
            if (Physics.CapsuleCast(p1, p2, controller.radius, -transform.right, out hit, range))
                distanceToObstacle = hit.distance;

            //Visualize cast hitbox
            //RenderCastVolume(p1, p2, controller.radius, -transform.right, range);

            //Debug.Log(distanceToObstacle);
        }
    }

    //Used to visualize CapsuleCast hitbox
    void RenderCastVolume(Vector3 p1, Vector3 p2, float radius, Vector3 dir, float distance)
    {
        if (!shape)
        {
            shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(shape.GetComponent<Collider>());
            shape.GetComponent<Renderer>().material = mat;
        }
        Vector3 scale;
        float diam = 2 * radius;
        scale.x = diam;
        scale.y = Vector3.Distance(p2, p1) + diam;
        scale.z = distance + diam;
        shape.transform.localScale = scale;
        shape.transform.position = (p1 + p2 + dir.normalized * distance) / 2;
        shape.transform.rotation = Quaternion.LookRotation(dir, p2 - p1);
    }

}