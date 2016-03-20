using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public int Damage;

	public void Hit()
	{
		Destroy(gameObject);
	}
}
