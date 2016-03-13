using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	public GameObject Projectile;
	public float ShipSpeed = 10;
	public float BulletSpeed = 10;

	float shipRealSpeed;
	float bulletRealSpeed;
	float xmin = 0;
	float xmax = 0;
	float ymax = 0;
	float padding = 0.5f;

	ArrayList bullets;
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
		ymax = topMost.y;
		ShipSpeed = Mathf.Clamp(ShipSpeed, 1f, 100f);
		shipRealSpeed = ShipSpeed * 0.2f;
		BulletSpeed = Mathf.Clamp(BulletSpeed, 1f, 100f);
		bulletRealSpeed = BulletSpeed * 0.2f;
		bullets = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {

		//this makes it independent of frame rate (?)
		float speed = shipRealSpeed * Time.deltaTime;
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			MoveLeft(speed);
		} else if (Input.GetKey(KeyCode.RightArrow))
		{
			MoveRight(speed);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			//shoudl extract to it's own class
			GameObject bullet = (GameObject)Instantiate(Projectile, transform.position, Quaternion.identity);
			bullets.Add(bullet);
		}

		MoveBullets(bullets);
	}

	private void MoveBullets(ArrayList bullets)
	{
		IEnumerator bs = bullets.GetEnumerator();
		while(bs.MoveNext())
		{
			GameObject bullet = bs.Current as GameObject;
			bullet.transform.position += Vector3.up * bulletRealSpeed;
			if (bullet.transform.position.y > ymax)
			{
				Destroy(bullet);
				bullets.Remove(bullet);
			}
		}
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
}
