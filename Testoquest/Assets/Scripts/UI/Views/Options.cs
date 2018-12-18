using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Options : UIView
{
	public Slider MusicSlider;
	public Slider SoundSlider;
	public Toggle VibrationsToggle;
	public InputField StartingRepeatQuestionsInputField;
	public InputField RepeatQuestionsInputField;
	public InputField TimeForAnswerImInputField;
	public Toggle LearnWithoutGameToggle;

	[SerializeField] private Button SaveButton;
	[SerializeField] private string previousTimeForAnswerState;

	private void Start()
	{
		SaveButton.onClick.AddListener(SaveAndGoBack);
		MusicSlider.onValueChanged.AddListener(SetMusic);
		SoundSlider.onValueChanged.AddListener(SetSound);
		VibrationsToggle.onValueChanged.AddListener(SetVibrations);
		RepeatQuestionsInputField.onEndEdit.AddListener(SetRepeats);
		StartingRepeatQuestionsInputField.onEndEdit.AddListener(SetStartingRepeats);
		TimeForAnswerImInputField.onEndEdit.AddListener(SetTimeForAnswer);
		LearnWithoutGameToggle.onValueChanged.AddListener(SetLearnWithoutGame);
	}



	public override void OnEnable()
	{
		OptionsManager.Instance.GetOptionsToUI(this);
		previousTimeForAnswerState = TimeForAnswerImInputField.text;
		UIManager.Instance.Background.DisableAll();
		UIManager.Instance.Background.BackButton.gameObject.SetActive(true);
		UIManager.Instance.Background.SetBackButtonListener(GoBackAndResetOptions);
	}

	private void GoBackAndResetOptions()
	{
		OptionsManager.Instance.ResetOptions();
		UIManager.Instance.GoBack();
	}

	public override void OnDisable()
	{

	}

	private void SetLearnWithoutGame(bool arg0)
	{
		OptionsManager.Instance.LearnWithoutGame = arg0;
	}

	private void SetTimeForAnswer(string arg0)
	{
		TimeSpan newSpan;
		if (!TimeSpan.TryParseExact(arg0, @"mm\:ss", CultureInfo.CurrentCulture, TimeSpanStyles.None, out newSpan))
		{
			TimeForAnswerImInputField.text = previousTimeForAnswerState;
		}
		else
		{
			OptionsManager.Instance.TimeForAnswer = newSpan;
		}
	}

	private void SetRepeats(string arg0)
	{
		OptionsManager.Instance.RepeatQuestionsNumber = int.Parse(arg0);
	}

	private void SetStartingRepeats(string arg0)
	{
		OptionsManager.Instance.StartingRepeatQuestionsNumber = int.Parse(arg0);
	}

	private void SetVibrations(bool arg0)
	{
		OptionsManager.Instance.Vibrations = arg0;
	}

	private void SetSound(float arg0)
	{
		OptionsManager.Instance.SoundVolume = arg0;
	}

	private void SetMusic(float arg0)
	{
		OptionsManager.Instance.MusicVolume = arg0;
	}

	private void SaveAndGoBack()
	{
		OptionsManager.Instance.SaveOptions(this);
		UIManager.Instance.GoBack();
	}


}
