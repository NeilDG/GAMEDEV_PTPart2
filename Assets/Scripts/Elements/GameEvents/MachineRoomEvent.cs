using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the machine door opening and randomizing of power source to open.
/// </summary>
public class MachineRoomEvent : GameEvent {
	[SerializeField] private GameObject machineDoor;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		this.gameEventName = GameEventNames.MACHINE_ROOM_EVENT_NAME;
	}

	public override void OnStartEvent ()
	{
		this.machineDoor.SetActive (false);
		Debug.Log ("Opened machine door");

		this.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
