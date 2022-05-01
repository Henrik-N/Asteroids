
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WorldboxSystem: MonoBehaviour
{
	[SerializeField]
	Transform topLeft;

	[SerializeField]
	Transform bottomRight;
	
	List<RigidB> _rbs = new List<RigidB>();
	
	static WorldboxSystem _instance;
	
	static WorldboxSystem Instance
	{
		get
		{
			if (!_instance)
				Debug.LogError("World box system == null");
			return _instance;
		}
		set => _instance = value;
	}

	void Awake()
	{
		Instance = this;
	}


	public static void Register(in RigidB b)
	{
		Instance._rbs.Add(b);
	}

	void Update()
	{
		Instance._rbs.RemoveAll(rb => rb == null);
		
		Instance._rbs.ForEach(rb => 
		{
			var pos = rb.transform.position;

			pos = pos.x > bottomRight.position.x ? new Vector3(topLeft.position.x, pos.y, pos.z)
				: pos.x < topLeft.position.x ? new Vector3(bottomRight.position.x, pos.y, pos.z)
				: pos;

			pos = pos.y > topLeft.position.y ? new Vector3(pos.x, bottomRight.position.y, pos.z)
				: pos.y < bottomRight.position.y ? new Vector3(pos.x, topLeft.position.y, pos.z)
				: pos;
			
			rb.transform.position = pos;
		});
	}
}
