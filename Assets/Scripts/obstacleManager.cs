using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//script to manage the position of the obstacles


public class obstacleManager : MonoBehaviour {


	private float screenHeight;
	Vector3 downmost;
	Vector3 upmost;
	Vector3 leftmost;
	Vector3 rightmost;
	float distance;
	private List<GameObject> obstacles;
	private GameObject target;
	private float time;
	private float planetTime;
	public float minPlanetTime=1f;
	public float maxPlanetTime=3f;
	public Sprite[] planetSprites;
	public Sprite[] starSprites;
	public GameObject planet;
	public float minPlanetSpeed=2f;
	public float maxPlanetSpeed = 5f;
	public float minPlanetSize = 0.4f;
	public float maxPlanetSize=2f;
	private float starTime;
	private float nextStarSpawnTime;
	public float minstarSpawnTime=0f;
	public float maxstarSpawnTime=0.1f;
	private float starSpawnTime;
	public float minStarSpeed=6f;
	public float maxStarSpeed = 15f;
	public float minStarSize = 0.8f;
	public float maxStarSize=0.3f;

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
		time = 0f;
		planetTime = 0f;
		starSpawnTime = 0f;

		//spawn planets to initially populate screen
		for (int i = 0; i < 50; i++) {
			SpawnStar (Random.Range (leftmost.x, rightmost.x));
		}

		for (int i = 0; i < 5; i++) {
			SpawnPlanet (Random.Range (leftmost.x, rightmost.x));
		}

	}
	
	// Update is called once per frame
	void Update () {
		

		if (time > planetTime) {
			SpawnPlanet (rightmost.x+0.5f);
			planetTime = Random.Range (minPlanetTime, maxPlanetTime);
			time = 0f;
		}

		if (starTime > starSpawnTime) {
			SpawnStar (rightmost.x+0.5f);
			nextStarSpawnTime = Random.Range (minstarSpawnTime, maxstarSpawnTime);
			starTime = 0f;
		}

		starTime += Time.deltaTime;
		time += Time.deltaTime;


		Anchor ();
		ReArrange ();
	}

	//anchor top and bottom forcefields
	void Anchor(){
		GameObject[] forcefields = GameObject.FindGameObjectsWithTag ("forcefield");
		GameObject target = GameObject.FindGameObjectWithTag ("target");
		forcefields = AddItemToArray (forcefields, target);
		if(forcefields.Length > 0){
		forcefields = forcefields.OrderBy(go => go.transform.position.y).ToArray();

		float bottomY = downmost.y + (forcefields[0].transform.localScale.y / 2);
		//float topY = upmost.y - (forcefields [forcefields.Length-1].transform.localScale.y / 2);

		forcefields [0].transform.position = new Vector3 (forcefields [0].transform.position.x, bottomY, forcefields [0].transform.position.z);
		//forcefields [forcefields.Length-1].transform.position = new Vector3 (forcefields [forcefields.Length-1].transform.position.x, topY, forcefields [forcefields.Length-1].transform.position.z);
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

	//adjust field y positions correctly
	void ReArrange(){
		GameObject[] forcefields = GameObject.FindGameObjectsWithTag ("forcefield");
		GameObject target = GameObject.FindGameObjectWithTag ("target");
		forcefields = AddItemToArray (forcefields, target);
		forcefields = forcefields.OrderBy(go => go.transform.position.y).ToArray();

		for (int i = 1; i < forcefields.Length-1; i++) {
			float xpos = forcefields [i].transform.position.x;
			float ypos = forcefields [i].transform.position.y;

			//position of next element
			float nextxpos = forcefields [i+1].transform.position.x;
			float nextypos = forcefields [i+1].transform.position.y;

			float deltax = nextxpos - xpos;

			float radius = forcefields [i].transform.localScale.x/2;
			float nextradius = forcefields [i + 1].transform.localScale.x/2;

			float deltay = Mathf.Sqrt (Mathf.Pow((radius + nextradius) , 2) - Mathf.Pow(deltax , 2));
			float newnextypos = deltay + forcefields [i].transform.position.y; 

			//reposition next element
			forcefields[i+1].transform.position=new Vector3(forcefields[i+1].transform.position.x,newnextypos,forcefields[i+1].transform.position.z);


		}

	}

	void SpawnPlanet(float xpos){

		GameObject newPlanet = Instantiate (planet, new Vector3 (xpos, Random.Range(downmost.y,upmost.y), 2f), Quaternion.identity);
		float planetXVelocity = Random.Range (minPlanetSpeed, maxPlanetSpeed);
		newPlanet.GetComponent<Rigidbody2D> ().velocity = new Vector3 (-planetXVelocity, 0, 0f);
		float planetSize = Random.Range (minPlanetSize, maxPlanetSize);
		newPlanet.transform.localScale = new Vector3 (planetSize, planetSize, planetSize);

		SpriteRenderer spriteRenderer = newPlanet.GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite= planetSprites [Random.Range(0,planetSprites.Length-1)];
	}

	void SpawnStar(float xpos){

		GameObject newStar = Instantiate (planet, new Vector3 (xpos, Random.Range(downmost.y,upmost.y), 3f), Quaternion.identity);
		float starXVelocity = Random.Range (minStarSpeed, maxStarSpeed);
		newStar.GetComponent<Rigidbody2D> ().velocity = new Vector3 (-starXVelocity, 0, 0f);
		float starSize = Random.Range (minStarSize, maxStarSize);
		newStar.transform.localScale = new Vector3 (starSize, starSize, starSize);

		SpriteRenderer spriteRenderer = newStar.GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite= starSprites [Random.Range(0,starSprites.Length-1)];
		
	}
}
