﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OptionsManager : Singleton<OptionsManager>
{
	public float MusicVolume
	{
		get { return _musicVolume; }
		set
		{
			_musicVolume = value;
			//AudioMixer set
		}
	}

	public float SoundVolume
	{
		get { return _soundVolume; }
		set
		{
			_soundVolume = value;
			//AudioMixer set
		}
	}

	public bool Vibrations
	{
		get { return _vibrations; }
		set { _vibrations = value; }
	}
	
	public int StartingRepeatQuestionsNumber
	{
		get { return _startingRepeatQuestionsNumber; }
		set{ _startingRepeatQuestionsNumber = value; }
	}

	public int RepeatQuestionsNumber
	{
		get { return _repeatQuestionsNumber; }
		set{ _repeatQuestionsNumber = value; }
	}

	public TimeSpan TimeForAnswer
	{
		get { return _timeForAnswer; }
		set { _timeForAnswer = value; }
	}

	public bool LearnWithoutGame
	{
		get { return _learnWithoutGame; }
		set { _learnWithoutGame = value; }
	}

	[SerializeField] private float _musicVolume;
	[SerializeField] private float _soundVolume;
	[SerializeField] private bool _vibrations;
	[SerializeField] private int _repeatQuestionsNumber;
	[SerializeField] private int _startingRepeatQuestionsNumber;
	[SerializeField] private TimeSpan _timeForAnswer;
	[SerializeField] private bool _learnWithoutGame;


	private void Start()
	{
		ResetOptions();
	}

	public void ResetOptions()
	{
		PlayerPrefsManager.SetOptionsFromPlayerPrefs();
	}

	public void SaveOptions(Options options)
	{
		PlayerPrefsManager.SaveOptions();
	}

	public void GetOptionsToUI(Options optionsUI)
	{
		optionsUI.MusicSlider.value = MusicVolume;
		optionsUI.SoundSlider.value = SoundVolume;
		optionsUI.VibrationsToggle.isOn = Vibrations;
		optionsUI.RepeatQuestionsInputField.text = RepeatQuestionsNumber.ToString();
		optionsUI.StartingRepeatQuestionsInputField.text = StartingRepeatQuestionsNumber.ToString();
		optionsUI.TimeForAnswerImInputField.text = TimeForAnswer.ToString(@"mm\:ss");
		optionsUI.LearnWithoutGameToggle.isOn = LearnWithoutGame;
	}
}