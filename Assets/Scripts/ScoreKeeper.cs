using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

	public static int totalScore=0;
	private Text myText;


	void Start(){
		myText=GetComponent<Text>();
		Reset ();
	}
	//note the functions are public since they are called from elsewhere
	public void Score(int points){
		totalScore += points;
	myText.text=totalScore.ToString();
}

	public static void Reset(){
		totalScore = 0;
		//myText.text=totalScore.ToString();
	}
}
