using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

public class SoundManager : ScriptableObject {

	//added by zee
	private bool 						isBGMPaused = false;

	private float 						fadeInTime = 0.0f;
	private float 						fadeOutTime = 1.0f;
	private float 						duration = 1.0f;

	private static SoundManager _instance;
	public static SoundManager Instance
	{
		get
		{
			if (_instance == null) LoadData("");
			return _instance;
		}
	}
	
	public static void LoadData(string location)
	{
		if (_instance != null) return;
		_instance = Resources.Load(location + "SoundManager", typeof(SoundManager)) as SoundManager;
	}
	
	#if UNITY_EDITOR
	public static SoundManager CreateData(string location)
	{
		_instance = CreateInstance<SoundManager>();
		AssetDatabase.CreateAsset(_instance, Path.Combine(location, "SoundManager.asset"));
		
		return _instance;
	}
	#endif

	float bgmVolume = 0.0f;
	public float BGMVolume
	{
		get
		{
			return bgmVolume;
		}
		set
		{
			bgmVolume = Mathf.Clamp(value, 0.0f, 1.0f);
			if (musicGameObject != null) musicGameObject.GetComponent<AudioSource>().volume = bgmVolume;
		}
	}
	public float SFXVolume
	{
		get
		{
			return AudioListener.volume;
		}
		set
		{
			AudioListener.volume = Mathf.Clamp(value, 0.0f, 1.0f);
		}
	}

	public string resourceLocation = string.Empty;
	public SoundData[] soundDataList =  new SoundData[0];
	
	GameObject musicGameObject = null;
	Queue<GameObject> soundObjectList = new Queue<GameObject>();
	Dictionary<string, AudioClip> audioClipAssets = new Dictionary<string, AudioClip>();
	Dictionary<string, GameObject> audioLoop = new Dictionary<string, GameObject>();

	//for non-loopable SFX
	Dictionary<string, GameObject> audioSFX = new Dictionary<string, GameObject>();

	public void PlayBGM(string soundKey, bool loop = true, float delay = -1.0f)
	{
		if (musicGameObject == null)
		{
			musicGameObject = GetSoundObject();
			musicGameObject.name = "BGMObject";
			musicGameObject.GetComponent<AudioSource>().ignoreListenerVolume = true;
			musicGameObject.GetComponent<AudioSource>().volume = bgmVolume;
		}
		SoundInstanceManager soundInstance = musicGameObject.GetComponent<SoundInstanceManager>();
		soundInstance.Play(GetAudioClip(soundKey), loop, delay, 1);
	}

	public void StopBGM()
	{
		musicGameObject.GetComponent<AudioSource>().Stop();
		StoreAudioObject(musicGameObject);
		musicGameObject = null;
	}

	public void Pause()
	{
		if (musicGameObject != null) musicGameObject.GetComponent<AudioSource>().Pause();
		AudioListener.pause = true;
	}

	public void Unpause()
	{
		if (musicGameObject != null) musicGameObject.GetComponent<AudioSource>().Play();
		AudioListener.pause = false;
	}

	//added by zee
	public void PauseBGM()
	{
		if (musicGameObject != null) musicGameObject.GetComponent<AudioSource>().Pause();
		isBGMPaused = true;
	}
	
	public void UnpauseBGM()
	{
		if (musicGameObject != null) musicGameObject.GetComponent<AudioSource>().Play();
		isBGMPaused = false;
	}

	//added by zee
	public void FadeInBGM()
	{

			if (this.GetIsBGMPause())
				this.UnpauseBGM();
			
			this.fadeInTime += Time.deltaTime;
			this.BGMVolume = fadeInTime/duration;
			
			if (this.BGMVolume >= 1.0f)
			{
				
				this.fadeInTime = 0.0f;
			}
			

	}
	
	public void FadeOutBGM()
	{
			this.fadeOutTime -= Time.deltaTime;
			
			this.BGMVolume = fadeOutTime/duration;
			
			if (this.BGMVolume <= 0.0f)
			{
				this.PauseBGM();
				this.fadeOutTime = 1.0f;
			}

	}

	public bool GetIsBGMPause()
	{
		return isBGMPaused;
	}

	public void PlaySFX(string soundKey, float delay = 0, float pitch = 1)
	{
		GameObject gObj = GetSoundObject ();
		gObj.GetComponent<SoundInstanceManager>().Play(GetAudioClip(soundKey), false, delay, pitch);

		if(!audioSFX.ContainsKey(soundKey))
		{
			audioSFX.Add (soundKey, gObj);
		}
	}

	public void PlayLoopSFX(string soundKey, float delay = 0, float pitch = 1)
	{
		if (audioLoop.ContainsKey(soundKey)) return;
		GameObject gobj = GetSoundObject();
		gobj.GetComponent<SoundInstanceManager>().Play(GetAudioClip(soundKey), true, delay, pitch);
		audioLoop.Add(soundKey, gobj);
	}

	public void StopLoopSFX(string soundKey)
	{
		if (!audioLoop.ContainsKey(soundKey)) return;

		this.StoreAudioObject(audioLoop[soundKey]);
		GameObject soundObj = audioLoop [soundKey];
		soundObj.GetComponent<SoundInstanceManager> ().Stop ();
		audioLoop.Remove(soundKey);
	}

	//added by zee
	public void StopSFX(string soundKey)
	{
		if(!audioSFX.ContainsKey(soundKey)) return;

		GameObject soundObj = audioSFX [soundKey];

		if(soundObj == null) return;

		soundObj.GetComponent<SoundInstanceManager> ().Stop ();
		audioSFX.Remove (soundKey);
	}

	GameObject GetSoundObject()
	{
		GameObject gobj = null;
		while (soundObjectList.Count > 0)
		{
			gobj = soundObjectList.Dequeue();
			if (gobj == null) continue;
			gobj.SetActive(true);
			gobj.name = "SFXObject";
			gobj.GetComponent<AudioSource>().ignoreListenerVolume = false;
			gobj.GetComponent<AudioSource>().volume = 0.5f;
			return gobj;
		}

		gobj = new GameObject("SFXObject");
		gobj.AddComponent<SoundInstanceManager>();
		return gobj;
	}

	AudioClip GetAudioClip(string soundKey)
	{
		if (audioClipAssets.ContainsKey(soundKey)) return audioClipAssets[soundKey];

		for (int i = 0; i < soundDataList.Length; i++)
		{
			if (soundDataList[i].soundKey == soundKey)
			{
				AudioClip clip =  Resources.Load(resourceLocation + "/" + soundDataList[i].soundFile, typeof(AudioClip)) as AudioClip;
				audioClipAssets.Add(soundKey, clip);
				return clip;
			}
		}

		Debug.LogWarning(soundKey + " Not found!");
		return null;
	}

	public void StoreAudioObject(GameObject gobj)
	{
		if (gobj == null) return;
		gobj.SetActive(false);
		if (gobj.GetComponent<AudioSource>().loop) Destroy(gobj);
		else soundObjectList.Enqueue(gobj);
	}
	
	#if UNITY_EDITOR
	public void Add()
	{
		ArrayUtility.Add<SoundData>(ref soundDataList, new SoundData());
	}
	public void Remove(int index)
	{
		if (index < 0 || index >= soundDataList.Length) return;
		ArrayUtility.RemoveAt<SoundData>(ref soundDataList, index);
	}
	#endif
	
	[System.Serializable]
	public class SoundData
	{
		public string soundKey;
		public string soundFile;
	}
}
