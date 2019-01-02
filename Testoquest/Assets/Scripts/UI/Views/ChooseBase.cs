using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseBase : UIView
{
	public AddBase AddBase;
	public Game Game;
	public EditBase EditBase;
	public YesOrNo YesOrNo;
	public Button AddBaseButton;
	public Button ChooseButton;
	public QuestionDataBaseScrollViewElement SelectedBase;
	public Transform Content;
	public ToggleGroup ContentToggleGroup;
	public GameObject QuestionBaseContentElementPrefab;

	private void Start()
	{
		AddBaseButton.onClick.AddListener(AddBaseButton_OnClicked);
		ChooseButton.onClick.AddListener(ChooseButton_OnClicked);
	}

	public override void OnEnable()
	{
		UIManager.Instance.Background.DisableAll();
		UIManager.Instance.Background.BackButton.gameObject.SetActive(true);
		UIManager.Instance.Background.SetBackButtonListener(UIManager.Instance.GoBack);
		ClearScrollView();
		SetScrollView();
	}

	public override void OnDisable()
	{
		ClearScrollView();
	}

	private void ClearScrollView()
	{
		foreach (Transform child in Content.transform)
		{
			Destroy(child.gameObject);
		}
	}

	private void SetScrollView()
	{
		foreach (QuestionDataBase questionDataBase in QuestionDataBaseManager.Instance.QuestionDataBases)
		{
			GameObject newContentElement =Instantiate(QuestionBaseContentElementPrefab,
				Content);
			QuestionDataBaseScrollViewElement newContentElementScript =
				newContentElement.GetComponent<QuestionDataBaseScrollViewElement>();
			newContentElementScript.SetQuestionBase(questionDataBase);
			newContentElementScript.ChooseBaseToggle.group = ContentToggleGroup;
			newContentElementScript.ChooseBaseToggle.onValueChanged.AddListener(
				delegate
				{
					ContentElementOnToggled(newContentElementScript.ChooseBaseToggle, newContentElementScript);
				});
			newContentElementScript.EditButton.onClick.AddListener(
				delegate
				{
					ContentElementOnEdit(newContentElementScript.QuestionDataBase);
				});
			newContentElementScript.ResetButton.onClick.AddListener(
				delegate
				{
					ContentElementOnReset(newContentElementScript);
				});
			newContentElementScript.DeleteButton.onClick.AddListener(
				delegate
				{
					ContentElementOnDelete(newContentElementScript);
				});
		}
	}

	private void ContentElementOnDelete(QuestionDataBaseScrollViewElement contentElement)
	{
		YesOrNo.SetFromChooseBaseDeleteButton(contentElement.QuestionDataBase);
		UIManager.Instance.GoToView(YesOrNo);
	}

	private void ContentElementOnReset(QuestionDataBaseScrollViewElement contentElement)
	{
		YesOrNo.SetFromChooseBaseResetButton(contentElement.QuestionDataBase);
		UIManager.Instance.GoToView(YesOrNo);
	}

	private void ContentElementOnEdit(QuestionDataBase questionDataBase)
	{
		EditBase.SetEditBase(questionDataBase);
		UIManager.Instance.GoToView(EditBase);
	}

	private void ContentElementOnToggled(bool value,QuestionDataBaseScrollViewElement questionBaseContentElement)
	{
		if (value)
		{
			SelectedBase = questionBaseContentElement;
		}
	}

	private void AddBaseButton_OnClicked()
	{
		UIManager.Instance.GoToView(AddBase);
	}

	private void ChooseButton_OnClicked()
	{
		if (SelectedBase.BaseProgressSlider.value == 1)
		{
			YesOrNo.SetFromChooseBaseBaseIs100Percent(SelectedBase.QuestionDataBase,GoToGame);
			UIManager.Instance.GoToView(YesOrNo);
		}
		else if(SelectedBase.QuestionDataBase.Questions.Count <= 0)
		{
			UnityAction editBaseDelegate = delegate { ContentElementOnEdit(SelectedBase.QuestionDataBase); };
			YesOrNo.SetFromChooseBaseBaseIsEmpty(SelectedBase.QuestionDataBase,editBaseDelegate);
			UIManager.Instance.GoToView(YesOrNo);
		}
		else
		{
			GoToGame();
		}
	}

	private void GoToGame()
	{
		Game.SetQuestionBase(SelectedBase.QuestionDataBase);
		UIManager.Instance.GoToView(Game);
	}
}
