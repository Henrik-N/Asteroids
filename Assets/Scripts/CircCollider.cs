using System;
using UnityEngine;

public class CircCollider : MonoBehaviour
{
	public float radius;

	[HideInInspector]
	public bool colliderEnabled = true;
	
	public EventHandler<CircCollider> OnCollisionEvent; // input is the collided with collider

	// the position of the object at the moment of impact (may change after if it gets reset etc)
	[NonSerialized]
	public Vector2 impactPosition = Vector2.zero;
	
	
	[Flags]
	public enum CollisionMask
	{
		None = 0,
		Player = 1 << 0,
		Projectile = 1 << 1,
		Asteroid = 1 << 2
	}
	public CollisionMask collisionMask;
	
	
	void Start()
	{
		CollisionSystem.Instance.Register(this);
	}

	public bool CheckOverlap(in CircCollider otherCollider)
	{
		if (!colliderEnabled || !otherCollider.colliderEnabled) 
			return false;
		
		Vector2 otherPos = otherCollider.gameObject.transform.position;
		Vector2 thisPos = transform.position;
		float rangeBetween = (thisPos - otherPos).magnitude;

		return rangeBetween < radius + otherCollider.radius;
	}

	void OnDrawGizmos() // debug lines to see how big the HitBox is
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
