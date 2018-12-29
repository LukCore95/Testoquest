using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public static class PlayerPrefsManager
{
	public static void SaveQuestionDataBasePath(string dataBasePath,string dataBaseName)
	{
		PlayerPrefs.SetString(dataBaseName + "_Path",dataBasePath);
		PlayerPrefs.Save();
	}

	public static string GetQuestionDataBasePath(string dataBaseName)
	{
		return PlayerPrefs.GetString(dataBaseName + "_Path");
	}

	public static void DeleteQuestionDataBasePath(string dataBaseName)
	{
		PlayerPrefs.DeleteKey(dataBaseName + "_Path");
		PlayerPrefs.Save();
	}

	public static void SaveQuestionDataBaseNameToPlayerPrefs(int index,string dataBaseName)
	{
		PlayerPrefs.SetString("QuestionDataBase" + index,dataBaseName);
		PlayerPrefs.Save();
	}

	public static string GetQuestionDataBaseNameFromPlayerPrefs(int index)
	{
		return PlayerPrefs.GetString("QuestionDataBase" + index);
	}

	public static bool CheckQuestionDataBaseNameFromPlayerPrefs(int index)
	{
		return PlayerPrefs.HasKey("QuestionDataBase" + index);
	}

	public static void DeleteQuestionDataBaseName(int index)
	{
		PlayerPrefs.DeleteKey("QuestionDataBase" + index);
		PlayerPrefs.Save();
	}

	public static TimeSpan GetQuestionDataBaseTimeSpent(string dataBaseName)
	{
		string str = PlayerPrefs.GetString(dataBaseName + "_TimeSpent", "00:00:00");
		return TimeSpan.Parse(PlayerPrefs.GetString(dataBaseName+"_TimeSpent", "00:00:00"),CultureInfo.CurrentCulture);
	}

	public static void SaveQuestionDataBaseTimeSpent(string dataBaseName, string timeSpent)
	{
		PlayerPrefs.SetString(dataBaseName+"_TimeSpent",timeSpent);
		PlayerPrefs.Save();
	}
	
	public static void DeleteQuestionDataBaseTimeSpent(string dataBaseName)
	{
		PlayerPrefs.DeleteKey(dataBaseName+"_TimeSpent");
		PlayerPrefs.Save();
	}

	public static void SaveQuestionDataBaseState(QuestionDataBase dataBaseToSave)
	{
		foreach (Question question in dataBaseToSave.Questions)
		{
			string key = dataBaseToSave.Name + "_" + question.QuestionName;
			int value = Convert.ToInt32(question.IsAnswered);
			PlayerPrefs.SetInt(key,value);
		}
		PlayerPrefs.Save();
	}

	public static void SaveQuestionState(QuestionDataBase dataBase, Question question)
	{
		string key = dataBase.Name + "_" + question.QuestionName;
		int value = Convert.ToInt32(question.IsAnswered);
		PlayerPrefs.SetInt(key,value);
		PlayerPrefs.Save();
	}

	public static bool GetQuestionState(string dataBaseName, Question question)
	{
		string key = dataBaseName + "_" + question.QuestionName;
		return Convert.ToBoolean(PlayerPrefs.GetInt(key,0));
	}

	public static void DeleteQuestionState(QuestionDataBase dataBase, Question question)
	{
		string key = dataBase.Name + "_" + question.QuestionName;
		PlayerPrefs.DeleteKey(key);
		PlayerPrefs.Save();
	}

	public static void SaveOptions()
	{
		PlayerPrefs.SetFloat("MusicVolume",OptionsManager.Instance.MusicVolume);
		PlayerPrefs.SetFloat("SoundVolume",OptionsManager.Instance.SoundVolume);
		PlayerPrefs.SetInt("Vibrations", Convert.ToInt32(OptionsManager.Instance.Vibrations));
		PlayerPrefs.SetInt("Repeats", OptionsManager.Instance.RepeatsPerQuestionsAtMistakeNumber);
		PlayerPrefs.SetInt("StartingRepeats", OptionsManager.Instance.StartingRepeatsPerQuestionsNumber);
		PlayerPrefs.SetString("TimeForAnswer",OptionsManager.Instance.TimeForAnswer.ToString(@"mm\:ss"));
		PlayerPrefs.SetInt("LearnWithoutGame", Convert.ToInt32(OptionsManager.Instance.LearnWithoutGame));

		PlayerPrefs.Save();
	}

	public static void SetOptionsFromPlayerPrefs()
	{
		OptionsManager.Instance.MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
		OptionsManager.Instance.SoundVolume = PlayerPrefs.GetFloat("SoundVolume");
		OptionsManager.Instance.Vibrations = Convert.ToBoolean(PlayerPrefs.GetInt("Vibrations"));
		OptionsManager.Instance.RepeatsPerQuestionsAtMistakeNumber = PlayerPrefs.GetInt("Repeats");
		OptionsManager.Instance.StartingRepeatsPerQuestionsNumber = PlayerPrefs.GetInt("StartingRepeats");
		OptionsManager.Instance.TimeForAnswer = TimeSpan.ParseExact(PlayerPrefs.GetString("TimeForAnswer"),@"mm\:ss", CultureInfo.CurrentCulture, TimeSpanStyles.None);
		OptionsManager.Instance.LearnWithoutGame = Convert.ToBoolean(PlayerPrefs.GetInt("LearnWithoutGame"));
	}
}
