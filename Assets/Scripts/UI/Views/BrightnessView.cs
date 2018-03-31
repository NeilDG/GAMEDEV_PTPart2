using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BrightnessView : View {
	[SerializeField] private Slider slider;

	void Start() {
		this.slider.value = 0.5f;
	}

	public void OnSliderChanged() {
		Debug.Log("Slider changed " +this.slider.value);

		float floatDensity = this.slider.value * GameFlowConstants.MAX_FOG_DENSITY;

		UserSettings.Instance.SetFogDensity(floatDensity);
		RenderSettings.fogDensity = UserSettings.Instance.GetFogDensity();
	}

	public void OnConfirmClicked() {
		Cursor.visible = false;
		LoadManager.LoadScene(SceneNames.IN_GAME_SCENE, true);
	}
}
