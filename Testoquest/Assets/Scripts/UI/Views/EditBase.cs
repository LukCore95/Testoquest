using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditBase : UIView
{
	[SerializeField] private Text _baseNameText;
	[SerializeField] private AddEditQuestion _addEditQuestion;
	[SerializeField] private YesOrNo _yesOrNo;
	[SerializeField] private ChooseBase _chooseBase;
	[SerializeField] private Transform _content;
	[SerializeField] private QuestionDataBase _baseToEdit;
	[SerializeField] Button _addQuestionButton;
	[SerializeField] Button _goBackButton;
	[SerializeField] private QuestionScrollViewElement _questionScrollViewElementPrefab;


	private void Start()
	{
		_addQuestionButton.onClick.AddListener(AddNewQuestion);
		_goBackButton.onClick.AddListener(GoBack);
	}

	private void GoBack()
	{
		UIManager.Instance.DeleteFromHistory(this);
		UIManager.Instance.DeleteFromHistory(_chooseBase);
		UIManager.Instance.GoToView(_chooseBase);
	}

	public override void OnEnable()
	{
		UIManager.Instance.Background.DisableAll();
		ClearScrollView();
		SetScrollView();
	}

	public override void OnDisable()
	{
		ClearScrollView();
	}

	private void ClearScrollView()
	{
		foreach (Transform child in _content.transform)
		{
			Destroy(child.gameObject);
		}
	}

	private void SetScrollView()
	{
		foreach (Question question in _baseToEdit.Questions)
		{
			QuestionScrollViewElement newContentElementScript = Instantiate(_questionScrollViewElementPrefab,
				_content);
			newContentElementScript.SetQuestion(question);
			newContentElementScript.EditButton.onClick.AddListener(
				delegate
				{
					ContentElementOnEdit(newContentElementScript.Question);
				});
			newContentElementScript.DeleteButton.onClick.AddListener(
				delegate
				{
					ContentElementOnDelete(newContentElementScript.Question);
				});
		}
	}

	private void ContentElementOnDelete(Question question)
	{
		_yesOrNo.SetFromDeleteQuestionButton(_baseToEdit, question);
		UIManager.Instance.GoToView(_yesOrNo);
	}

	private void ContentElementOnEdit(Question question)
	{
		_addEditQuestion.SetQuestionToEdit(_baseToEdit, question);
		UIManager.Instance.GoToView(_addEditQuestion);
	}

	public void SetEditBase(QuestionDataBase questionDataBase)
	{
		_baseToEdit = questionDataBase;
		_baseNameText.text = _baseToEdit.Name;
	}

	private void AddNewQuestion()
	{
		Question question = new Question();
		_addEditQuestion.SetQuestionToEdit(_baseToEdit,question);
		UIManager.Instance.GoToView(_addEditQuestion);
	}
}
