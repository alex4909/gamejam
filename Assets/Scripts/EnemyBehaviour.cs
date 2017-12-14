using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public float health = 150;
	public GameObject projectile;
	public float enemyProjectileSpeed = 10f;
	public float laserRate = 0.0005f;
	public int enemyScore = 150;
	public AudioClip fireSound;
	public AudioClip destroyedSound;

	private ScoreKeeper scoreKeeper;


	void Start(){
		// call in a scorekeeper object
		scoreKeeper=GameObject.Find ("Score").GetComponent<ScoreKeeper>();
	}

	void Update(){

		if (Time.deltaTime * Random.value <= laserRate) {
			fireLaser ();
		}
	}


	void OnTriggerEnter2D(Collider2D collider){
		
		//ensure collisions with player laser only are registered
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage();
			missile.Hit();
		}

		if (health <= 0) {
			AudioSource.PlayClipAtPoint (destroyedSound,transform.position);
			Destroy (gameObject);
			//update thescore if an enemy is destroyed
			scoreKeeper.Score (enemyScore);
		}
}



	void fireLaser(){
		GameObject enemyBeam = Instantiate(	projectile, transform.position,Quaternion.identity) as GameObject;
		enemyBeam.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, -enemyProjectileSpeed, 0);
		AudioSource.PlayClipAtPoint (fireSound,transform.position);
	}

		
}
