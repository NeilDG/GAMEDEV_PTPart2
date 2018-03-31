using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimationSimulator : MonoBehaviour {

	[SerializeField] private Animator animation;
	[SerializeField] private Text[] uilabels;
	[SerializeField] private string[] animationNames;


	void Start()
	{
		for(int i = 0 ; i < animationNames.Length; i++)
		{
			uilabels[i].text = animationNames[i];
		}
	}

	public void OnIdleClicked()
	{
		animation.Play(animationNames[0]);
	}

	public void OnWalkClicked()
	{
		animation.Play(animationNames[1]);
	}

	public void OnRunClicked()
	{
		animation.Play(animationNames[2]);
	}

	public void OnAttack1Clicked()
	{
		animation.Play(animationNames[3]);
	}

	public void OnAttack2Clicked()
	{
		animation.Play(animationNames[4]);
	}
}
