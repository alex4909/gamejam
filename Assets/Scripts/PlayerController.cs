using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//ship speed
	public float speed = 15f;
	public float padding = 0f;
	public float projectileSpeed=10f;
	public float fireRate=0.2f;
	public float health = 500;
	public AudioClip fireSound;
	public AudioClip loseSound;

	public GameObject projectile;
	public GameObject thrusterPrefab;

	//x limits
	float ymin;
	float ymax;

	// Use this for initialization
	void Start () {
		//get z distance from camera to player
		float distance = transform.position.z - Camera.main.transform.position.z;

		//get left limit by calculating in worldpoints from camera view
		Vector3 upmost = Camera.main.ViewportToWorldPoint(new Vector3(0,1,distance));

		//get right limit
		Vector3 downmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
	
		ymax = upmost.y-padding;
		ymin = downmost.y+padding;


		//spawn thruster particle stream
		//GameObject thruster=Instantiate(thrusterPrefab,this.transform.position,Quaternion.identity) as GameObject;
		//thruster.transform.parent = this.transform;
	
	
	}
	
	// Update is called once per frame
	void Update () {

		MoveWithKeys ();

		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating("FireLaser",0.0001f,fireRate) ;
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("FireLaser");
		}


	}


	//collisions
	void OnCollisionEnter2D(Collision2D collider){


			AudioSource.PlayClipAtPoint (loseSound,transform.position);
			LevelManager man = GameObject.Find ("LevelManager").GetComponent<LevelManager> ();
			man.LoadLevel ("End Menu");
			Destroy (gameObject);

	}



	//move function
	void MoveWithKeys(){



		if (Input.GetKey (KeyCode.UpArrow)) {

			//update position - note time.deltatime makes it framerate independant
			transform.position +=  Vector3.up*speed*Time.deltaTime;
		}

		else if (Input.GetKey (KeyCode.DownArrow)) {
			transform.position -= Vector3.up*speed*Time.deltaTime;
		}
			
		//restrict player to gamespace
		float newY = Mathf.Clamp (transform.position.y, ymin, ymax);
		transform.position = new Vector3 (transform.position.x, newY, transform.position.z);
	}


 // fire function

	void FireLaser(){
		GameObject beam = Instantiate(projectile, transform.position,Quaternion.Euler(0,0,90)) as GameObject;
			beam.GetComponent<Rigidbody2D>().velocity = new Vector3 (projectileSpeed, 0, 0);
			AudioSource.PlayClipAtPoint (fireSound,transform.position);
	}

}
