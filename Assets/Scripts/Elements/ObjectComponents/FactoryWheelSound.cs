using UnityEngine;
using System.Collections;

public class FactoryWheelSound : MonoBehaviour {

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		this.audioSource = this.GetComponent<AudioSource> ();
		EventBroadcaster.Instance.AddObserver (EventNames.ON_ESCAPE_EVENT_STARTED, this.StartSound);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_ESCAPE_EVENT_STARTED, this.StartSound);
	}

	private void StartSound() {
		this.audioSource.loop = true;
		this.audioSource.Play ();
	}
}
