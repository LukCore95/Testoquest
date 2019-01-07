using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] private AudioSource _musicAudioSource;
	[SerializeField] private AudioSource _soundAudioSource;
	[SerializeField] private AudioClip _correctAudioClip;
	[SerializeField] private AudioClip _wrongAudioClip;
	[SerializeField] private bool _isMuted;

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
		if (!_isMuted)
		{
			_soundAudioSource.clip = _correctAudioClip;
			_soundAudioSource.Play();
		}
	}

	public void PlayWrongSFX()
	{
		if (!_isMuted)
		{
			_soundAudioSource.clip = _wrongAudioClip;
			_soundAudioSource.Play();
		}
	}

	public void SetAudioSources(bool value)
	{
		_isMuted = !value;
		if (_isMuted)
		{
			_musicAudioSource.Pause();
		}
		else
		{
			_musicAudioSource.Play();
		}
	}
}
