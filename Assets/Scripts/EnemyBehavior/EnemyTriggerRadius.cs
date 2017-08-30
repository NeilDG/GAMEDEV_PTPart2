using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the on trigger event for any detected triggers
/// </summary>
public class EnemyTriggerRadius : MonoBehaviour {

	private System.Action<Collider> actionForEnter;
	private System.Action<Collider> actionForStay;
	private System.Action<Collider> actionForExit;

	public void SetOnTriggerDelegate(System.Action<Collider> actionForEnter, System.Action<Collider> actionForStay, System.Action<Collider> actionForExit) {
		this.actionForEnter = actionForEnter;
		this.actionForStay = actionForStay;
		this.actionForExit = actionForExit;
	}

	void OnTriggerEnter(Collider other) {
		if (this.actionForEnter != null) {
			this.actionForEnter(other);
		}
	}

	void OnTriggerStay(Collider other) {
		if (this.actionForStay != null) {
			this.actionForStay(other);
		}
	}

	void OnTriggerExit(Collider other) {
		if(this.actionForExit != null) {
			this.actionForExit(other);
		}
	}

	public float GetRadius() {
		return this.GetComponent<SphereCollider>().radius;
	}
}
