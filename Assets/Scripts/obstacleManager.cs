using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


//script to manage the position of the obstacles


public class obstacleManager : MonoBehaviour {


	private float screenHeight;
	Vector3 downmost;
	Vector3 upmost; 
	float distance;
	private List<GameObject> obstacles;
	private GameObject target;

	// Use this for initialization
	void Start () {
		//get z distance from camera to player
		distance = transform.position.z - Camera.main.transform.position.z;
		//get up limit by calculating in worldpoints from camera view
		upmost = Camera.main.ViewportToWorldPoint(new Vector3(0,1,distance));
		//get down limit
		downmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		screenHeight = upmost.y - downmost.y;

	}
	
	// Update is called once per frame
	void Update () {
		Anchor ();
		ReArrange ();
	}

	//anchor top and bottom forcefields
	void Anchor(){
		GameObject[] forcefields = GameObject.FindGameObjectsWithTag ("forcefield");
		GameObject target = GameObject.FindGameObjectWithTag ("target");
		forcefields = AddItemToArray (forcefields, target);
		forcefields = forcefields.OrderBy(go => go.transform.position.y).ToArray();

		float bottomY = downmost.y + (forcefields[0].transform.localScale.y / 2);
		//float topY = upmost.y - (forcefields [forcefields.Length-1].transform.localScale.y / 2);

		forcefields [0].transform.position = new Vector3 (forcefields [0].transform.position.x, bottomY, forcefields [0].transform.position.z);
		//forcefields [forcefields.Length-1].transform.position = new Vector3 (forcefields [forcefields.Length-1].transform.position.x, topY, forcefields [forcefields.Length-1].transform.position.z);

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
}
