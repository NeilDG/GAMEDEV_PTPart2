using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour 
{
	[System.Serializable]
	private struct LightRange
	{
		public float increaseAmount;
		public float decreaseAmount;
		public float minValue;
		public float maxValue;
	}

	[System.Serializable]
	private struct LightIntensity
	{
	    public float increaseAmount;
		public float decreaseAmount;
		public float minValue;
		public float maxValue;
	}

	private Light lightSource;

	[SerializeField] private CharacterController charControl;
	[SerializeField] private LightRange lightRange;
	//[SerializeField] private LightIntensity lightIntensity;

	// Use this for initialization
	void Start () {
		this.lightSource = this.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		bool isMoving = directionVector != Vector3.zero;

		this.UpdateLightSourceRange(isMoving);
		//this.UpdateLightIntensity(isMoving);
	}

	private void UpdateLightSourceRange(bool decrease = false)
	{
		if (decrease == true) {
			this.lightSource.range -= lightRange.decreaseAmount;
			this.lightSource.range = Mathf.Clamp(lightSource.range, 
			                                     lightRange.minValue, 
			                                     lightRange.maxValue);
		}
		else {
			this.lightSource.range += lightRange.increaseAmount;
			this.lightSource.range = Mathf.Clamp(lightSource.range, 
			                                     lightRange.minValue, 
			                                     lightRange.maxValue);
		}
	
	}

	/*private void UpdateLightIntensity(bool decrease = false)
	{
		if (decrease == true) {
			this.lightSource.intensity -= lightIntensity.decreaseAmount;
			this.lightSource.intensity = Mathf.Clamp(lightSource.intensity, 
			                                         lightIntensity.minValue, 
			                                         lightIntensity.maxValue);
		}
		else {
			this.lightSource.intensity += lightIntensity.increaseAmount;
			this.lightSource.intensity = Mathf.Clamp(lightSource.intensity, 
			                                         lightIntensity.minValue, 
			                                         lightIntensity.maxValue);
		}
	}*/
}
