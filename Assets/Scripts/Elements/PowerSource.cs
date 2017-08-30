using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a power source
/// </summary>
public class PowerSource : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Power Collided with: " + other.gameObject.name);

		PlayerHP playerHP = other.GetComponent<PlayerHP> ();

		if(playerHP != null && EventsInitiator.Instance.IsGameEventActive(GameEventNames.MACHINE_IGNITE_EVENT_NAME) == false){
			EventsInitiator.Instance.ActivateGameEvent(GameEventNames.MACHINE_IGNITE_EVENT_NAME);
			EventsInitiator.Instance.ActivateGameEvent(GameEventNames.CAMERA_SHAKE_EVENT_NAME);
		}
	}
}
