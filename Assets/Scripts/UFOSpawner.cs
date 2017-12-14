using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOSpawner : MonoBehaviour {


	public GameObject UFOPrefab;


	//height and width of formation
	public float width=10;
	public float height = 5;
	public float speed = 5f;

	public float spawnRate=0.00001f;

	private float xmax;
	private float xmin;



	//gizmo to show formationin editor

	void OnDrawGizmos(){

		Gizmos.DrawWireCube(transform.position,new Vector3(width,height,1));
	}




	// Update is called once per frame
	void Update () {

		if (Time.deltaTime * Random.value <= spawnRate) {
			SpawnEnemies ();
		}

		foreach (Transform childPositionGameObject in transform) {
			moveRight (childPositionGameObject);
		}
	}



	void SpawnEnemies(){

			GameObject enemy = Instantiate (UFOPrefab, transform.position, Quaternion.identity) as GameObject;
			//enemy transform is same as parents' transform (ie enemy is child of position)
			enemy.transform.parent = transform;
	}




	//function to move enemy right
	void moveRight(Transform UFO){
		UFO.transform.position += Vector3.right*speed*Time.deltaTime;
	}


}
