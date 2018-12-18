using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
	public Background Background;

	[SerializeField] private List<UIView> _views = new List<UIView>();
	[SerializeField] private List<UIView> _viewsHistory = new List<UIView>();

	public void GoToView(UIView viewToSet)
	{
		foreach (var uiView in _views)
		{
			if (uiView != viewToSet)
			{
				uiView.gameObject.SetActive(false);
			}
			else
			{
				uiView.gameObject.SetActive(true);
			}
		}
		viewToSet.gameObject.SetActive(true);
		AddViewToHistory(viewToSet);
	}

	public void GoBack()
	{
		if (_viewsHistory.Count > 0)
		{
			_viewsHistory.RemoveAt(_viewsHistory.Count-1);
		}

		GoToView(_viewsHistory[_viewsHistory.Count - 1]);
	}

	private void ResetHistory()
	{

	}

	private void AddViewToHistory(UIView viewToAdd)
	{
		if (_viewsHistory.Count > 0)
		{
			if (_viewsHistory[_viewsHistory.Count - 1] != viewToAdd)
			{
				_viewsHistory.Add(viewToAdd);
			}
		}
		else
		{
			_viewsHistory.Add(viewToAdd);
		}
	}

}
