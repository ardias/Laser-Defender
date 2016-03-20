using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	GameObject enemyInstance;
	public float width = 10f; 
	public float height = 10f;
	public float SpawnDelay = 0.5f;

	float xmin = 0;
	float xmax = 0;
	bool movingLeft = true;
	public float speed = 5;
	// Use this for initialization
	void Start ()
	{
		float zdistance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zdistance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, zdistance));
		xmin = leftmost.x;
		xmax = rightmost.x;
		SpawnUntilFull();
	}

	void SpawnUntilFull()
	{
		Transform freePosition = NextFreePosition();
		if (freePosition != null)
		{
			enemyInstance = (GameObject)Instantiate(enemyPrefab, freePosition.position, Quaternion.identity);
			//set the enemy instance as a 'child' of the postion transform
			enemyInstance.transform.parent = freePosition;
		}
		//only 'schedule' another respwan if there is another position free (otherwise it would keep respawning forever) 
		//ie. stop respawing if it's full
		if (NextFreePosition() != null)
		{
			Invoke("SpawnUntilFull", SpawnDelay);
		} 

	}

	private void SpawnEnemies()
	{
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

		if(AllDead())
		{
			Debug.Log("Regenerating..");
			SpawnUntilFull();
		}
	}

	Transform NextFreePosition()
	{
		foreach (Transform childGameObject in transform)
		{
			if (childGameObject.childCount == 0)
			{
				return childGameObject;
			}
		}
		return null;
	}

	private bool AllDead()
	{
		//the idea here is that we loop over the children of the formation and
		//if none of the child transforms has a 'enemy prefab' then all enemies are dead
		//this is not very otpimized. we should probably get the count at the begining and
		//decrease everytime an enemy dies and just check the count > 0
		foreach (Transform childGameObject in transform)
		{
			if(childGameObject.childCount > 0)
			{
				return false;
			}
		}
		return true;
	}
}
