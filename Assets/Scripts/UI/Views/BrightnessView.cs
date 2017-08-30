using UnityEngine;
using System.Collections;

public class BrightnessView : View {
	[SerializeField] private UILabel valueLabel;
	[SerializeField] private UISlider slider;

	void Start() {
		this.slider.value = 0.5f;
		this.valueLabel.text = GameFlowConstants.MAX_FOG_DENSITY.ToString();
	}

	public void OnSliderChanged() {
		Debug.Log("Slider changed " +this.slider.value);

		float floatDensity = this.slider.value * GameFlowConstants.MAX_FOG_DENSITY;
		this.valueLabel.text = floatDensity.ToString("0.000");

		UserSettings.Instance.SetFogDensity(floatDensity);
		RenderSettings.fogDensity = UserSettings.Instance.GetFogDensity();
	}

	public void OnConfirmClicked() {
		Cursor.visible = false;
		Application.LoadLevel(SceneNames.IN_GAME_SCENE);
	}
}
