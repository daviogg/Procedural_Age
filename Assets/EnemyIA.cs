using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour {
	public GameObject bulletPrefab;
	private Vector3 playerPosition;
	private Vector3 enemyPosition;
	private GameObject player;
	private bool playerOnRange = false;

	public float speed = 5;
	private float timer = 0;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer >= 4 && playerOnRange){
			Fire();
			timer = 0;
		}
	}

	private void Fire(){
		playerPosition = player.transform.position;
		enemyPosition  = transform.position;

		GameObject bullet = Instantiate(bulletPrefab, enemyPosition, Quaternion.identity);
		bullet.GetComponent<Rigidbody2D>().AddForce((playerPosition - enemyPosition).normalized * speed * Time.deltaTime, ForceMode2D.Impulse);
	}

	void OnTriggerEnter2D(Collider2D collision){
		Debug.Log("Enter");
		if(collision.tag == "Player"){
			playerOnRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D collision){
		if(collision.tag == "Player"){
			playerOnRange = false;
		}
	}
}
