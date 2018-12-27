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
		foreach (string questionDataBaseName in QuestionDataBaseNames)
		{
			QuestionDataBase newBase = new QuestionDataBase();
			newBase.Name = questionDataBaseName;
			newBase.Path = PlayerPrefsManager.GetQuestionDataBasePath(newBase.Name);
			newBase.TimeSpent = PlayerPrefsManager.GetQuestionDataBaseTimeSpent(newBase.Name);
			newBase.Questions = LocalDataManager.GetQuestionsFromFolder(newBase.Path);
			foreach (Question question in newBase.Questions)
			{
				question.IsAnswered = PlayerPrefsManager.GetQuestionState(questionDataBaseName,question);
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
		PlayerPrefsManager.SaveQuestionDataBasePath(questionDataBasePath,dataBaseName);
		UpdateQuestionDataBaseNames();
		UpdateQuestionDataBases();
	}

	public void DeleteDataBase(string dataBaseName)
	{
		int index = QuestionDataBaseNames.FindIndex(name => name == dataBaseName);
		PlayerPrefsManager.DeleteQuestionDataBaseName(index);
		PlayerPrefsManager.DeleteQuestionDataBasePath(dataBaseName);
		QuestionDataBaseNames.Remove(dataBaseName);
		RefreshDataBases();
	}

	private void RefreshDataBases()
	{
		int index = 0;
		while (PlayerPrefsManager.CheckQuestionDataBaseNameFromPlayerPrefs(index))
		{
			PlayerPrefsManager.DeleteQuestionDataBaseName(index);
			index++;
		}

		for (var i= 0; i < QuestionDataBaseNames.Count; i++)
		{
			PlayerPrefsManager.SaveQuestionDataBaseNameToPlayerPrefs(i, QuestionDataBaseNames[i]);
		}

		UpdateQuestionDataBases();
	}

	public void UpdateQuestionToAnswered(QuestionDataBase DataBase,Question answeredQuestion)
	{
		Question _answeredQuestion =
			DataBase.Questions.First(question => question.QuestionName == answeredQuestion.QuestionName);
		PlayerPrefsManager.SaveQuestionState(DataBase,_answeredQuestion);
	}
}
