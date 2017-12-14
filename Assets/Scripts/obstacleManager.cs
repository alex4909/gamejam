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
		obstacles = (GameObject.FindGameObjectsWithTag("forcefield")).ToList();
		target = GameObject.FindGameObjectWithTag("target");
		obstacles.Add (target);///
		obstacles = obstacles.OrderBy (go => go.transform.position.y.ToArray ());
		for (int i =0 ;i < obstacles.Count;i ++) {

			if (i == 0) {
				obstacles [i].transform.position.y = downmost + obstacles [i].transform.localScale.y;
			}
		}
	}
}
