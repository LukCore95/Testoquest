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

	[SerializeField] private AnimationManager _animationManager;
	[SerializeField] private QuestionDataBase _selectedQuestionDataBase;
	[SerializeField] private List<QuestionRepeatsStruct> _questionRepeats = new List<QuestionRepeatsStruct>();
	[SerializeField] private Question _currentQuestion;
	[SerializeField] private List<AnswerScrollViewElement> _currentAnswerElements = new List<AnswerScrollViewElement>();
	[SerializeField] private TimeSpan _timeSpent;
	[SerializeField] private bool _isTime;
	[SerializeField] private double _timer;
	[SerializeField] private double _timerForAnswer;
	[SerializeField] private float _answeredQuestionsNumber;
	[SerializeField] private float _goodAnswersNumber;
	[SerializeField] private float _allQuestionRepeats;


	private void Start()
	{
		ExitButton.onClick.AddListener(SaveAndGoBack);
	}

	private void Update()
	{
		if (_isTime)
		{
			_timer += Time.deltaTime;
			_timeSpent = TimeSpan.FromSeconds(_timer);
			TimeSpentText.text = _timeSpent.ToString("hh\\:mm\\:ss");
		}
	}

	public override void OnEnable()
	{
		UIManager.Instance.Background.DisableAll();
		UIManager.Instance.Background.BackButton.gameObject.SetActive(true);
		UIManager.Instance.Background.SetBackButtonListener(SaveAndGoBack);
		_isTime = true;
		WinBorder.SetActive(false);
		QuestionPhase();
	}

	public override void OnDisable()
	{
		_isTime = false;
		_questionRepeats.Clear();
		_timeSpent = new TimeSpan(0,0,0);
		_timer = 0;
		_selectedQuestionDataBase = null;
	}

	public void SetQuestionBase(QuestionDataBase selectedQuestionDataBase)
	{
		_answeredQuestionsNumber = 0;
		_questionRepeats.Clear();
		_selectedQuestionDataBase = selectedQuestionDataBase;
		_allQuestionRepeats = OptionsManager.Instance.StartingRepeatsPerQuestionsNumber * this._selectedQuestionDataBase.Questions.Count;
		_goodAnswersNumber = _selectedQuestionDataBase.Questions.FindAll(question => question.IsAnswered).ToList().Count;
		foreach (Question question in this._selectedQuestionDataBase.Questions)
		{
			if (!question.IsAnswered)
			{
				_questionRepeats.Add(new QuestionRepeatsStruct(question,OptionsManager.Instance.StartingRepeatsPerQuestionsNumber));
			}
			else
			{
				_answeredQuestionsNumber++;
			}
		}
		_timeSpent = this._selectedQuestionDataBase.TimeSpent;
		_timer = _timeSpent.TotalSeconds;
		TimeSpentText.text = _timeSpent.ToString();
		UpdateProgress();
	}

	private void QuestionPhase()
	{
		_animationManager.PlayIdleAnimation();
		if (_questionRepeats.Count > 0)
		{
			_currentAnswerElements.Clear();
			Random randomQuestionIndex = new Random();
			Random randomAnswersIndex = new Random();
			_currentQuestion = _questionRepeats[randomQuestionIndex.Next(_questionRepeats.Count)].Question;
			QuestionText.text = _currentQuestion.QuestionText;
			_currentQuestion.Answers = _currentQuestion.Answers.OrderBy(answer => randomAnswersIndex.Next()).ToList();
			foreach (Transform child in Content)
			{
				Destroy(child.gameObject);
			}
			foreach (Answer answer in _currentQuestion.Answers)
			{
				GameObject answerGameObject = Instantiate(AnswerContentElementPrefab, Content);
				AnswerScrollViewElement answerScript = answerGameObject.GetComponent<AnswerScrollViewElement>();
				answerScript.SetAnswer(answer);
				_currentAnswerElements.Add(answerScript);
			}

			StartCoroutine(DecreaseQuestionTimer());

			SelectButton.onClick.RemoveAllListeners();
			SelectButton.onClick.AddListener(CheckAnswersPhase);
			SelectButtonText.text = "Wybierz Odpowiedzi";
		}
	}

	private IEnumerator DecreaseQuestionTimer()
	{
		_timerForAnswer = OptionsManager.Instance.TimeForAnswer.TotalSeconds;
		while (_timerForAnswer > 0)
		{
			_timerForAnswer -= Time.deltaTime;
			TimeForAnswerText.text = TimeSpan.FromSeconds(_timerForAnswer).ToString("mm\\:ss");
			yield return new WaitForFixedUpdate();
		}
		CheckAnswersPhase();
	}

	private void CheckAnswersPhase()
	{
		foreach (AnswerScrollViewElement answer in _currentAnswerElements)
		{
			answer.AnswerToggle.interactable = false;
		}
		StopAllCoroutines();
		int numberOfAnswers = _currentQuestion.Answers.Count;
		int numberOfMatchedAnswers = 0;
		int repeatsStructIndex =
			_questionRepeats.FindIndex(_struct => _struct.Question == _currentQuestion);
		QuestionRepeatsStruct repeatsStruct = _questionRepeats[repeatsStructIndex];
		foreach (Answer answer in _currentQuestion.Answers)
		{
			AnswerScrollViewElement answerElement =
				_currentAnswerElements.Find(_answerElement => _answerElement.Answer == answer);
			if (answerElement.IsSelected == answer.IsCorrect)
			{
				numberOfMatchedAnswers++;
				if(answer.IsCorrect)
				{
					answerElement.BackgroundImage.color = Color.green;
				}
				else
				{
					answerElement.BackgroundImage.color = Color.white;
				}
			}
			else
			{
				answerElement.BackgroundImage.color = Color.red;
			}
		}

		if (numberOfAnswers == numberOfMatchedAnswers)
		{
			AudioManager.Instance.PlayCorrectSFX();
			_animationManager.PlayGoodAnswerAnimation();
			_goodAnswersNumber++;
			repeatsStruct.RepeatsLeft--;
		}
		else
		{
			if (OptionsManager.Instance.Vibrations)
			{
				Handheld.Vibrate();
			}
			AudioManager.Instance.PlayWrongSFX();
			_animationManager.PlayBadAnswerAnimation();
			repeatsStruct.RepeatsLeft = repeatsStruct.RepeatsLeft + OptionsManager.Instance.RepeatsPerQuestionsAtMistakeNumber;
			_allQuestionRepeats += OptionsManager.Instance.RepeatsPerQuestionsAtMistakeNumber;
		}

		_questionRepeats[repeatsStructIndex] = repeatsStruct;

		if (_questionRepeats[repeatsStructIndex].RepeatsLeft == 0)
		{
			_questionRepeats.RemoveAt(repeatsStructIndex);
			_answeredQuestionsNumber++;
			_currentQuestion.IsAnswered = true;
			QuestionDataBaseManager.Instance.UpdateQuestionToAnswered(_selectedQuestionDataBase, _currentQuestion);
		}

		UpdateProgress();
		CheckIfWon();

		SelectButton.onClick.RemoveAllListeners();
		SelectButton.onClick.AddListener(QuestionPhase);
		SelectButtonText.text = "Kolejne Pytanie";
	}

	private void CheckIfWon()
	{
		if (_questionRepeats.Count == 0)
		{
			_isTime = false;
			_animationManager.PlayWinAnimation();
			WinBorder.SetActive(true);
		}
	}

	private void UpdateProgress()
	{
		_selectedQuestionDataBase.TimeSpent = TimeSpan.Parse(TimeSpentText.text,CultureInfo.CurrentCulture);
		//selectedQuestionDataBase.TimeSpent = TimeSpan.ParseExact(TimeSpentText.text,@"hh\:mm\:ss", CultureInfo.CurrentCulture, TimeSpanStyles.None);
		PlayerPrefsManager.SaveQuestionDataBaseTimeSpent(_selectedQuestionDataBase.Name,TimeSpentText.text);


		EnemyHPSlider.value = 1 - _goodAnswersNumber / _allQuestionRepeats;
		EnemyHPText.text = _allQuestionRepeats - _goodAnswersNumber + "/" + _allQuestionRepeats;
		QuestionBaseProgressSlider.value = _selectedQuestionDataBase.GetPercentageOfAnsweredQuestions();
		QuestionBaseProgressText.text = _answeredQuestionsNumber + "/" + _selectedQuestionDataBase.Questions.Count;
	}

	private void SaveAndGoBack()
	{
		PlayerPrefsManager.SaveQuestionDataBaseTimeSpent(_selectedQuestionDataBase.Name,TimeSpentText.text);
		UIManager.Instance.GoBack();
	}
}
