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
	
	JoyStick _joystick;
	RigidB _rb;

	void Awake()
	{
		_rb = GetComponent<RigidB>();
	}

	void Update()
	{
		_joystick.UpdateState();
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
