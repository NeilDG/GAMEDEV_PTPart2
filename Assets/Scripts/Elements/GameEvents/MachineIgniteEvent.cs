using UnityEngine;
using System.Collections;

public class MachineIgniteEvent : GameEvent {
	[SerializeField] private AudioSource audioSource;

	[SerializeField] private AudioClip[] machineClips;

	[SerializeField] private AudioClip machineOverloadClip;
	[SerializeField] private AudioClip machineRunClip;
	[SerializeField] private AudioClip machineWarningClip;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		this.gameEventName = GameEventNames.MACHINE_IGNITE_EVENT_NAME;
	}

	public override void OnStartEvent ()
	{
		this.audioSource.playOnAwake = false;
		this.gameObject.SetActive (true);

		ObjectiveView.ShowObjective (DialogConstants.OBJECTIVE_3_STRING);
		this.StartCoroutine (this.PlayIgniteMachineSounds());
	}

	private IEnumerator PlayIgniteMachineSounds() {
		for(int i = 0; i < this.machineClips.Length; i++) {
			this.audioSource.clip = this.machineClips[i];
			this.audioSource.Play();
			yield return new WaitForSeconds(this.audioSource.clip.length);
		}

		this.audioSource.clip = this.machineOverloadClip;
		this.audioSource.Play ();
		EventBroadcaster.Instance.PostEvent (GameEventNames.MACHINE_IGNITE_EVENT_NAME);
		yield return new WaitForSeconds (13.0f);

		this.audioSource.clip = this.machineRunClip;
		this.audioSource.loop = true;
		this.audioSource.Play ();

		AtmosphereHandler.Instance.PlayAmbientEventSoundLoop (this.machineWarningClip, 0.4f);
		GameStateMachine.Instance.ChangeState (GameStateMachine.StateType.GAME_ESCAPE_EVENT);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
