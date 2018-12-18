using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AnswerScrollViewElement : MonoBehaviour
{
	public Answer Answer;
	public Toggle AnswerToggle;
	public Image BackgroundImage;
	public Text AnswerText;

	public bool IsSelected;

	public void SetAnswer(Answer answer)
	{
		Answer = answer;
		AnswerToggle.isOn = false;
		BackgroundImage.color = Color.white;
		AnswerText.text = Answer.AnswerText;
		AnswerToggle.onValueChanged.AddListener(OnToggled);
	}

	private void OnToggled(bool arg0)
	{
		IsSelected = arg0;
	}
}

