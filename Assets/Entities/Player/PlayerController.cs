using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	public GameObject Projectile;
	public float PlayerSpeed = 1f;
	public float ProjectileSpeed = 1f;
	public float ProjectileRate = 1f;
	public int Health;

	float shipSpeed;
	float laserSpeed;
	float xmin = 0;
	float xmax = 0;
	//float ymax = 0;
	float padding = 0.5f;
	

	// Use this for initialization
	void Start () {

		//calc the distance from the object to the camera
		float zdistance = transform.position.z - Camera.main.transform.position.z;
		//get the world limits
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zdistance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, zdistance));
		Vector3 topMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, zdistance));
		xmin = leftmost.x + padding; 
		xmax = rightmost.x - padding;
		//ymax = topMost.y;
		shipSpeed = PlayerSpeed;
		laserSpeed = ProjectileSpeed;
	}
	
	// Update is called once per frame
	void Update () {

		//this makes it independent of frame rate (?)
		float speed = shipSpeed * Time.deltaTime;
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			MoveLeft(speed);
		} else if (Input.GetKey(KeyCode.RightArrow))
		{
			MoveRight(speed);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			InvokeRepeating("Fire", 0.000001f, 1f/ProjectileRate);
		}
		if(Input.GetKeyUp(KeyCode.Space))
		{
			CancelInvoke("Fire");
		}
	}

	private void Fire()
	{
		//shoudl extract to it's own class
		GameObject bullet = (GameObject)Instantiate(Projectile, transform.position, Quaternion.identity);
		bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * laserSpeed; // * (Time.deltaTime * 1000) ;
	}

	private void MoveLeft(float speed)
	{
		float leftIncrement = Vector3.left.x * speed;
		float left = Mathf.Clamp(transform.position.x + leftIncrement, xmin, xmax);
		this.transform.position = new Vector3(left, transform.position.y, transform.position.z);
		
	}

	private void MoveRight(float speed)
	{
		float rightIncrement = Vector3.right.x * speed;
		float left = Mathf.Clamp(transform.position.x + rightIncrement, xmin, xmax);
		this.transform.position = new Vector3(left, transform.position.y, transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		Projectile missile = collider.gameObject.GetComponent<Projectile>(); 
		if(missile)
		{
			Health -= missile.Damage;
			if (Health == 0)
			{
				Destroy(gameObject);
			}
		}
		
	}
}
