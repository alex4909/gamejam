using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceField : MonoBehaviour {

	public float health = 150;
	public GameObject projectile;
	public float enemyProjectileSpeed = 10f;
	public float laserRate = 0.0005f;
	public float changeSize = 0.1f;
	public int enemyScore = 150;
	public AudioClip fireSound;
	public AudioClip destroyedSound;

	private ScoreKeeper scoreKeeper;


	void Start(){

	}

	void Update(){

	}


	void OnCollisionEnter2D(Collision2D collider){

		//ensure collisions with player laser only are registered
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			missile.Hit();

			float currenty = gameObject.transform.position.y;

			if (gameObject.tag == "target") {
				gameObject.transform.localScale -= new Vector3 (changeSize,changeSize,changeSize);
				CircleCollider2D col = gameObject.GetComponent<CircleCollider2D> ();
				col.transform.localScale -= new Vector3 (changeSize,changeSize,changeSize);

				GameObject[] forceFields = GameObject.FindGameObjectsWithTag ("forcefield");
				foreach (GameObject forceField in forceFields) {
					float rescaleSize = changeSize / forceFields.Length;
					forceField.transform.localScale += new Vector3 (rescaleSize, rescaleSize, rescaleSize);
					CircleCollider2D col1 = forceField.GetComponent<CircleCollider2D>();
					col1.transform.localScale += new Vector3 (rescaleSize, rescaleSize, rescaleSize);
					//TODO reposition here
				}

			} else if (gameObject.tag == "forcefield") {
				gameObject.transform.localScale += new Vector3 (changeSize,changeSize,changeSize);
				CircleCollider2D col = gameObject.GetComponent<CircleCollider2D> ();
				col.transform.localScale += new Vector3 (changeSize,changeSize,changeSize);

				GameObject[] forceFields = GameObject.FindGameObjectsWithTag ("forcefield");
				float rescaleSize = changeSize / forceFields.Length;
				GameObject target = GameObject.FindGameObjectWithTag ("target");
				target.transform.localScale -= new Vector3 (changeSize,changeSize,changeSize);
				CircleCollider2D col2 = target.GetComponent<CircleCollider2D>();
				col2.transform.localScale -= new Vector3 (rescaleSize, rescaleSize, rescaleSize);

			}
		}

		/*
		if (health <= 0) {
			AudioSource.PlayClipAtPoint (destroyedSound,transform.position);
			Destroy (gameObject);
			//update thescore if an enemy is destroyed
			scoreKeeper.Score (enemyScore);
		}*/
	}





}
