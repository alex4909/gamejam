using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetParticles : MonoBehaviour {


	public Material[] planetMaterials;
	private Material mat;
	// Use this for initialization
	void Start () {
		mat = gameObject.GetComponent<ParticleSystemRenderer> ().material;
			}
	
	// Update is called once per frame
	void Update () {
		mat = planetMaterials [Random.Range (0, 3)];
	}
}
