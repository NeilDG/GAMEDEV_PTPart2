using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum ObjectBehaveType
{
	AddRigidBody,
	AddForce,
	MakeSound,
	PlayAnimation,
	PlayParticle
}

public class ObjectManager : MonoBehaviour 
{
	private static ObjectManager instance;

	public static ObjectManager Instance
	{
		get 
		{
			if(instance == null)
			{
				GameObject g = new GameObject();
				instance = g.AddComponent<ObjectManager>();
			}
			return instance;
		}
	}

	private Dictionary<string, GameObject> objectPool;
	private bool isTriggersActive = true;

	//TODO: @NeilDG set this to "true" when lights already went off
	public bool IsTriggersActive
	{
		set { this.isTriggersActive = value; }
		get { return this.isTriggersActive; }
	}

	void Awake()
	{
		instance = this;
		this.objectPool = new Dictionary<string, GameObject>();
	}

	void Update()
	{
	}

	public void AddObjectToPool(string objectKey, GameObject obs)
	{
		if(!this.objectPool.ContainsKey(objectKey))
		{
			this.objectPool.Add(objectKey, obs);
		}
		else
		{
			Debug.LogError("This object is already added!");
		}
	}

	public void RemoveObjectFromPool(string objectKey)
	{
		if(this.objectPool.ContainsKey(objectKey))
		{
			this.objectPool.Remove(objectKey);
		}
		else
		{
			Debug.LogError("Objectkey:"+ objectKey +" not found!");
		}
	}

	public void SetObjectBehaviour(string objectKey, ObjectBehaveType type)
	{
		if(this.objectPool.ContainsKey(objectKey))
		{
			this.DoObjectBehaviour(this.objectPool[objectKey], type);
		}
		else
		{
			Debug.LogError("Objectkey:"+ objectKey +" not found!");
		}
	}

	private void DoObjectBehaviour(GameObject targetObject, ObjectBehaveType type)
	{
		switch(type)
		{
		case ObjectBehaveType.AddRigidBody:
			this.DoRigidBodyBehaviour(targetObject);
			break;
		case ObjectBehaveType.AddForce:
			this.DoAddForceBehaviour(targetObject);
			break;
		case ObjectBehaveType.MakeSound:
			this.DoMakeSoundBehaviour(targetObject);
			break;
		case ObjectBehaveType.PlayAnimation:
			this.DoPlayAnimationBehaviour(targetObject);
			break;
		case ObjectBehaveType.PlayParticle:
			this.DoPlayParticleBehaviour(targetObject);
			break;
		}
	}

	private void DoRigidBodyBehaviour(GameObject targetObject)
	{
		ObjectController oc = targetObject.GetComponent<ObjectController>();
		targetObject.AddComponent<Rigidbody>();
		ConstantForce cf = targetObject.AddComponent<ConstantForce>();
		cf.GetComponent<ConstantForce>().force = new Vector3(0,-50,0);
	}

	private void DoAddForceBehaviour(GameObject targetObject)
	{

	}

	private void DoMakeSoundBehaviour(GameObject targetObject)
	{
		
	}

	private void DoPlayAnimationBehaviour(GameObject targetObject)
	{
		
	}

	private void DoPlayParticleBehaviour(GameObject targetObject)
	{
		
	}
}
