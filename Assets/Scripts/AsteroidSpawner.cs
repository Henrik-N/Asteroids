using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	[SerializeField]
	GameObject asteroidPrefab;

	[SerializeField]
	Transform[] spawnPoints;
	
	static AsteroidSpawner _instance;
	
	static AsteroidSpawner Instance
	{
		get
		{
			if (!_instance)
				Debug.LogError("Collision system instance == null");
			return _instance;
		}
		set => _instance = value;
	}

	public static Asteroid2 SpawnAsteroid()
	{
		return Instantiate(Instance.asteroidPrefab).GetComponent<Asteroid2>();
	}

	void Awake()
	{
		Instance = this;
		SpawnAsteroids();
	}
	
	void SpawnAsteroids()
	{
		foreach (var spawnPos in spawnPoints)
		{
			Asteroid2 asteroid = SpawnAsteroid();

			asteroid.transform.position = spawnPos.position;

			// Start as large
			asteroid.Init(Asteroid2.SizeGrade.Large);
			
			// set random movement speed
			asteroid.SetMovementSpeed(Random.Range(asteroid.minSpeed, asteroid.maxSpeed));
		
			// set off in random direction
			float x = Random.Range(-1f, 1f);
			float y = Random.Range(-1f, 1f);
			Vector2 dir = new Vector2(Mathf.Cos(x), Mathf.Sin(y));
			
			asteroid.rb.AddForce(dir * asteroid.movementSpeed);
			asteroid.rb.AddTorque(asteroid.movementSpeed);
		}
	}
}

