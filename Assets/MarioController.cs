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
		if (controller.isGrounded) {
			currentFloorPos = transform.position.y;
			moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			moveDirection = transform.TransformDirection (moveDirection);
			moveDirection *= speed;
			if (Input.GetButton ("Jump"))
				moveDirection.y = jumpSpeed;

		}
		updateFlip = Input.GetAxis ("Horizontal") == 0 ? false : true;
		if(updateFlip)
			renderer.flipX = Input.GetAxis ("Horizontal") > 0 ? true : false;
		isRunning = (Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z)) * speed == 0.0 ? false : true; 
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
		animator.SetBool ("isRunning", isRunning);
		animator.SetFloat ("yVelocity", moveDirection.y);
		animator.SetBool ("Backwards", Input.GetAxis ("Vertical") > 0 ? true : false);
		animator.SetBool ("onGround", controller.isGrounded);
		if(Input.GetKeyDown (KeyCode.E))
			animator.SetBool ("Hammer", true);

	}
}
