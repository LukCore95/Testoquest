using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScrollViewElement : MonoBehaviour
{
	public Question Question;
	public Text QuestionName;
	public Button EditButton;
	public Button DeleteButton;

	public void SetQuestion(Question question)
	{
		Question = question;
		QuestionName.text = Question.QuestionName;
	}
}

