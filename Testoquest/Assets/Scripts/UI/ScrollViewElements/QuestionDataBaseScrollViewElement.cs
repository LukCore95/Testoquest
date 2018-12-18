using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDataBaseScrollViewElement : MonoBehaviour
{
	public QuestionDataBase QuestionDataBase;
	public Toggle ChooseBaseToggle;
	public Slider BaseProgressSlider;
	public Text BaseName;
	public Button ResetButton;
	public Button EditButton;
	public Button DeleteButton;

	public void SetQuestionBase(QuestionDataBase questionDataBase)
	{
		QuestionDataBase = questionDataBase;
		BaseProgressSlider.value = QuestionDataBase.GetPercentageOfAnsweredQuestions();
		BaseName.text = QuestionDataBase.Name;
	}
}

