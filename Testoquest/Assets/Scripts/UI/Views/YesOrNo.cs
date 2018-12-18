using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesOrNo : UIView
{
	[SerializeField] private Text _questionText;
	[SerializeField] private Button _yesButton;
	[SerializeField] private Button _noButton;

	public override void OnEnable()
	{
		
	}

	public override void OnDisable()
	{
		
	}

	public void SetFromMainMenu()
	{
		UIManager.Instance.Background.DisableAll();

		ResetButtons();
		_questionText.text = "Czy na pewno chcesz wyjść?";
		_noButton.onClick.AddListener(UIManager.Instance.GoBack);
		_yesButton.onClick.AddListener(Application.Quit);
	}

	private void ResetButtons()
	{
		_noButton.onClick.RemoveAllListeners();
		_yesButton.onClick.RemoveAllListeners();
	}

	public void SetFromChooseBaseResetButton(QuestionDataBase contentElementQuestionDataBase)
	{
		throw new System.NotImplementedException();
	}

	public void SetFromChooseBaseDeleteButton(QuestionDataBase contentElementQuestionDataBase)
	{
		throw new System.NotImplementedException();
	}
}
