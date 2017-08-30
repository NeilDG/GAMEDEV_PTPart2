using UnityEngine;
using System.Collections;

public class UserSettings {
	private static UserSettings sharedInstance = null;

	public static UserSettings Instance {
		get {
			if(sharedInstance == null) {
				sharedInstance = new UserSettings();
			}

			return sharedInstance;
		}
	}

	private float fogDensity = 0.0f;

	private UserSettings() {
		this.fogDensity = GameFlowConstants.MAX_FOG_DENSITY;
	}

	public void SetFogDensity(float value) {
		this.fogDensity = value;
	}

	public float GetFogDensity() {
		return this.fogDensity;
	}
}
