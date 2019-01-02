using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public static class LocalDataManager
{
	public static List<Question> GetQuestionsFromFolder(string folderPath)
	{
		List<Question> questions = new List<Question>();
		Directory.CreateDirectory(folderPath);
		string[] filePaths = Directory.GetFiles(folderPath,"pyt*.txt");
		
		foreach (string filePath in filePaths)
		{
			string[] fileLines = File.ReadAllLines(filePath);
			string fileNameWithExtension = Path.GetFileName(filePath);
			char[] answerCorrectnessArray = fileLines[0].ToCharArray();
			Question newQuestion = new Question();
			newQuestion.QuestionName = fileNameWithExtension.Split('.')[0];
			newQuestion.QuestionText = fileLines[1];
			for (int i = 2, charIndex = 1; i < fileLines.Length; i++,charIndex++)
			{
				if (fileLines[i].Length > 0)
				{
					newQuestion.Answers.Add(new Answer(fileLines[i],false));
					if (answerCorrectnessArray[charIndex] == '1')
					{
						newQuestion.Answers.Last().IsCorrect = true;
					}
					else
					{
						newQuestion.Answers.Last().IsCorrect = false;
					}
				}
			}
			questions.Add(newQuestion);
		}

		return questions.ToList();
	}

	public static void DeleteQuestion(QuestionDataBase dataBase, Question question)
	{
		File.Delete(dataBase.Path + "/" + question.QuestionName + ".txt");
	}

	public static void SaveQuestion(QuestionDataBase dataBaseToEdit, Question question)
	{
		string answerCorrectness = "X";

		foreach (Answer answer in question.Answers)
		{
			if (answer.IsCorrect)
			{
				answerCorrectness += 1.ToString();
			}
			else
			{
				answerCorrectness += 0.ToString();
			}
		}

		List<string> questionContentLines = new List<string>();
		questionContentLines.Add(answerCorrectness);
		questionContentLines.Add(question.QuestionText);
		foreach (Answer answer in question.Answers)
		{
			questionContentLines.Add(answer.AnswerText);
		}
		File.WriteAllLines(@dataBaseToEdit.Path + "/" + question.QuestionName + ".txt",questionContentLines);
	}
}
