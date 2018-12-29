using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class YesOrNo : UIView
{
	[SerializeField] private Text _questionText;
	[SerializeField] private Button _yesButton;
	[SerializeField] private Button _noButton;


	public override void OnEnable()
	{
		UIManager.Instance.Background.DisableAll();
	}

	public override void OnDisable()
	{
		ResetButtons();
	}

	public void SetFromMainMenu()
	{
		UIManager.Instance.Background.DisableAll();

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
		_questionText.text = "Czy na pewno chcesz zresetoweać postęp bazy " + contentElementQuestionDataBase.Name+"?";
		_noButton.onClick.AddListener(UIManager.Instance.GoBack);
		_yesButton.onClick.AddListener(delegate
		{
			QuestionDataBaseManager.Instance.ResetDataBaseState(contentElementQuestionDataBase.Name);
			UIManager.Instance.GoBack();
		});
	}

	public void SetFromChooseBaseSelectBaseButton(QuestionDataBase contentElementQuestionDataBase, UnityAction GoToGameMethod)
	{	
		_questionText.text = "Wybór bazy " + contentElementQuestionDataBase.Name+" sprawi reset postępu, kontynuować?";
		_noButton.onClick.AddListener(UIManager.Instance.GoBack);
		_yesButton.onClick.AddListener(delegate
		{
			QuestionDataBaseManager.Instance.ResetDataBaseState(contentElementQuestionDataBase.Name);
			GoToGameMethod.Invoke();
		});
	}

	public void SetFromChooseBaseDeleteButton(QuestionDataBase contentElementQuestionDataBase)
	{
		_questionText.text = "Czy na pewno chcesz usunąć bazę " + contentElementQuestionDataBase.Name + "? To usunie tylko bazę z aplikacji, folder z bazą pozostanie na telefonie.";
		_noButton.onClick.AddListener(UIManager.Instance.GoBack);
		_yesButton.onClick.AddListener(delegate
		{
			QuestionDataBaseManager.Instance.DeleteDataBase(contentElementQuestionDataBase.Name);
			UIManager.Instance.GoBack();
		});
	}
}
