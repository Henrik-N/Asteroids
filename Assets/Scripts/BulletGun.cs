using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidB))]
public class BulletGun : MonoBehaviour
{
	[SerializeField]
	Transform fireFrom;

	[SerializeField]
	GameObject bulletPrefab;
	
	[SerializeField]
	float bulletSpeed = 2f;

	[SerializeField]
	int bulletPoolSize = 50;

	[SerializeField]
	float bulletLifeTime = 3f;
	
	List<Bullet> _bulletBuffer;

	int _currentIndex;
	int NextIndex => _currentIndex++ % bulletPoolSize;
	
	void Awake()
	{
		InitBullets();
	}

	void InitBullets()
	{
		_bulletBuffer = new List<Bullet>(bulletPoolSize);
		
		for (int i = 0; i < bulletPoolSize; i++)
		{
			GameObject bullet = Instantiate(bulletPrefab);
			Bullet bulletComp = bullet.GetComponent<Bullet>();
			_bulletBuffer.Add(bulletComp);
		}
	}

	public void FireBullet(in float currentforwardMovementSpeed)
	{
		_bulletBuffer[NextIndex].Fire(
			from: fireFrom.position, 
			velocity: fireFrom.up * (currentforwardMovementSpeed + bulletSpeed),
			lifeTime: bulletLifeTime
			);
	}
}
