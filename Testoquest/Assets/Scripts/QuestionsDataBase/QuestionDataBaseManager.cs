﻿using System;
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
		PlayerPrefsManager.DeleteQuestionDataBaseTimeSpent(dataBaseName);
		QuestionDataBaseNames.Remove(dataBaseName);
		RefreshDataBases();
	}

	public void ResetDataBaseState(string dataBaseName)
	{
		QuestionDataBase dataBase = QuestionDataBases.Find(database => database.Name == dataBaseName);
		dataBase.TimeSpent = new TimeSpan();
		foreach (Question question in dataBase.Questions)
		{
			question.IsAnswered = false;
			PlayerPrefsManager.SaveQuestionState(dataBase,question);
		}
		PlayerPrefsManager.SaveQuestionDataBaseState(dataBase);
		PlayerPrefsManager.SaveQuestionDataBaseTimeSpent(dataBaseName,dataBase.TimeSpent.ToString());
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

	public void UpdateQuestionToAnswered(QuestionDataBase DataBase,Question updatedAnsweredQuestion)
	{
		Question _answeredQuestion =
			DataBase.Questions.First(question => question.QuestionName == updatedAnsweredQuestion.QuestionName);
		_answeredQuestion = updatedAnsweredQuestion;
		PlayerPrefsManager.SaveQuestionState(DataBase,_answeredQuestion);
	}

	public void DeleteQuestionFromDatabase(string dataBaseName, string questionQuestionName)
	{
		QuestionDataBase dataBase = QuestionDataBases.Find(database => database.Name == dataBaseName);
		Question question = dataBase.Questions.First(q => q.QuestionName == questionQuestionName);

		if (question != null)
		{
			LocalDataManager.DeleteQuestion(dataBase, question);
			PlayerPrefsManager.DeleteQuestionState(dataBase,question);
			dataBase.Questions.Remove(question);
		}
	}

	public void AddNewQuestion(QuestionDataBase dataBase, Question newQuestion)
	{
		QuestionDataBase dataBaseToEdit = QuestionDataBases.Find(dbase => dbase.Name == dataBase.Name);

		if (dataBaseToEdit.Questions.Exists(q => q.QuestionName == newQuestion.QuestionName))
		{
			int existingQuestionIndex = dataBaseToEdit.Questions.FindIndex(q => q.QuestionName == newQuestion.QuestionName);
			dataBaseToEdit.Questions[existingQuestionIndex] = newQuestion;
		}
		else
		{
			dataBaseToEdit.Questions.Add(newQuestion);
		}

		PlayerPrefsManager.SaveQuestionState(dataBaseToEdit,newQuestion);
		LocalDataManager.SaveQuestion(dataBaseToEdit, newQuestion);
	}
}
