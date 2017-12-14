using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserShredder : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col){

		Projectile missile = col.gameObject.GetComponent<Projectile> ();
		if (missile){
		Destroy(col.gameObject);
	}
}
}
