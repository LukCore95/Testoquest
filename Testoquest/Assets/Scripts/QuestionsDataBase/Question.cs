using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Question
{
	public string QuestionName;
	public string QuestionText;
	public bool IsAnswered;
	public List<Answer> Answers = new List<Answer>();
}
