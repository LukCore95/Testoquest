using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.UI;

public class QuestionDataBaseManager : Singleton<QuestionDataBaseManager>
{
	public List<QuestionDataBase> QuestionDataBases;
	public List<string> QuestionDataBaseNames;

	private void Start()
	{
		UpdateQuestionDataBaseNames();
		UpdateQuestionDataBases();
	}

	private void UpdateQuestionDataBaseNames()
	{
		int index = 0;
		QuestionDataBaseNames.Clear();
		while (PlayerPrefsManager.CheckQuestionDataBaseNameFromPlayerPrefs(index))
		{
			QuestionDataBaseNames.Add(PlayerPrefsManager.GetQuestionDataBaseNameFromPlayerPrefs(index));
			index++;
		}	
	}

	private void UpdateQuestionDataBases()
	{
		QuestionDataBases.Clear();
		for (var index = 0; index < QuestionDataBaseNames.Count; index++)
		{
			string questionDataBaseName = QuestionDataBaseNames[index];
			QuestionDataBase newBase = new QuestionDataBase();
			newBase.Name = questionDataBaseName;
			newBase.Path = PlayerPrefsManager.GetQuestionDataBasePath(index);
			newBase.TimeSpent = PlayerPrefsManager.GetQuestionDataBaseTimeSpent(index);
			newBase.Questions = LocalDataManager.GetQuestionsFromFolder(newBase.Path);
			foreach (Question question in newBase.Questions)
			{
				question.IsAnswered = PlayerPrefsManager.GetQuestionState(index, question);
			}

			QuestionDataBases.Add(newBase);
		}
	}

	public void AddNewQuestionDataBase(string questionDataBasePath, string dataBaseName)
	{
		int index = 0;
		while (PlayerPrefsManager.CheckQuestionDataBaseNameFromPlayerPrefs(index))
		{
			index++;
		}
		PlayerPrefsManager.SaveQuestionDataBaseNameToPlayerPrefs(index,dataBaseName);
		PlayerPrefsManager.SaveQuestionDataBasePath(questionDataBasePath,index);
		UpdateQuestionDataBaseNames();
		UpdateQuestionDataBases();
	}

	public void DeleteDataBase(string dataBaseName)
	{
		int index = QuestionDataBaseNames.FindIndex(name => name == dataBaseName);
		int iterations = QuestionDataBaseNames.Count;
		QuestionDataBase database = QuestionDataBases.Find(_database => _database.Name == dataBaseName);
		foreach (Question question in database.Questions)
		{
			PlayerPrefsManager.DeleteQuestionState(index, question);
		}
		PlayerPrefsManager.DeleteQuestionDataBaseName(index);
		PlayerPrefsManager.DeleteQuestionDataBasePath(index);
		PlayerPrefsManager.DeleteQuestionDataBaseTimeSpent(index);
		QuestionDataBaseNames.Remove(dataBaseName);
		RefreshDataBases(iterations);
	}

	public void ResetDataBaseState(string dataBaseName)
	{
		QuestionDataBase dataBase = QuestionDataBases.Find(database => database.Name == dataBaseName);
		int index = QuestionDataBaseNames.FindIndex(name => name == dataBaseName);
		dataBase.TimeSpent = new TimeSpan();
		foreach (Question question in dataBase.Questions)
		{
			question.IsAnswered = false;
			PlayerPrefsManager.SaveQuestionState(index, question);
		}
		PlayerPrefsManager.SaveQuestionDataBaseState(dataBase, index);
		PlayerPrefsManager.SaveQuestionDataBaseTimeSpent(index, dataBase.TimeSpent.ToString());
	}

	private void RefreshDataBases(int iterations)
	{
		int index = 0;
		for (var i = 0; i < iterations; i++)
		{
			if (PlayerPrefsManager.CheckQuestionDataBaseNameFromPlayerPrefs(i))
			{
				index++;
				PlayerPrefsManager.DeleteQuestionDataBaseName(i);
				PlayerPrefsManager.DeleteQuestionDataBasePath(i);
			}
		}

		for (var i= 0; i < QuestionDataBaseNames.Count; i++)
		{
			string dataBaseName = QuestionDataBaseNames[i];
			QuestionDataBase database = QuestionDataBases.Find(_database => _database.Name == dataBaseName);
			PlayerPrefsManager.SaveQuestionDataBaseNameToPlayerPrefs(i, dataBaseName);
			PlayerPrefsManager.SaveQuestionDataBasePath(database.Path, i);
		}

		UpdateQuestionDataBases();
	}

	public void UpdateQuestionToAnswered(QuestionDataBase DataBase,Question updatedAnsweredQuestion)
	{
		Question _answeredQuestion =
			DataBase.Questions.First(question => question.QuestionName == updatedAnsweredQuestion.QuestionName);
		int index = QuestionDataBaseNames.FindIndex(name => name == DataBase.Name);
		_answeredQuestion = updatedAnsweredQuestion;
		PlayerPrefsManager.SaveQuestionState(index, _answeredQuestion);
	}

	public void DeleteQuestionFromDatabase(string dataBaseName, string questionQuestionName)
	{
		QuestionDataBase dataBase = QuestionDataBases.Find(database => database.Name == dataBaseName);
		Question question = dataBase.Questions.First(q => q.QuestionName == questionQuestionName);
		int index = QuestionDataBaseNames.FindIndex(name => name == dataBase.Name);

		if (question != null)
		{
			LocalDataManager.DeleteQuestion(dataBase, question);
			PlayerPrefsManager.DeleteQuestionState(index, question);
			dataBase.Questions.Remove(question);
		}
	}

	public void AddNewQuestion(QuestionDataBase dataBase, Question newQuestion)
	{
		QuestionDataBase dataBaseToEdit = QuestionDataBases.Find(dbase => dbase.Name == dataBase.Name);
		int index = QuestionDataBaseNames.FindIndex(name => name == dataBaseToEdit.Name);

		if (dataBaseToEdit.Questions.Exists(q => q.QuestionName == newQuestion.QuestionName))
		{
			int existingQuestionIndex = dataBaseToEdit.Questions.FindIndex(q => q.QuestionName == newQuestion.QuestionName);
			dataBaseToEdit.Questions[existingQuestionIndex] = newQuestion;
		}
		else
		{
			dataBaseToEdit.Questions.Add(newQuestion);
		}

		PlayerPrefsManager.SaveQuestionState(index, newQuestion);
		LocalDataManager.SaveQuestion(dataBaseToEdit, newQuestion);
	}
}
