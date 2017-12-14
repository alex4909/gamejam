using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

	public GameObject forceField;
	private float screenHeight;
	public float defaultx = 6f;
	public float speed = 5f;

	// Use this for initialization
	void Start () {
		//get z distance from camera to player
		float distance = transform.position.z - Camera.main.transform.position.z;
		//get up limit by calculating in worldpoints from camera view
		Vector3 upmost = Camera.main.ViewportToWorldPoint(new Vector3(0,1,distance));
		//get down limit
		Vector3 downmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		screenHeight = upmost.y - downmost.y;

		SpawnFixedObstacles (5f, screenHeight, defaultx, downmost.y, upmost.y);
		SpawnObstacles (5f, screenHeight, defaultx, downmost.y, upmost.y);

	}

	// Update is called once per frame
	void Update () {
		
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
}
