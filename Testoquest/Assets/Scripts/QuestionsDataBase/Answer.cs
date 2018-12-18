using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Answer
{
	public string AnswerText;
	public bool IsCorrect;

	public Answer(string answerText, bool isCorrect)
	{
		AnswerText = answerText;
		IsCorrect = isCorrect;
	}
}
