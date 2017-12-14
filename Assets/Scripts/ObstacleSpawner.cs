using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstacleSpawner : MonoBehaviour {

	public GameObject forceField;
	private float screenHeight;
	Vector3 downmost;
	Vector3 upmost; 
	Vector3 leftmost;
	Vector3 rightmost;
	float distance;
	public int maxObstacles=5;
	public int minObstacles=3;
	public float defaultxSpawn;
	public float speed = 5f;
	//TODO make speed vary with time
	public Sprite targetSprite;
	public GameObject Asteroid;
	public float minspin=0.5f;
	public float maxspin=2f;

	// Use this for initialization
	void Start () {
		//get z distance from camera to player
		distance = transform.position.z - Camera.main.transform.position.z;
		//get up limit by calculating in worldpoints from camera view
		upmost = Camera.main.ViewportToWorldPoint(new Vector3(0,1,distance));
		//get down limit
		downmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));

		leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		//get down limit
		rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		screenHeight = upmost.y - downmost.y;

		defaultxSpawn = rightmost.x + 0.5f;


	}

	// Update is called once per frame
	void Update () {

		GameObject[] currentForcefields= GameObject.FindGameObjectsWithTag ("forcefield");
		if (currentForcefields.Length == 0) {
			int numberOfObstacles = Random.Range (minObstacles, maxObstacles);
			SpawnFixedObstacles (numberOfObstacles, screenHeight, defaultxSpawn, downmost.y, upmost.y);
			SpawnObstacles (numberOfObstacles, screenHeight, defaultxSpawn, downmost.y, upmost.y);
			AssignTarget ();
			SpawnAsteroids ();
			speed += 0.2f;
		}
	}

//spawns the top and bottom obstacles
	void SpawnFixedObstacles(float numberOfAsteroids , float playHeight, float defaultx, float downmost, float upmost){
		float diameter = playHeight / (numberOfAsteroids);
		GameObject bottomField = Instantiate(forceField,new Vector3(defaultx,downmost+(diameter/2),0f),Quaternion.identity) as GameObject;
		bottomField.transform.localScale = new Vector3 (diameter, diameter, diameter);
		bottomField.GetComponent<Rigidbody2D>().velocity = new Vector3 (-speed, 0, 0);
		GameObject topField = Instantiate(forceField,new Vector3(defaultx,upmost-(diameter/2),0f),Quaternion.identity) as GameObject;
		topField.transform.localScale = new Vector3 (diameter, diameter, diameter);
		topField.GetComponent<Rigidbody2D>().velocity = new Vector3 (-speed, 0, 0);

	}

	void SpawnObstacles(float numberOfAsteroids,float playHeight, float defaultx, float downmost, float upmost){

		GameObject[] fixedObstacles = GameObject.FindGameObjectsWithTag ("forcefield");
		//combines size of the 2 fixed obstacles
		float fixedDiameter = fixedObstacles[0].transform.localScale.y + fixedObstacles[1].transform.localScale.y;
		float diameter = (playHeight - 2*( playHeight / (numberOfAsteroids)))/(numberOfAsteroids-2); //TODO - change to be variable
		for (int i = 0; i < numberOfAsteroids - 2; i++) {
			GameObject field = Instantiate(forceField,new Vector3(defaultx,upmost-(diameter/2) -((i+1)*diameter),0),Quaternion.identity);
			field.transform.localScale = new Vector3 (diameter, diameter, diameter);
			field.GetComponent<Rigidbody2D>().velocity = new Vector3 (-speed, 0, 0);
		}
	}

	//randomly assign one forcefield as the target
	void AssignTarget(){
		GameObject[] forcefields = GameObject.FindGameObjectsWithTag ("forcefield");
		int targetIndex = Random.Range (0, forcefields.Length - 1);
		forcefields [targetIndex].tag = "target";
		SpriteRenderer sr = forcefields [targetIndex].GetComponent<SpriteRenderer> ();
		sr.sprite = targetSprite;
	}


	void SpawnAsteroids(){
		GameObject[] forcefields = GameObject.FindGameObjectsWithTag ("forcefield");
		GameObject target = GameObject.FindGameObjectWithTag ("target");
		forcefields = AddItemToArray (forcefields, target);

		foreach (GameObject forcefield in forcefields) {
			var asteroid = Instantiate (Asteroid, new Vector3 (forcefield.transform.position.x, forcefield.transform.position.y, forcefield.transform.position.z), Quaternion.identity);
			asteroid.transform.parent = forcefield.transform;
			Rigidbody rb = asteroid.GetComponent<Rigidbody> ();
			rb.angularVelocity =  new Vector3 (Random.Range(minspin,maxspin),Random.Range(minspin,maxspin), Random.Range(minspin,maxspin));
		}
	}


	private GameObject[] AddItemToArray (GameObject[] original, GameObject itemToAdd) {
		GameObject[] finalArray = new GameObject[ original.Length + 1 ];
		for(int i = 0; i<original.Length; i ++ ) {
			finalArray[i] = original[i];
		}
		finalArray[finalArray.Length - 1] = itemToAdd;
		return finalArray;
	}



}
