using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour 
{
	[SerializeField] private GameObject targetObject;
	[SerializeField] private ObjectKeys targetObjectKey;
	[SerializeField] private ObjectBehaveType behaveType;

	// Use this for initialization
	void Start () {
		ObjectManager.Instance.AddObjectToPool(this.targetObjectKey.ToString(), targetObject);
	}

	void OnTriggerEnter(Collider c)
	{
		if(ObjectManager.Instance.IsTriggersActive && c.tag == "Player")
		{
			ObjectManager.Instance.SetObjectBehaviour(this.targetObjectKey.ToString(), this.behaveType);
			Debug.Log("Collided with:"+ this.gameObject.name);
			Destroy(this.GetComponent<Collider>());
		}
	}
}
