using UnityEngine;
using System.Collections;

public class EventsInitiator : MonoBehaviour {
	private static EventsInitiator sharedInstance = null;

	public static EventsInitiator Instance {
		get {
			return sharedInstance;
		}
	}

	[SerializeField] private GameEvent[] gameEventList;
	[SerializeField] private AudioClip[] eventShakeClip;

	void Awake() {
		sharedInstance = this;

		RenderSettings.fogDensity = 0.0f; //let ceiling lights change the fog density
	}

	public void InitiateTremorEvent() {
		int randomIndex = Random.Range(0 , this.eventShakeClip.Length);

		this.ActivateGameEvent (GameEventNames.CAMERA_SHAKE_EVENT_NAME);
		AtmosphereHandler.Instance.PlayAmbientEventSound(this.eventShakeClip[randomIndex]);
	}

	public void ActivateGameEvent(string gameEventName) {
		foreach(GameEvent gameEvent in this.gameEventList) {
			if(gameEvent.GetGameEventName() == gameEventName) {
				gameEvent.OnStartEvent();
			}
		}
	}

	public bool IsGameEventActive(string gameEventName) {
		foreach(GameEvent gameEvent in this.gameEventList) {
			if(gameEvent.GetGameEventName() == gameEventName) {
				return (gameEvent.isActiveAndEnabled);
			}
		}
		return false;
	}
}
