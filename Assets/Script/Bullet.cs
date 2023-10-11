using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlayerController Owner;
	private float speed = 30f;

	private float timeAlive = 1;

	public void ResetMod(Bullet bullet)
	{
		timeAlive = 0f;
	}

	public void Move()
	{
		transform.position += transform.forward * (speed * timeAlive * timeAlive * timeAlive) * Time.deltaTime;
		timeAlive += Time.deltaTime;
	}

    private void Update()
    {
		Move();
		if(timeAlive > 10)
        {
			Destroy(gameObject);
        }
	}
}
