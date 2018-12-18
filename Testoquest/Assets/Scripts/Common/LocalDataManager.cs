using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public static class LocalDataManager
{
	public static Question[] GetQuestionsFromFolder(string folderPath)
	{
		List<Question> questions = new List<Question>();

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

		return questions.ToArray();
	}
}
