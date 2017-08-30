using UnityEngine;
using System.Collections;

public class SFXCaller : MonoBehaviour {

	[SerializeField] GameObject loadingIndicator;
	/// <summary>
	/// Created by: Zee
	/// Script for calling SFX of loading in and loading out.
	/// </summary>
	public void LoaderIn()
	{
		SoundManager.Instance.PlaySFX (SoundKeys.LOADING_IN_SFX_KEY);
	}

	public void LoaderOut()
	{
		SoundManager.Instance.PlaySFX (SoundKeys.LOADING_OUT_SFX_KEY);
	}

	public void PlayGoldSFX()
	{
		SoundManager.Instance.PlaySFX (SoundKeys.GOLD_RECEIVE_SFX_KEY);
	}

	public void PlayStar1SFX()
	{
		SoundManager.Instance.PlaySFX (SoundKeys.STAR_COMPLETE_1_SFX_KEY);
	}

	public void PlayStar2SFX()
	{
		SoundManager.Instance.PlaySFX (SoundKeys.STAR_COMPLETE_2_SFX_KEY);
	}

	public void PlayStar3SFX()
	{
		SoundManager.Instance.PlaySFX (SoundKeys.STAR_COMPLETE_3_SFX_KEY);
	}

	public void PlayGoldShineSFX()
	{
		SoundManager.Instance.PlaySFX (SoundKeys.GOLD_SHINE_SFX_KEY);
	}

	public void PlayObstacleHintSFX()
	{
		SoundManager.Instance.PlaySFX (SoundKeys.OBSTACLE_HINT_SFX_KEY);
	}
}
