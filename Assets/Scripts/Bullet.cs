using System;
using System.Timers;
using UnityEngine;

[RequireComponent(typeof(RigidB))]
public class Bullet : MonoBehaviour
{
	RigidB _rb;
	SpriteRenderer _sprite;

	bool Enabled
	{
		get => _rb.enabled;
		set
		{
			_rb.enabled = value;
			_sprite.enabled = value;
		}
	}

	void Awake()
	{
		_rb = GetComponent<RigidB>();
		_sprite = GetComponent<SpriteRenderer>();
		
		Enabled = false;
	}

	public void Fire(in Vector2 from, in Vector2 velocity, in float lifeTime)
	{
		Enabled = true;
		
		transform.position = from;

		_rb.Velocity = Vector2.zero;
		_rb.AddForce(velocity);
	}
}
