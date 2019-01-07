using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
	public Button BackButton;
	public Button AboutUsButton;
	public AboutApp AboutAppView;
	public Toggle SoundToggle;
	public Image SoundOffImage;

	private void Start()
	{
		SoundToggle.onValueChanged.AddListener(SoundToggle_OnValueChanged);
		SoundToggle_OnValueChanged(SoundToggle.isOn);
		AboutUsButton.onClick.AddListener(GoToAboutApp);
	}

	public void DisableAll()
	{
		BackButton.gameObject.SetActive(false);
		SoundToggle.gameObject.SetActive(false);
		AboutUsButton.gameObject.SetActive(false);
	}

	public void SetBackButtonListener(UnityAction action)
	{
		BackButton.onClick.RemoveAllListeners();
		BackButton.onClick.AddListener(action);
	}

	private void SoundToggle_OnValueChanged(bool value)
	{
		SoundOffImage.gameObject.SetActive(!value);
		AudioManager.Instance.SetAudioSources(value);
	}

	private void GoToAboutApp()
	{
		UIManager.Instance.GoToView(AboutAppView);
	}
}
