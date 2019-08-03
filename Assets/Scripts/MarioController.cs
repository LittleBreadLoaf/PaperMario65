using UnityEngine;
using System.Collections;

public class MarioController : MonoBehaviour {

	private CharacterController controller;
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
	void Start () {
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		renderer = GetComponent<SpriteRenderer>();
	}

	void FixedUpdate(){
	}

	// Update is called once per frame
	void Update () {
		
		moveDirection = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		moveDirection.Normalize ();
		moveDirection = transform.TransformDirection (moveDirection);

		if (controller.isGrounded) {
			yVelocity = -0.1F;
			currentFloorPos = transform.position.y;
			isRunning = (Mathf.Abs (Input.GetAxisRaw ("Horizontal")) + Mathf.Abs (Input.GetAxisRaw ("Vertical"))) == 0.0 ? false : true; 
			moveDirection *= speed;
			if (Input.GetButton ("Jump"))
				yVelocity = jumpSpeed;
			if(Input.GetKeyDown (KeyCode.E))
				animator.SetBool ("Hammer", true);
		} else {
			moveDirection *= (1.0F * speed);
			yVelocity -= gravity * Time.deltaTime;
		}
		
		moveDirection.y = yVelocity;
		controller.Move(moveDirection * Time.deltaTime);

		updateFlip = Input.GetAxis ("Horizontal") == 0 ? false : true;
		if(updateFlip)
			renderer.flipX = Input.GetAxis ("Horizontal") > 0 ? true : false;
		animator.SetBool ("isRunning", isRunning);
		animator.SetFloat ("yVelocity", moveDirection.y);
		animator.SetBool ("Backwards", Input.GetAxis ("Vertical") > 0 ? true : false);
		animator.SetBool ("onGround", controller.isGrounded);


	}
}
