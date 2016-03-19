using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {


	public float Damage;

	public void Hit()
	{
		Destroy(gameObject);
	}
}
