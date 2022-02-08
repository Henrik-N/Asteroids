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

	Vector2 _velocity = Vector2.zero;
	float _angularVelocity;

	float _fixedDt;

	void Awake()
	{
		if (mass <= 0f)
			Debug.LogError("Mass can't be zero");
		
		_fixedDt = Time.fixedDeltaTime;
	}

	public void AddForce(in Vector2 force)
	{
		_velocity += force * ((1f / mass) * _fixedDt);
	}

	public void AddTorque(in float torque)
	{
		float momentOfIntertia = mass;
		_angularVelocity += torque * (1f / momentOfIntertia); // todo take fixed dt into account here?
	}

	void FixedUpdate()
	{
		Vector2 decelerationForce = -linearDrag * _velocity;
		AddForce(decelerationForce);

		float torqueDecelerationForce = -angularDrag * _angularVelocity;
		AddTorque(torqueDecelerationForce);

		
		Quaternion torqueToAdd = Quaternion.AngleAxis(_angularVelocity * _fixedDt, Vector3.forward);

		transform.position += (Vector3) (_velocity * _fixedDt);
		transform.rotation *= torqueToAdd;
	}
}
