using UnityEngine;
using System.Collections;

public class PowerSourceRandomizer : MonoBehaviour {
	[SerializeField] private PowerSource[] powerSourceList;

	// Use this for initialization
	void Start () {
		this.HideAllPowerSources ();

		EventBroadcaster.Instance.AddObserver (EventNames.ON_MAIN_EVENT_GAME_STARTED, this.RandomizeActivePowerSource);
	}

	void Destroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.ON_MAIN_EVENT_GAME_STARTED, this.RandomizeActivePowerSource);
	}

	private void HideAllPowerSources() {
		for(int i = 0; i < this.powerSourceList.Length; i++) {
			this.powerSourceList[i].gameObject.SetActive(false);
		}
	}

	private void RandomizeActivePowerSource() {
		int index = Random.Range (0, this.powerSourceList.Length);
		this.powerSourceList[index].gameObject.SetActive(true);

		Debug.Log ("Selected " + index + " as power source");
	}
}
