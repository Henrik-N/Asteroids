using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class RigidB : MonoBehaviour
{
	[SerializeField]
	float mass = 1f;

	[SerializeField]
	float linearDrag = 1f;

	[SerializeField]
	float angularDrag = 0.5f;

	public Vector2 Velocity { get; set; }
	float _angularVelocity;

	public void AddForce(in Vector2 force)
	{
		Velocity += force * ((1f / mass) * Time.fixedDeltaTime);
	}

	public void AddTorque(in float torque)
	{
		float momentOfIntertia = mass;
		_angularVelocity += torque * (1f / momentOfIntertia); // todo take fixed dt into account here?
	}
	
	void Awake()
	{
		if (mass <= 0f)
			Debug.LogError("Mass can't be zero");
	}

	void FixedUpdate()
	{
		Vector2 decelerationForce = -linearDrag * Velocity;
		AddForce(decelerationForce);

		float torqueDecelerationForce = -angularDrag * _angularVelocity;
		AddTorque(torqueDecelerationForce);

		Quaternion torqueToAdd = Quaternion.AngleAxis(_angularVelocity * Time.fixedDeltaTime, Vector3.forward);

		transform.position += (Vector3) (Velocity * Time.fixedDeltaTime);
		transform.rotation *= torqueToAdd;
	}
}
