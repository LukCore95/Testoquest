using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AddEditQuestion : UIView
{
	[SerializeField] private InputField _questionNameInputField;
	[SerializeField] private InputField _questionTextInputField;
	[SerializeField] private Button _addAnswerButton;
	[SerializeField] private Button _saveQuestionButton;
	[SerializeField] private Transform _content;
	[SerializeField] private AnswerEditScrollViewElement _answerPrefab;
	[SerializeField] private QuestionDataBase _dataBase;
	[SerializeField] private string _questionName;
	

	private void Start()
	{
		_addAnswerButton.onClick.AddListener(AddNewAnswer);
		_saveQuestionButton.onClick.AddListener(SaveQuestion);
	}

	public override void OnEnable()
	{
		UIManager.Instance.Background.DisableAll();
		UIManager.Instance.Background.SetBackButtonListener(UIManager.Instance.GoBack);
		UIManager.Instance.Background.BackButton.gameObject.SetActive(true);
		SetView();
	}
	
	public override void OnDisable()
	{
		ClearView();
	}

	private void ClearView()
	{
		_questionNameInputField.text = null;
		_questionTextInputField.text = null;
		_questionNameInputField.interactable = true;
		_questionName = null;
		ClearScrollView();
	}

	private void ClearScrollView()
	{
		foreach (Transform child in _content)
		{
			Destroy(child.gameObject);
		}
	}
	private void SetView()
	{
		ClearScrollView();
		if (_questionName != null)
		{
			_questionNameInputField.text = _questionName;
			_questionNameInputField.interactable = false;
			_questionTextInputField.text = _dataBase.Questions.Find(q => q.QuestionName == _questionName).QuestionText;
			SetScrollView();
		}
	}
	
	private void SetScrollView()
	{
		Question question = _dataBase.Questions.Find(q => q.QuestionName == _questionName);
		if (question != null)
		{
			foreach (Answer answer  in question.Answers)
			{
				AnswerEditScrollViewElement newContentElementScript = Instantiate(_answerPrefab,
					_content);
				newContentElementScript.SetAnswer(answer);
			}
		}
	}

	public void SetQuestionToEdit(QuestionDataBase baseToEdit, Question question)
	{
		_dataBase = baseToEdit;
		_questionName = question.QuestionName;
	}

	private void AddNewAnswer()
	{
		AnswerEditScrollViewElement newContentElementScript = Instantiate(_answerPrefab,
			_content);
		newContentElementScript.SetAnswer(null);
	}

	private void SaveQuestion()
	{
		if (_questionNameInputField.text != "" && _questionTextInputField.text != "" && _content.childCount > 1)
		{
			Question newQuestion = new Question();
			newQuestion.QuestionName = _questionNameInputField.text;
			newQuestion.QuestionText = _questionTextInputField.text;
			foreach (Transform answerChild in _content)
			{
				newQuestion.Answers.Add(answerChild.GetComponent<AnswerEditScrollViewElement>().Answer);
			}

			QuestionDataBaseManager.Instance.AddNewQuestion(_dataBase,newQuestion);

			UIManager.Instance.GoBack();
		}
	}
}
