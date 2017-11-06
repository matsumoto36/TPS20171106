using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private ScoreScript ss;
	public int HP;
	public int score;

	// Use this for initialization
	void Start () {
        ss = GameObject.Find("GameRoot").GetComponent<ScoreScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Bullet") {
			if(--HP <= 0) {
				Destroy(gameObject);
                //スコアを足す
                ss.score += score;
			}
			Destroy(other.gameObject);
		}
	}
}
