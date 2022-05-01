using UnityEngine;

[RequireComponent(typeof(RigidB))]
public class Bullet : MonoBehaviour
{
	RigidB _rb;
	SpriteRenderer _sprite;
	CircCollider _collider;

	readonly Vector2 _resetPos = new Vector2(1000f, 1000f);

	float lifeTime = 1f;
	float timer = 0f;

	bool Enabled
	{
		set
		{
			_rb.enabled = value;
			_sprite.enabled = value;
			_collider.colliderEnabled = value;
			
			if (value)
			{
				timer = 0f;
			}
		}
		get => _rb.enabled;
	}

	void Awake()
	{
		_rb = GetComponent<RigidB>();
		_sprite = GetComponent<SpriteRenderer>();
		_collider = GetComponent<CircCollider>();
		_collider.OnCollisionEvent += OnCollisionEvent;
		
		ResetBullet();
	}

	void ResetBullet()
	{
		Enabled = false;
		transform.position = _resetPos;
	}

	void OnCollisionEvent(object sender, CircCollider other)
	{
		if (other.collisionMask.HasFlag(CircCollider.CollisionMask.Player) ||
		    other.collisionMask.HasFlag(CircCollider.CollisionMask.Projectile)) return; // ignore collisions with player and other projectiles

		ResetBullet();
	}
	
	public void Fire(in Vector2 from, in Vector2 velocity, in float bulletLifeTime)
	{
		Enabled = true;
		lifeTime = bulletLifeTime;
		
		transform.position = from;

		_rb.Velocity = Vector2.zero;
		_rb.AddForce(velocity);
	}

	void Update()
	{
		if (!Enabled) return;
		
		timer += Time.deltaTime;
		if (timer > lifeTime)
		{
			Enabled = false;
		}
	}
}
