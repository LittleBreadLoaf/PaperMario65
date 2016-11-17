using UnityEngine;
using System.Collections;

public class GoombaController : MonoBehaviour {

	private Animator animator;
	private SpriteRenderer renderer;
	Random rand = new Random();
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	int idleAnimation = 0;
	void Update () {
		idleAnimation += Random.Range(1, 6);
		if (idleAnimation > 700) {
			idleAnimation = 0;
			animator.SetTrigger ("idle_change");
		}
	}
}
