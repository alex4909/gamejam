using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceField : MonoBehaviour {

	public int maxhealth = 7;
	public int minhealth = 3;
	private int health;
	public GameObject projectile;
	public float enemyProjectileSpeed = 10f;
	public float laserRate = 0.0005f;
	public float changeSize = 0.1f;
	public int enemyScore = 150;
	public AudioClip fireSound;
	public AudioClip destroyedSound;
	public float pushDistance = 0.8f;

	private ScoreKeeper scoreKeeper;


	void Start(){
		health = Random.Range (minhealth, maxhealth);
	}

	void Update(){

	}


	void OnTriggerEnter2D(Collider2D collider){

		health -= 1;

		//ensure collisions with player laser only are registered
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			missile.Hit();

			float currenty = gameObject.transform.position.y;
			transform.position += Vector3.right * pushDistance;

			if (gameObject.tag == "target") {

				//resize everything
				gameObject.transform.localScale -= new Vector3 (changeSize,changeSize,changeSize);
				CircleCollider2D col = gameObject.GetComponent<CircleCollider2D> ();
				col.transform.localScale -= new Vector3 (changeSize,changeSize,changeSize);

				GameObject[] forceFields = GameObject.FindGameObjectsWithTag ("forcefield");
				foreach (GameObject forceField in forceFields) {
					float rescaleSize = changeSize / forceFields.Length;
					forceField.transform.localScale += new Vector3 (rescaleSize, rescaleSize, rescaleSize);
					CircleCollider2D col1 = forceField.GetComponent<CircleCollider2D>();
					col1.transform.localScale += new Vector3 (rescaleSize, rescaleSize, rescaleSize);

					//destroy if out of health
					if (health <= 0) {
						Destroy (gameObject);
					}

				}

			} else if (gameObject.tag == "forcefield") {

				//resize everything
				gameObject.transform.localScale += new Vector3 (changeSize,changeSize,changeSize);
				CircleCollider2D col = gameObject.GetComponent<CircleCollider2D> ();
				col.transform.localScale += new Vector3 (changeSize,changeSize,changeSize);

				GameObject[] forceFields = GameObject.FindGameObjectsWithTag ("forcefield");
				float rescaleSize = changeSize / forceFields.Length;
				GameObject target = GameObject.FindGameObjectWithTag ("target");
				target.transform.localScale -= new Vector3 (changeSize,changeSize,changeSize);
				CircleCollider2D col2 = target.GetComponent<CircleCollider2D>();
				col2.transform.localScale -= new Vector3 (changeSize,changeSize,changeSize);


			}
		}


	}





}
