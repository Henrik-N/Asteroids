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
	
	List<Bullet> _bulletPool;

	int _currentIndex;
	int NextIndex => _currentIndex++ % bulletPoolSize;
	
	void Awake()
	{
		InitBullets();
	}

	void InitBullets()
	{
		_bulletPool = new List<Bullet>(bulletPoolSize);
		
		for (int i = 0; i < bulletPoolSize; i++)
		{
			GameObject bullet = Instantiate(bulletPrefab);
			Bullet bulletComp = bullet.GetComponent<Bullet>();
			_bulletPool.Add(bulletComp);
		}
	}

	public void FireBullet(in float currentforwardMovementSpeed)
	{
		_bulletPool[NextIndex].Fire(
			from: fireFrom.position, 
			velocity: fireFrom.up * (currentforwardMovementSpeed + bulletSpeed)
			);
	}
}
