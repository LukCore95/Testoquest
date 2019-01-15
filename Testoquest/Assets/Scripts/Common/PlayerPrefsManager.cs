using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public static class PlayerPrefsManager
{
	public static void SaveQuestionDataBasePath(string dataBasePath, int index)
	{
		PlayerPrefs.SetString("QuestionDataBase" + index + "_Path",dataBasePath);
		PlayerPrefs.Save();
	}

	public static string GetQuestionDataBasePath(int index)
	{
		return PlayerPrefs.GetString("QuestionDataBase" + index + "_Path");
	}

	public static void DeleteQuestionDataBasePath(int index)
	{
		PlayerPrefs.DeleteKey("QuestionDataBase" + index + "_Path");
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

	public static TimeSpan GetQuestionDataBaseTimeSpent(int index)
	{
		string str = PlayerPrefs.GetString("QuestionDataBase" + index + "_TimeSpent", "00:00:00");
		return TimeSpan.Parse(PlayerPrefs.GetString("QuestionDataBase" + index + "_TimeSpent", "00:00:00"),CultureInfo.CurrentCulture);
	}

	public static void SaveQuestionDataBaseTimeSpent(int index, string timeSpent)
	{
		PlayerPrefs.SetString("QuestionDataBase" + index + "_TimeSpent",timeSpent);
		PlayerPrefs.Save();
	}
	
	public static void DeleteQuestionDataBaseTimeSpent(int index)
	{
		PlayerPrefs.DeleteKey("QuestionDataBase" + index + "_TimeSpent");
		PlayerPrefs.Save();
	}

	public static void SaveQuestionDataBaseState(QuestionDataBase dataBaseToSave, int index)
	{
		foreach (Question question in dataBaseToSave.Questions)
		{
			string key = "QuestionDataBase" + index + "_" + question.QuestionName;
			int value = Convert.ToInt32(question.IsAnswered);
			PlayerPrefs.SetInt(key,value);
		}
		PlayerPrefs.Save();
	}

	public static void SaveQuestionState(int index, Question question)
	{
		string key = "QuestionDataBase" + index + "_" + question.QuestionName;
		int value = Convert.ToInt32(question.IsAnswered);
		PlayerPrefs.SetInt(key,value);
		PlayerPrefs.Save();
	}

	public static bool GetQuestionState(int index, Question question)
	{
		string key = "QuestionDataBase" + index + "_" + question.QuestionName;
		return Convert.ToBoolean(PlayerPrefs.GetInt(key,0));
	}

	public static void DeleteQuestionState(int index, Question question)
	{
		string key = "QuestionDataBase" + index + "_" + question.QuestionName;
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

	public static void SetOptionsForFirstLaunch()
	{
		if (!PlayerPrefs.HasKey("MusicVolume"))
		{
			PlayerPrefs.SetFloat("MusicVolume", 1);
			PlayerPrefs.SetFloat("SoundVolume", 1);
			PlayerPrefs.SetInt("Vibrations", 1);
			PlayerPrefs.SetInt("Repeats", 3);
			PlayerPrefs.SetInt("StartingRepeats", 3);
			PlayerPrefs.SetString("TimeForAnswer", "01:00");
			PlayerPrefs.SetInt("LearnWithoutGame", 0);
			SetOptionsFromPlayerPrefs();
			PlayerPrefs.Save();
		}
	}
}
