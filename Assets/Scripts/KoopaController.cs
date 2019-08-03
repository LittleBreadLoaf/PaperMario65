using UnityEngine;
using System.Collections;

public class KoopaController : MonoBehaviour {

	private CharacterController controller;
	private Animator animator;
	public float speed = 2.0F;
	public float jumpSpeed = 5.0F;
	public float gravity = 20.0F;
	private bool isRunning = false;
	private Vector3 moveDirection = Vector3.zero;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
	}

	void FixedUpdate(){
	}

	
	// Update is called once per frame
	void Update () {
		if (controller.isGrounded) {
			moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			moveDirection = transform.TransformDirection (moveDirection);
			moveDirection *= speed;
			if (Input.GetButton ("Jump"))
				moveDirection.y = jumpSpeed;

		}
		isRunning = (moveDirection.x + moveDirection.z) * speed == 0.0 ? false : true; 
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
		animator.SetBool ("isRunning", isRunning);

	}
}
