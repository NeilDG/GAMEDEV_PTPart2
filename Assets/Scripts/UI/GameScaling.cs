using UnityEngine;
using System.Collections;

/// <summary>
/// Component that handles scaling for the game camera. This should not interfere with UIScaling.
/// 
/// Created By: NeilDG
/// </summary>
public class GameScaling : MonoBehaviour {

	private Camera gameCamera;

	private const float UNIT_CONSTANT = 0.75f;

	void Awake() {
		this.gameCamera = this.GetComponent<Camera>();
	}

	// Use this for initialization
	void Start () {
		this.UpdateCameraSize();
	}

	private void UpdateCameraSize() {
		this.gameCamera.orthographicSize = (float)Screen.height / (float)Screen.width * UNIT_CONSTANT;
	}

}
