using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] private AudioSource _musicAudioSource;
	[SerializeField] private AudioSource _soundAudioSource;
	[SerializeField] private AudioClip _correctAudioClip;
	[SerializeField] private AudioClip _wrongAudioClip;

	public void SetMusicVolume(float volume)
	{
		_musicAudioSource.volume = volume;
	}

	public void SetSoundVolume(float volume)
	{
		_soundAudioSource.volume = volume;
	}

	public void PlayCorrectSFX()
	{
		_soundAudioSource.clip = _correctAudioClip;
		_soundAudioSource.Play();
	}

	public void PlayWrongSFX()
	{
		_soundAudioSource.clip = _wrongAudioClip;
		_soundAudioSource.Play();
	}
}
