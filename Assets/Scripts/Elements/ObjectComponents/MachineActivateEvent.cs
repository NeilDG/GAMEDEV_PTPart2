using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineActivateEvent : MonoBehaviour {

	[SerializeField] private Animation machineAnimation;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (false);
		this.machineAnimation.wrapMode = WrapMode.Once;

		EventBroadcaster.Instance.AddObserver (GameEventNames.MACHINE_IGNITE_EVENT_NAME, this.OnMachineIgniteEvent);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (GameEventNames.MACHINE_IGNITE_EVENT_NAME);
	}

	private void OnMachineIgniteEvent() {
		this.gameObject.SetActive(true);
		this.machineAnimation.Play ();
	}
}
