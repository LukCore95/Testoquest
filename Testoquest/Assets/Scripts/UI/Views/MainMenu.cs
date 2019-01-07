using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIView
{
	[SerializeField] private ChooseBase _chooseBase;
	[SerializeField] private Options _options;
	[SerializeField] private YesOrNo _exit;
	[SerializeField] private Button _startButton;
	[SerializeField] private Button _optionsButton;
	[SerializeField] private Button _closeButton;

	private void Start()
	{
		UIManager.Instance.GoToView(this);
		OptionsManager.CheckIfFirstTime();
		_startButton.onClick.AddListener(GoToChooseQuestionBase);
		_optionsButton.onClick.AddListener(GoToOptions);
		_closeButton.onClick.AddListener(GoToExit);
	}

	public override void OnEnable()
	{
		UIManager.Instance.Background.DisableAll();
		UIManager.Instance.Background.SoundToggle.gameObject.SetActive(true);
		UIManager.Instance.Background.AboutUsButton.gameObject.SetActive(true);
	}

	public override void OnDisable()
	{
		
	}

	private void GoToExit()
	{
		_exit.SetFromMainMenu();
		UIManager.Instance.GoToView(_exit);
	}

	private void GoToOptions()
	{
		UIManager.Instance.GoToView(_options);
	}

	private void GoToChooseQuestionBase()
	{
		UIManager.Instance.GoToView(_chooseBase);
	}
}
