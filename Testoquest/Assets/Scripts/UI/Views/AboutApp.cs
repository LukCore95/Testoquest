using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutApp : UIView
{
	[SerializeField] private Button CloseButton;

	private void Start()
	{
		CloseButton.onClick.AddListener(UIManager.Instance.GoBack);
	}

	public override void OnEnable()
	{
		UIManager.Instance.Background.DisableAll();
		UIManager.Instance.Background.SoundToggle.gameObject.SetActive(true);
		UIManager.Instance.Background.AboutUsButton.gameObject.SetActive(true);
	}

	public override void OnDisable()
	{
	}
}
