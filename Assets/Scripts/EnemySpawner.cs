//ISSUE - NOT RESPAWNING ALL ENEMIESWHEN ALL DESTROYED. no idea why the suggested code should actually work

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public GameObject thrusterPrefab;

	//height and width of formation
	public float width=10;
	public float height = 5;
	public float speed = 10f;
	public float spawnDelay = 0.5f;

	private float direction = 1f;
	private float xmax;
	private float xmin;

	// Use this for initialization
	void Start () {

		//define playspace
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distanceToCamera));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distanceToCamera));
		xmax = rightmost.x;
		xmin = leftmost.x;

		SpawnUntilFull ();

	}
		

	//gizmo to show formationin editor

	void OnDrawGizmos(){

		Gizmos.DrawWireCube(transform.position,new Vector3(width,height,1));
	}




	// Update is called once per frame
	void Update () {

		moveLeftRight ();
		if (AllMembersDead()) {
			SpawnUntilFull ();
		}

	}



	void SpawnEnemies(){
		//define array of child transforms
		Transform[] Children = GetComponentsInChildren<Transform>();

		//for every child in the parent transform
		foreach (Transform child in Children) {
			//spawn a new enemy by instantiating an enemy object
			GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			//enemy transform is same as parents' transform (ie enemy is child of position)
			enemy.transform.parent = child;

			//spawn thruster particle stream
			GameObject thruster=Instantiate(thrusterPrefab,enemy.transform.position,Quaternion.identity) as GameObject;
			thruster.transform.parent = enemy.transform;
		}
	}

	//spawns enemies one by one
	void SpawnUntilFull(){
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			//enemy transform is same as parents' transform (ie enemy is child of position)
			enemy.transform.parent = freePosition;

			//spawn thruster particle stream
			GameObject thruster=Instantiate(thrusterPrefab,enemy.transform.position,Quaternion.identity) as GameObject;
			thruster.transform.parent = enemy.transform;
		}

		//only spawn if free position available (recursive)
		if(NextFreePosition()){
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}


	//function to move enemies left and right
	void moveLeftRight(){
		if(transform.position.x >= xmax - (width/2)){
			direction =-1;
		}

		else if(transform.position.x <= xmin + (width/2)){
			direction =1;
		}

		transform.position += Vector3.right*speed*Time.deltaTime*direction;
	}

	//function to determine whether all members are dead
	bool AllMembersDead(){
			foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
				return false;
			}
		}
		//outside of loop - only true if no children found
				return true;
	}
		

	//function to return next free position for enemy to respawn in
	Transform NextFreePosition(){
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0) {
				return childPositionGameObject;
			}
		}
		return null;
	}

}
