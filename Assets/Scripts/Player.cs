using System;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(RigidB))]
public class Player : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField]
	float maxSpeed = 8f;

	[SerializeField]
	float turnSpeed = 2f;
	
	const float TurnspeedMultiplier = 100f;
	
	[SerializeField]
	GameObject bulletPrefab;

	// components
	RigidB _rb;
	BulletGun _bulletGun;

	JoyStick _joystick; 
	
	void Awake()
	{
		_rb = GetComponent<RigidB>();
		_bulletGun = GetComponent<BulletGun>();
	}

	void Update()
	{
		_joystick.UpdateState();
		

		if (Input.GetKeyDown(KeyCode.Space))
		{
			float forwardMovementSpeed = Vector2.Dot(transform.up, _rb.Velocity);
			_bulletGun.FireBullet(forwardMovementSpeed);
		}
	}

	void FixedUpdate()
	{
		float speed = Mathf.Clamp(_joystick.Y * maxSpeed, 0f, maxSpeed);
		Vector2 force = transform.up * speed;

		_rb.AddForce(force);
		_rb.AddTorque(-turnSpeed * TurnspeedMultiplier * _joystick.X);
	}

	struct JoyStick
	{
		Vector2 _state; 

		public float X => _state.x;
		public float Y => _state.y;

		public void UpdateState()
		{
			_state = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
		}
	}
}
