using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
	static CollisionSystem _instance;
	List<CircCollider> _colliderObjs = new List<CircCollider>();
	
	public static CollisionSystem Instance
	{
		get
		{
			if (_instance == null)
				Debug.LogError("Collision system instance == null");
			return _instance;
		}
	}
	
	void Awake()
	{
		_instance = this;
	}
	
	///  Registers a collider to the system
	public void Register(in CircCollider col)
	{
		_colliderObjs.Add(col);
	}

	void FixedUpdate()
	{
		FireAnyCollisionEvents();
	}

	void FireAnyCollisionEvents()
	{
		// clean pointers to destroyed GameObjects
		_colliderObjs.RemoveAll(c => c == null);
		
		for (int i = 0; i < _colliderObjs.Count; i++)
		{
			for (int k = i + 1; k < _colliderObjs.Count; k++)
			{
				// ignore disabled colliders
				if (!_colliderObjs[i].colliderEnabled || !_colliderObjs[k].colliderEnabled) continue;
				
				bool areColliding = _colliderObjs[k].CheckOverlap(_colliderObjs[i]);

				if (!areColliding) continue;

				_colliderObjs[i].impactPosition = _colliderObjs[i].transform.position;
				_colliderObjs[k].impactPosition = _colliderObjs[k].transform.position;
				
				_colliderObjs[k].OnCollisionEvent?.Invoke(this, _colliderObjs[i]);
				_colliderObjs[i].OnCollisionEvent?.Invoke(this, _colliderObjs[k]);
			}
		}
	}
}
