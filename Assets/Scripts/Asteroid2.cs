using System;
using UnityEngine;

public class Asteroid2 : MonoBehaviour
{
	[SerializeField]
	Sprite spriteLarge;

	[SerializeField]
	Sprite spriteSmall;

	[Header("Speed randomizer")]
	[SerializeField]
	public float minSpeed = 3f;
	[SerializeField]
	public float maxSpeed = 5f;
	
	public float movementSpeed; // = Random.Range(minSpeed, maxSpeed);
	
	// components
	SpriteRenderer _spriteRenderer;
	CircCollider _collider;
	public RigidB rb;
	
	public enum SizeGrade
	{
		None, // destroyed
		Small,
		Medium,
		Large
	}
	
	SizeGrade _grade;

	SizeGrade Grade
	{
		get => _grade;
		set
		{
			_grade = value;
			switch (_grade)
			{
				case SizeGrade.Small:
					_spriteRenderer.sprite = spriteSmall;
					transform.localScale = Vector3.one;
					_collider.radius = .16f;
					break;
				case SizeGrade.Medium:
					_spriteRenderer.sprite = spriteLarge;
					_collider.radius = .25f;
					transform.localScale = Vector3.one;
					break;
				case SizeGrade.Large:
					_spriteRenderer.sprite = spriteLarge;
					_collider.radius = .5f;
					transform.localScale = new Vector3(2, 2, 1);
					break;
				case SizeGrade.None:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(value), value, null);
			}
		}
	}

	void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<CircCollider>();
		rb = GetComponent<RigidB>();
		_collider.OnCollisionEvent += OnCollisionEvent;
	}
	
	public void Init(in SizeGrade grade)
	{
		Grade = grade;
	}

	public void SetMovementSpeed(in float speed)
	{
		movementSpeed = speed;
	}
	
	void OnCollisionEvent(object sender, CircCollider other)
	{
		// make smaller
		Grade = Grade switch
		{
			SizeGrade.Large => SizeGrade.Medium,
			SizeGrade.Medium => SizeGrade.Small,
			_ => SizeGrade.None
		};
		
		if (Grade == SizeGrade.None)
		{
			Destroy(gameObject); // TODO remove through spawner singleton
			return;
		}

		Vector2 toOther = _collider.impactPosition - other.impactPosition;
		Vector3 perpendicular = Vector3.Cross(toOther, Vector3.forward).normalized;
	
		// spawn new asteroid
		Asteroid2 newAsteroid = AsteroidSpawner.SpawnAsteroid();
		newAsteroid.Init(Grade);
		
		// position of newly spawned asteroid and this asteroid need to need to be offset to not trigger a collision with each other
		Vector3 positionOffset = perpendicular * (_collider.radius + 0.1f);
		newAsteroid.transform.position = transform.position - positionOffset;
		transform.position += positionOffset;
	
		
		// make them a bit faster
		SetMovementSpeed(movementSpeed * 1.2f);
		newAsteroid.SetMovementSpeed(movementSpeed * 1.2f);
		
		// add forces in opposite directions perpendicular to the impact
		rb.Velocity = Vector2.zero;
		rb.AddForce(perpendicular * movementSpeed);
		newAsteroid.rb.AddForce(-perpendicular * newAsteroid.movementSpeed);
		
		// add some spin
		rb.AddTorque(movementSpeed);
		newAsteroid.rb.AddTorque(movementSpeed * 2f);
	}
}
