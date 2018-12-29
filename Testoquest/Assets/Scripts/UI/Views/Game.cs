using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

[Serializable]
struct QuestionRepeatsStruct
{
	public Question Question;
	public int RepeatsLeft;

	public QuestionRepeatsStruct(Question question, int repeatsLeft)
	{
		Question = question;
		RepeatsLeft = repeatsLeft;
	}
}

public class Game : UIView
{
	public Slider EnemyHPSlider;
	public Text EnemyHPText;
	public Slider QuestionBaseProgressSlider;
	public Text TimeSpentText;
	public Text QuestionBaseProgressText;
	public Button SelectButton;
	public Text SelectButtonText;
	public Transform Content;
	public Text QuestionText;
	public Text TimeForAnswerText;
	public GameObject WinBorder;
	public Button ExitButton;
	public GameObject AnswerContentElementPrefab;

	[SerializeField] private QuestionDataBase selectedQuestionDataBase;
	[SerializeField] private List<QuestionRepeatsStruct> questionRepeats = new List<QuestionRepeatsStruct>();
	[SerializeField] private Question currentQuestion;
	[SerializeField] private List<AnswerScrollViewElement> currentAnswerElements = new List<AnswerScrollViewElement>();
	[SerializeField] private TimeSpan timeSpent;
	[SerializeField] private double timer;
	[SerializeField] private double timerForAnswer;
	[SerializeField] private float answeredQuestionsNumber;
	[SerializeField] private float goodAnswersNumber;
	[SerializeField] private float allQuestionRepeats;

	private void Start()
	{
		ExitButton.onClick.AddListener(SaveAndGoBack);
	}

	private void Update()
	{
		timer += Time.deltaTime;
		timeSpent = TimeSpan.FromSeconds(timer);
		TimeSpentText.text = timeSpent.ToString();
	}

	public override void OnEnable()
	{
		UIManager.Instance.Background.DisableAll();
		UIManager.Instance.Background.BackButton.gameObject.SetActive(true);
		UIManager.Instance.Background.SetBackButtonListener(SaveAndGoBack);
		WinBorder.SetActive(false);
		QuestionPhase();
	}

	public override void OnDisable()
	{
		Time.timeScale = 1;
		questionRepeats.Clear();
		selectedQuestionDataBase = null;
	}

	public void SetQuestionBase(QuestionDataBase _selectedQuestionDataBase)
	{
		answeredQuestionsNumber = 0;
		questionRepeats.Clear();
		selectedQuestionDataBase = _selectedQuestionDataBase;
		allQuestionRepeats = OptionsManager.Instance.StartingRepeatsPerQuestionsNumber * selectedQuestionDataBase.Questions.Length;
		foreach (Question question in selectedQuestionDataBase.Questions)
		{
			if (!question.IsAnswered)
			{
				questionRepeats.Add(new QuestionRepeatsStruct(question,OptionsManager.Instance.StartingRepeatsPerQuestionsNumber));
			}
			else
			{
				answeredQuestionsNumber++;
			}
		}
		timeSpent = selectedQuestionDataBase.TimeSpent;
		timer = timeSpent.TotalSeconds;
		TimeSpentText.text = timeSpent.ToString();
		UpdateProgress();
	}

	private void QuestionPhase()
	{
		if (questionRepeats.Count > 0)
		{
			currentAnswerElements.Clear();
			Random randomIndex = new Random();
			currentQuestion = questionRepeats[randomIndex.Next(questionRepeats.Count)].Question;
			QuestionText.text = currentQuestion.QuestionText;
			foreach (Transform child in Content)
			{
				Destroy(child.gameObject);
			}
			foreach (Answer answer in currentQuestion.Answers)
			{
				GameObject answerGameObject = Instantiate(AnswerContentElementPrefab, Content);
				AnswerScrollViewElement answerScript = answerGameObject.GetComponent<AnswerScrollViewElement>();
				answerScript.SetAnswer(answer);
				currentAnswerElements.Add(answerScript);
			}

			StartCoroutine(DecreaseQuestionTimer());

			SelectButton.onClick.RemoveAllListeners();
			SelectButton.onClick.AddListener(CheckAnswersPhase);
			SelectButtonText.text = "Wybierz Odpowiedzi";
		}
	}

	private IEnumerator DecreaseQuestionTimer()
	{
		timerForAnswer = OptionsManager.Instance.TimeForAnswer.TotalSeconds;
		while (timerForAnswer > 0)
		{
			timerForAnswer -= Time.deltaTime;
			TimeForAnswerText.text = TimeSpan.FromSeconds(timerForAnswer).ToString();
			yield return new WaitForFixedUpdate();
		}
		CheckAnswersPhase();
	}

	private void CheckAnswersPhase()
	{
		StopAllCoroutines();
		int numberOfAnswers = currentQuestion.Answers.Count;
		int numberOfMatchedAnswers = 0;
		int repeatsStructIndex =
			questionRepeats.FindIndex(_struct => _struct.Question == currentQuestion);
		QuestionRepeatsStruct repeatsStruct = questionRepeats[repeatsStructIndex];
		foreach (Answer answer in currentQuestion.Answers)
		{
			AnswerScrollViewElement answerElement =
				currentAnswerElements.Find(_answerElement => _answerElement.Answer == answer);
			if (answerElement.IsSelected == answer.IsCorrect)
			{
				numberOfMatchedAnswers++;
			}

			if(answer.IsCorrect)
			{
				answerElement.BackgroundImage.color = Color.green;
			}
			else
			{
				answerElement.BackgroundImage.color = Color.red;
			}
		}



		if (numberOfAnswers == numberOfMatchedAnswers)
		{
			goodAnswersNumber++;
			repeatsStruct.RepeatsLeft--;
		}
		else
		{
			repeatsStruct.RepeatsLeft = repeatsStruct.RepeatsLeft + OptionsManager.Instance.RepeatsPerQuestionsAtMistakeNumber;
			allQuestionRepeats += OptionsManager.Instance.RepeatsPerQuestionsAtMistakeNumber;
		}

		questionRepeats[repeatsStructIndex] = repeatsStruct;

		if (questionRepeats[repeatsStructIndex].RepeatsLeft == 0)
		{
			questionRepeats.RemoveAt(repeatsStructIndex);
			answeredQuestionsNumber++;
			currentQuestion.IsAnswered = true;
			QuestionDataBaseManager.Instance.UpdateQuestionToAnswered(selectedQuestionDataBase, currentQuestion);
		}

		UpdateProgress();
		CheckIfWon();

		SelectButton.onClick.RemoveAllListeners();
		SelectButton.onClick.AddListener(QuestionPhase);
		SelectButtonText.text = "Kolejne Pytanie";
	}

	private void CheckIfWon()
	{
		if (questionRepeats.Count == 0)
		{
			Time.timeScale = 0;
			WinBorder.SetActive(true);
		}
	}

	private void UpdateProgress()
	{
		selectedQuestionDataBase.TimeSpent = TimeSpan.Parse(TimeSpentText.text,CultureInfo.CurrentCulture);
		//selectedQuestionDataBase.TimeSpent = TimeSpan.ParseExact(TimeSpentText.text,@"hh\:mm\:ss", CultureInfo.CurrentCulture, TimeSpanStyles.None);
		PlayerPrefsManager.SaveQuestionDataBaseTimeSpent(selectedQuestionDataBase.Name,TimeSpentText.text);


		EnemyHPSlider.value = 1 - goodAnswersNumber / allQuestionRepeats;
		EnemyHPText.text = allQuestionRepeats - goodAnswersNumber + "/" + allQuestionRepeats;
		QuestionBaseProgressSlider.value = selectedQuestionDataBase.GetPercentageOfAnsweredQuestions();
		QuestionBaseProgressText.text = answeredQuestionsNumber + "/" + selectedQuestionDataBase.Questions.Length;
	}

	private void SaveAndGoBack()
	{
		UIManager.Instance.GoBack();
	}
}
