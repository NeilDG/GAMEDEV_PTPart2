using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericCountdownTimer : MonoBehaviour {

	[SerializeField] private UILabel textDisplay;
	[SerializeField] private float duration;

	private float goalTime = 0.0f;
	private float lastKnownTime = 0.0f;
	private float currentTime = 0.0f;

	private bool countdownFinished = true;
	private bool paused = false;

	private ArrayList actionList = new ArrayList();

	// Use this for initialization
	void Start () {
		if (this.textDisplay != null) {
			this.textDisplay.text = string.Format ("{0:00}:{1:00}:{2:000}", 0, 0, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (this.countdownFinished == true || this.paused == true)
			return;

		this.currentTime = Mathf.Clamp (this.goalTime - Time.time, 0, float.MaxValue);

		if (this.currentTime > 0.000f) {
			int minutes = (int) currentTime / 60;
			int seconds = (int) currentTime % 60;
			int milliseconds =(int) (currentTime * 100) % 100;

			if (this.textDisplay != null) {
				this.textDisplay.text = string.Format ("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
			}
		} else {
			if (this.textDisplay != null) {
				this.textDisplay.text = string.Format ("{0:00}:{1:00}:{2:000}", 0, 0, 0);
			}
			this.countdownFinished = true;
			this.PerformFinishAction();
		}
	}

	public void SetDuration(float duration) {
		this.duration = duration;
	}

	public void StartTimer() {
		this.goalTime = Time.time + duration;
		this.countdownFinished = false;
	}

	public void PauseTimer() {
		this.paused = true;
		this.lastKnownTime = this.currentTime;
	}

	public void ResumeTimer() {
		if(this.countdownFinished == false) {
			this.paused = false;
			this.goalTime = Time.time + this.lastKnownTime;
		}
	}

	/// <summary>
	/// Adds a function to call upon timer finish.
	/// </summary>
	/// <param name="action">Action.</param>
	public void AddFinishAction(System.Action action) {
		this.actionList.Add (action);
	}

	/// <summary>
	/// Removes the specified function from the list of execution. 
	/// </summary>
	/// <param name="action">Action.</param>
	public void RemoveFinishAction(System.Action action) {
		if (this.actionList.Contains (action)) {
			this.actionList.Remove(action);
		}
	}

	public void PerformFinishAction() {
		Debug.LogWarning ("FINISHED TIMER!");

		foreach (System.Action action in this.actionList) {
			action();
		}
	}
}
