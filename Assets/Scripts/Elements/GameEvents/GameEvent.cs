using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a game event
/// </summary>
public abstract class GameEvent : MonoBehaviour {
	protected string gameEventName;

	public string GetGameEventName() {
		return this.gameEventName;
	}

	public virtual void Start() {
		this.gameObject.SetActive (false);
	}

	public abstract void OnStartEvent();
}
