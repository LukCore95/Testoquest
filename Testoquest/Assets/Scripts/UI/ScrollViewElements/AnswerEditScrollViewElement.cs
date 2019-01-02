using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerEditScrollViewElement : MonoBehaviour
{
	public Answer Answer;
	[SerializeField] private InputField _answerTextInputField;
	[SerializeField] private Toggle _isCorrectAnswerToogle;
	[SerializeField] private Button _deleteButton;

	public void SetAnswer(Answer answer)
	{
		Answer = new Answer("", false);
		if (answer != null)
		{
			Answer.AnswerText = answer.AnswerText;
			Answer.IsCorrect = answer.IsCorrect;
		}
		_isCorrectAnswerToogle.isOn = Answer.IsCorrect;
		_answerTextInputField.text = Answer.AnswerText;
		_isCorrectAnswerToogle.onValueChanged.AddListener(OnToggleValueChanged);
		_answerTextInputField.onEndEdit.AddListener(OnAnswerTextInputFieldEndEdit);
		_deleteButton.onClick.AddListener(OnDestroyClicked);
	}

	private void OnDestroyClicked()
	{
		Destroy(gameObject);
	}

	private void OnAnswerTextInputFieldEndEdit(string text)
	{
		Answer.AnswerText = text;
	}

	private void OnToggleValueChanged(bool value)
	{
		Answer.IsCorrect = value;
	}
}
