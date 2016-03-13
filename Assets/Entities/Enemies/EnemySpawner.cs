using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	GameObject enemyInstance;
	public float width = 10f;
	public float height = 10f;

	float xmin = 0;
	float xmax = 0;
	bool movingLeft = true;
	public float speed = 5;
	// Use this for initialization
	void Start () {
		float zdistance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zdistance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, zdistance));
		xmin = leftmost.x;
		xmax = rightmost.x;
		foreach (Transform child in transform)
		{
			enemyInstance = (GameObject)Instantiate(enemyPrefab, child.transform.position, Quaternion.identity);
			//set the enemy instance as a 'child' of the postion transform
			enemyInstance.transform.parent = child;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(this.transform.position, new Vector3(width, height));
	}

	// Update is called once per frame
	void Update()
	{

		//speed * deltatime -> move independent of frame rate
		if (movingLeft)
		{
			var leftEdgeOfFormation = this.transform.position.x - (width * 0.5f);
			//did we reach the left edge? if yes, just invert, do not update the position more
			if (leftEdgeOfFormation < xmin)
			{
				movingLeft = false;
			} else { 
				this.transform.position += Vector3.left * speed * Time.deltaTime;
			}
		}
		else
		{
			var rightEdgeOfFormation = this.transform.position.x + (width * 0.5f);
			//did we reach the right edge? if yes, just invert and do not update the position
			if (rightEdgeOfFormation > xmax)
			{
				movingLeft = true;
			} else {
				this.transform.position += Vector3.right * speed * Time.deltaTime;
			}
		}
	}
}
