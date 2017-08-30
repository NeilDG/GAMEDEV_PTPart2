using UnityEngine;
using System.Collections;

public class HighlightScript : MonoBehaviour {

	[SerializeField] private MeshRenderer renderer;
	[SerializeField] private Color minColor;
	[SerializeField] private Color maxColor;
	[SerializeField] private float speed = 0;

	private float timer = 0.0f;
	private bool isHighlight = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.F5))
		{
			isHighlight = !isHighlight;
		}

		if(isHighlight)
			this.renderer.material.color = Color.Lerp(this.renderer.material.color, maxColor, Time.deltaTime * speed);
		else
			this.renderer.material.color = Color.Lerp(this.renderer.material.color, minColor, Time.deltaTime * speed);

		if(this.renderer.material.color.r < 0.1f || this.renderer.material.color.r > 0.9f)
			isHighlight = !isHighlight;
	}
}
