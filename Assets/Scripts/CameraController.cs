using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Camera cam;
	private CharacterController mario;
	private Vector3 initialCamDistance = Vector3.zero;
	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
		mario = GameObject.Find ("Mario").GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 screenpos = cam.WorldToScreenPoint (mario.transform.position);

        if (MarioController.sceneStarted)
        {
            if(initialCamDistance == Vector3.zero)
            {

                initialCamDistance = mario.transform.position - cam.transform.position;
            }
            if (mario.isGrounded)
            {
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, mario.transform.position - initialCamDistance, Time.deltaTime * MarioController.speed);
                //if you want to match speed, use mario.velocity.magnitude
            }
            else
            {
                if (mario.transform.position.y < MarioController.currentFloorPos)
                {
                    cam.transform.position = Vector3.MoveTowards(cam.transform.position, mario.transform.position - initialCamDistance, Time.deltaTime * mario.velocity.magnitude);
                }
                else
                {
                    Vector3 newLoc = Vector3.MoveTowards(cam.transform.position, mario.transform.position - initialCamDistance, Time.deltaTime * MarioController.speed);
                    cam.transform.position = new Vector3(newLoc.x, cam.transform.position.y, newLoc.z);
                }

            }
        }
	}
}
