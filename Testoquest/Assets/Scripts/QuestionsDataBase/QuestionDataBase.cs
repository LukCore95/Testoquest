using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class QuestionDataBase
{
	public string Name;
	public string Path;
	public TimeSpan TimeSpent;
	public List<Question> Questions;

	public float GetPercentageOfAnsweredQuestions()
	{
		float answered = 0;
		foreach (Question question in Questions)
		{
			if (question.IsAnswered)
			{
				answered++;
			}
		}

		return answered / (float)Questions.Count;
	}
}
