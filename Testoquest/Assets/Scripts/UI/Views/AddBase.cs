using System.Collections;
using System.Collections.Generic;
using GracesGames.SimpleFileBrowser.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class AddBase : UIView
{
	public GameObject FileBrowserPrefab;
	public FileBrowser FileBrowserScript;
	public Text QuestionDataBasePathText;
	public Button LookForPathButton;
	public InputField BaseNameInputField;
	public Button AddBaseButton;

	private Text pathText;
	private InputField FolderInputField;

	private void Start()
	{
		LookForPathButton.onClick.AddListener(LookForPathButton_OnClicked);
		AddBaseButton.onClick.AddListener(AddBaseButton_OnClicked);
	}

	private void AddBaseButton_OnClicked()
	{
		if (BaseNameInputField.text.Length > 0)
		{
			QuestionDataBaseManager.Instance.AddNewQuestionDataBase(QuestionDataBasePathText.text,BaseNameInputField.text);
			UIManager.Instance.GoBack();
		}
	}

	private void LookForPathButton_OnClicked()
	{
		OpenFileBrowser();
	}

	private void OpenFileBrowser() {
		// Create the file browser and name it
		GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, transform);
		fileBrowserObject.name = "FileBrowser";
		// Set the mode to save or load
		FileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
		FileBrowserScript.SetupFileBrowser(ViewMode.Portrait);
		pathText = FileBrowserScript.UIScript.PathText.GetComponent<Text>();
		FolderInputField = FileBrowserScript.UIScript.SaveFileTextInputFile;
		FileBrowserScript.UIScript.SelectFileButton.GetComponent<Button>().onClick.RemoveAllListeners();
		FileBrowserScript.UIScript.SelectFileButton.GetComponent<Button>().onClick.AddListener(OnPathSelected);
	}

	private void OnPathSelected()
	{
		if (FolderInputField.text == "")
		{
			QuestionDataBasePathText.text = pathText.text;
		}
		else
		{
			QuestionDataBasePathText.text = pathText.text + "/" + FolderInputField.text;
		}
		FileBrowserScript.CloseFileBrowser();
	}

	public override void OnEnable()
	{
		UIManager.Instance.Background.DisableAll();
		UIManager.Instance.Background.BackButton.gameObject.SetActive(true);
		UIManager.Instance.Background.SetBackButtonListener(UIManager.Instance.GoBack);

		BaseNameInputField.text = "";
	}

	public override void OnDisable()
	{
		
	}
}
