using System;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(RigidB), typeof(BulletGun), typeof(CircCollider))]
public class Player : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField]
	float maxSpeed = 8f;

	[SerializeField]
	float turnSpeed = 2f;

	const float TurnspeedMultiplier = 100f;

	// components
	RigidB _rb;
	BulletGun _bulletGun;

	JoyStick _joystick;

	int _hp = 100;


	void OnDrawGizmos()
	{
		Handles.Label(transform.position, "Yeet health: " + _hp);
	}


	void Awake()
	{
		_rb = GetComponent<RigidB>();
		_bulletGun = GetComponent<BulletGun>();

		GetComponent<CircCollider>().OnCollisionEvent += OnCollisionEvent;
	}

	void Update()
	{
		_joystick.UpdateState();

		if (!Input.GetKeyDown(KeyCode.Space)) return;
		
		float forwardMovementSpeed = Vector2.Dot(transform.up, _rb.Velocity);
		_bulletGun.FireBullet(forwardMovementSpeed);
	}

	void FixedUpdate()
	{
		float speed = Mathf.Clamp(_joystick.Y * maxSpeed, 0f, maxSpeed);
		Vector2 force = transform.up * speed;

		_rb.AddForce(force);
		_rb.AddTorque(-turnSpeed * TurnspeedMultiplier * _joystick.X);
	}

	void OnCollisionEvent(object sender, CircCollider other)
	{
		if (other.collisionMask.HasFlag(CircCollider.CollisionMask.Projectile)) return; // ignore own projectiles

		_hp -= 10;
		Debug.Log("Player hit. HP: " + _hp);
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
