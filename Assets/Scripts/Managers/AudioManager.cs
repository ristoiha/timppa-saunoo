using ProjectEnums;
using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using System.Collections.Generic;
using MEC; // This is needed for Dictionary

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;
	public static Dictionary<AudioID, AudioEntryAsset> audioDictionary = new Dictionary<AudioID, AudioEntryAsset>();
	public static int cutsceneSkippedFrame;


	public AudioSource[] effectsSources;
	public AudioSource[] musicSources;
	public AudioSource[] voiceOverSources;
	public AudioMixer audioMixer;  // Reference to the Audio Mixer
	public AudioCollectionAsset audioCollection;
	public GameObject spatialAudioPrefab;

	public AudioID[] musics;
	public AudioClip[] glassShardSound;
	private AudioListener mainAudioListener;

	public string volumeParameter = "MasterVolume";  // Name of the exposed parameter for volume

	private Dictionary<AudioID, AudioSource> stopLoopDictionary = new Dictionary<AudioID, AudioSource>();

	private float previousMusicVolume;
	private AudioSource currentMusicSource;

	private void Awake() {
		if (instance == null) {
			instance = this;
			InitializeAudioDictionary();
			SubscribeEvents();
		}
		else {
			Destroy(gameObject);
		}
	}

	private void OnDestroy() {
		if (instance == this) {
			instance = null;
			UnsubscribeEvents();
		}
	}

	private void SubscribeEvents() {
		FloatVariable.RegisterListener(LocID.Option_MasterVolume, UpdateMasterVolume);
		FloatVariable.RegisterListener(LocID.Option_SFXVolume, UpdateEffectsVolume);
		FloatVariable.RegisterListener(LocID.Option_MusicVolume, UpdateMusicVolume);
		FloatVariable.RegisterListener(LocID.Option_VOVolume, UpdateVoiceOverVolume);
		AudioIDVariable.RegisterListener(LocID.Action_EndAudio, PlayEffectEnd);
	}

	private void UnsubscribeEvents() {
		FloatVariable.UnregisterListener(LocID.Option_MasterVolume, UpdateMasterVolume);
		FloatVariable.UnregisterListener(LocID.Option_SFXVolume, UpdateEffectsVolume);
		FloatVariable.UnregisterListener(LocID.Option_MusicVolume, UpdateMusicVolume);
		FloatVariable.UnregisterListener(LocID.Option_VOVolume, UpdateVoiceOverVolume);
		AudioIDVariable.UnregisterListener(LocID.Action_EndAudio, PlayEffectEnd);
	}

	private void InitializeAudioDictionary() {
		for (int i = 0; i < audioCollection.musics.Length; i++) {
			if (audioDictionary.ContainsKey(audioCollection.musics[i].id) == false) {
				audioDictionary.Add(audioCollection.musics[i].id, audioCollection.musics[i]);
			}
		}
		for (int i = 0; i < audioCollection.effects.Length; i++) {
			if (audioDictionary.ContainsKey(audioCollection.effects[i].id) == false) {
				audioDictionary.Add(audioCollection.effects[i].id, audioCollection.effects[i]);
			}
		}
	}

	private void PlayEffectEnd(AudioID audioID) {
		if (stopLoopDictionary.ContainsKey(audioID) == true) {
			stopLoopDictionary[audioID].Stop();
			stopLoopDictionary[audioID].loop = false;
			stopLoopDictionary.Remove(audioID);
		}
		if (audioDictionary[audioID].clipEnd != null) {
			AudioSource source = GetAvailableAudioSource(AudioSourceType.Effect);
			source.clip = audioDictionary[audioID].clipEnd;
			source.volume = audioDictionary[audioID].volume;
			source.Play();
		}
	}

	public void FadeOutMusicDuringVoiceOver(bool fadeIn) {
		float currentMusicVolume;
		float currentVOVolume;
		AudioManager.instance.audioMixer.GetFloat(MixerVolumeParameter.VOMaster.ToString(), out currentVOVolume); // Get the current voice over volume
		AudioManager.instance.audioMixer.GetFloat(MixerVolumeParameter.MusicMaster.ToString(), out currentMusicVolume); // Get the current music volume
		if (fadeIn == true) {
			previousMusicVolume = currentMusicVolume;
			if (currentMusicVolume + 20f > currentVOVolume) {
				Timing.RunCoroutine(FadeMusicToWantedLevel(Tools.LinearToDecibel(false, currentMusicVolume), Tools.LinearToDecibel(false, currentVOVolume - 20f)));
			}
		}
		else {
			Timing.RunCoroutine(FadeMusicToWantedLevel(Tools.LinearToDecibel(false, currentMusicVolume), Tools.LinearToDecibel(false, previousMusicVolume)));
		}
	}

	public IEnumerator<float> FadeMusicToWantedLevel(float currentVolume, float volumeToFade) {
		float timeElapsed =0;
		while (timeElapsed < Gval.crossFadeDuration) {
			float t = timeElapsed / Gval.crossFadeDuration;
			float newVolume1 = Mathf.Lerp(currentVolume, volumeToFade, t); // Fade 
			UpdateMusicVolume(newVolume1);
			timeElapsed += Time.deltaTime;
			yield return Timing.WaitForOneFrame;
		}
	}

	public static void PlayAudio(AudioID audioID) {
		if ((int)audioID < 100) {
			PlayMusic(audioID);
		}
		else {
			PlayEffect(audioID);
		}
	}

	private static void PlayMusic(AudioID audio) {
		AudioSource previousSource = AudioManager.instance.GetCurrentMusicSource();
		AudioSource source = AudioManager.instance.GetAvailableAudioSource(AudioSourceType.Music);
		source.clip = audioDictionary[audio].clip;
		source.volume = audioDictionary[audio].volume;
		source.Play();
		AudioManager.instance.currentMusicSource = source;
		if (previousSource != null) {
			Timing.RunCoroutine(AudioManager.instance.FadeAudiosource(previousSource, 0F));
		}
	}

	private IEnumerator<float> FadeAudiosource(AudioSource source, float targetVolume) {
		float startVolume = source.volume;
		for (float i = 0F; i < 1F; i += Time.unscaledDeltaTime) {
			source.volume = Mathf.Lerp(startVolume, targetVolume, i);
			yield return Timing.WaitForOneFrame;
		}
		source.volume = targetVolume;
		if (targetVolume == 0F) {
			source.Stop();
		}
	}

	public static void StopMusic() {
		for (int i = 0; i < AudioManager.instance.musicSources.Length; i++) {
			AudioManager.instance.musicSources[i].Stop();
		}
	}

	private AudioSource GetAvailableAudioSource(AudioSourceType sourceType) {
		AudioSource[] sourceArray = musicSources;
		if (sourceType == AudioSourceType.Music) {
			sourceArray = musicSources;
		}
		else if (sourceType == AudioSourceType.Effect) {
			sourceArray = effectsSources;
		}
		else if (sourceType == AudioSourceType.VoiceOver) {
			sourceArray = voiceOverSources;
		}
		for (int i = 0; i < sourceArray.Length; i++) {
			if (sourceArray[i].isPlaying == false && stopLoopDictionary.ContainsValue(sourceArray[i]) == false && sourceArray[i] != currentMusicSource) {
				return sourceArray[i];
			}
		}
		Debug.LogError("No free audiosources found");
		return null;
	}

	public AudioSource GetCurrentMusicSource() {
		return currentMusicSource;
	}

	public void SetVolumeTrackParameter(string trackName, float linearVolume) {
		float logVolume = Tools.ConvertLinearVolumeToLogarithmic(linearVolume);
		audioMixer.SetFloat(trackName, logVolume);
	}

	private static void PlayEffect(AudioID audioID) {
		AudioSource source = AudioManager.instance.GetAvailableAudioSource(AudioSourceType.Effect);
		source.clip = audioDictionary[audioID].clip;
		source.volume = audioDictionary[audioID].volume;
		source.Play();
		if (audioDictionary[audioID].clipLoop != null) {
			if (AudioManager.instance.stopLoopDictionary.ContainsKey(audioID) == false) {
				AudioSource loopSource = AudioManager.instance.GetAvailableAudioSource(AudioSourceType.Effect);
				loopSource.clip = audioDictionary[audioID].clipLoop;
				loopSource.volume = audioDictionary[audioID].volume;
				loopSource.loop = true;
				AudioManager.instance.stopLoopDictionary.Add(audioID, loopSource);
				loopSource.PlayScheduled(AudioSettings.dspTime + audioDictionary[audioID].clip.length);
			}
		}
	}

	public static void PlayEffect3D(AudioID audio, Vector3 position) {
		AudioSource spatialAudioSource = SpawnAudioSource(position);
		spatialAudioSource.clip = audioDictionary[audio].clip;
		spatialAudioSource.volume = audioDictionary[audio].volume;
		spatialAudioSource.Play();
		Destroy(spatialAudioSource.gameObject, audioDictionary[audio].clip.length + 0.1F);
	}

	public static void PlayEffect3D(AudioID audio, Transform parent) {
		AudioSource spatialAudioSource = SpawnAudioSource(parent.position);
		spatialAudioSource.transform.SetParent(parent);
		spatialAudioSource.clip = audioDictionary[audio].clip;
		spatialAudioSource.volume = audioDictionary[audio].volume;
		spatialAudioSource.Play();
		Destroy(spatialAudioSource.gameObject, audioDictionary[audio].clip.length + 0.1F);
	}

	private static AudioSource SpawnAudioSource(Vector3 position) {
		GameObject audioSourceObject = Instantiate(AudioManager.instance.spatialAudioPrefab, position, Quaternion.identity);
		return audioSourceObject.GetComponent<AudioSource>();
	}

	public AudioSource GetSpeechAudioSource(TextAudioPairAsset textAudioPairAsset){
		AudioSource source = GetAvailableAudioSource(AudioSourceType.VoiceOver);
		source.clip = textAudioPairAsset.audio[(int)LocalizationManager.instance.selectedLanguage];
		source.Play();
		return source;
	}

	public void StopSpeechAudioSource() {
		for (int i = 0; i < voiceOverSources.Length; i++) {
			voiceOverSources[i].Stop();
		}
	}

	public static float GetAudioLength(AudioID audio) {
		return audioDictionary[audio].clip.length;
	}

	public static void UpdateMasterVolume(float linearVolume) {
		float logVolume = Tools.ConvertLinearVolumeToLogarithmic(linearVolume);
		AudioManager.instance.audioMixer.SetFloat(MixerVolumeParameter.Master.ToString(), logVolume);
		UserProfile.SaveCurrent();

	}
	public static void UpdateVoiceOverVolume(float linearVolume) {
		float logVolume = Tools.ConvertLinearVolumeToLogarithmic(linearVolume);
		AudioManager.instance.audioMixer.SetFloat(MixerVolumeParameter.VOMaster.ToString(), logVolume);
		UserProfile.SaveCurrent();

	}
	public static void UpdateEffectsVolume(float linearVolume) {
		float logVolume = Tools.ConvertLinearVolumeToLogarithmic(linearVolume);
		AudioManager.instance.audioMixer.SetFloat(MixerVolumeParameter.SFXMaster.ToString(), logVolume);
		UserProfile.SaveCurrent();
	}

	public static void UpdateMusicVolume(float linearVolume) {
		float logVolume = Tools.ConvertLinearVolumeToLogarithmic(linearVolume);
		AudioManager.instance.audioMixer.SetFloat(MixerVolumeParameter.MusicMaster.ToString(), logVolume);
		UserProfile.SaveCurrent();
	}

	public static void LoadAudioSettings() {
		float masterVolume = Tools.ConvertLinearVolumeToLogarithmic(FloatVariable.GetValue(LocID.Option_MasterVolume));
		float effectsVolume = Tools.ConvertLinearVolumeToLogarithmic(FloatVariable.GetValue(LocID.Option_SFXVolume));
		float musicVolume = Tools.ConvertLinearVolumeToLogarithmic(FloatVariable.GetValue(LocID.Option_MusicVolume));
		float voiceOverVolume = Tools.ConvertLinearVolumeToLogarithmic(FloatVariable.GetValue(LocID.Option_VOVolume));
		AudioManager.instance.audioMixer.SetFloat(MixerVolumeParameter.Master.ToString(), masterVolume);
		AudioManager.instance.audioMixer.SetFloat(MixerVolumeParameter.SFXMaster.ToString(), effectsVolume);
		AudioManager.instance.audioMixer.SetFloat(MixerVolumeParameter.MusicMaster.ToString(), musicVolume);
		AudioManager.instance.audioMixer.SetFloat(MixerVolumeParameter.VOMaster.ToString(), voiceOverVolume);
	}

}

namespace ProjectEnums {
	public enum AudioSourceType {
		Music,
		Effect,
		VoiceOver,
		//Ambient,
	}
}

