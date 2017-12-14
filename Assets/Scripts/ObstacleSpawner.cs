﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

	public GameObject forceField;
	private float screenHeight;
	Vector3 downmost;
	Vector3 upmost; 
	float distance;
	public float defaultx = 12f;
	public float speed = 5f;
	public Sprite targetSprite;

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

		GameObject[] currentForcefields= GameObject.FindGameObjectsWithTag ("forcefield");
		if (currentForcefields.Length == 0) {
			
			SpawnFixedObstacles (5f, screenHeight, defaultx, downmost.y, upmost.y);
			SpawnObstacles (5f, screenHeight, defaultx, downmost.y, upmost.y);
			AssignTarget ();
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
}
