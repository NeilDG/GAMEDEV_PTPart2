using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundInstanceManager : MonoBehaviour {
	
	public void Play(AudioClip audioClip, bool loop, float delay, float pitch)
	{
		if (audioClip == null)
		{
			SoundManager.Instance.StoreAudioObject(gameObject);
			return;
		}
		if (GetComponent<AudioSource>().ignoreListenerVolume && GetComponent<AudioSource>().clip == audioClip) return; // do nothing

		GetComponent<AudioSource>().playOnAwake = false;
		GetComponent<AudioSource>().pitch = pitch;
		GetComponent<AudioSource>().clip = audioClip;
		GetComponent<AudioSource>().loop = loop;
		GetComponent<AudioSource>().PlayDelayed(delay);
		if (delay >= 0 && !loop) StartCoroutine(StoreDelay((delay + audioClip.length) * 2.0f));
		else if (loop) DontDestroyOnLoad(gameObject);
	}

	public void Stop() {
		this.GetComponent<AudioSource>().Stop ();
		if (GetComponent<AudioSource>().loop) {
			Destroy(this.gameObject);
		}
	}

	IEnumerator StoreDelay(float duration)
	{
		yield return new WaitForSeconds(duration);
		SoundManager.Instance.StoreAudioObject(gameObject);
	}
}
