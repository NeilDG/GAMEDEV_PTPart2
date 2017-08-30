using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains all patrol points for the enemy to walk in.
/// </summary>
public class EnemyPatrolPointDirectory : MonoBehaviour {

	private static EnemyPatrolPointDirectory sharedInstance = null;
	public static EnemyPatrolPointDirectory Instance {
		get {
			return sharedInstance;
		}
	}

	[SerializeField] private List<Transform> patrolPoints;

	void Awake() {
		sharedInstance = this;
	}

	// Use this for initialization
	void Start () {
		this.patrolPoints = new List<Transform> ();
		this.FindAllSpawnPoints ();
	}

	private void FindAllSpawnPoints() {
		foreach (Transform child in this.transform) {

			MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
			if(meshRenderer != null) {
				meshRenderer.enabled = false;
			}

			this.patrolPoints.Add(child);
		}
	}

	public Transform GetRandomPatrolPoint() {
		int index = Random.Range (0, this.patrolPoints.Count);

		return this.patrolPoints [index];
	}

	public int GetNumberOfPatrolPoints() {
		return this.patrolPoints.Count;
	}

}
