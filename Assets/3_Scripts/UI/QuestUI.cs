using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
	[SerializeField] private List<TMPro.TextMeshProUGUI> _questTexts;
	[SerializeField] private List<TMPro.TextMeshProUGUI> _questDiffculty;
	[SerializeField] private List<TMPro.TextMeshProUGUI> _questStatus;
	[SerializeField] private List<Transform> _questLines;

	[SerializeField] private GameObject _panel;
	[SerializeField] private GameObject _button;

	[SerializeField] private Color _CompletedColor;

	[SerializeField] private Animator _animator;


	private bool _isOpen = false;

	public void EnableQuestUI(bool pIsEnable)
	{
		_panel.SetActive(pIsEnable);
		_button.SetActive(pIsEnable);
	}

	public void OnButtonClick()
	{
		_isOpen = !_isOpen;

		if (_isOpen) Open();
		else Close();
	}

	public void Open()
	{
		_animator.SetTrigger("Open");
	}

	public void Close()
	{
		_animator.SetTrigger("Close");
	}

	public void UpdateLines(List<QuestStatus> pQuests)
	{
		int length = _questLines.Count;
		int pQuestsLength = pQuests.Count;
		QuestStatus currentQuest;

		for (int i = 0; i < length; i++)
		{
			if (i < pQuestsLength)
			{
				_questLines[i].gameObject.SetActive(true);

				currentQuest = pQuests[i];
				_questTexts[i].text = currentQuest.Instructions;
				_questStatus[i].text = currentQuest.Score.ToString();
				_questDiffculty[i].text = currentQuest.Difficulty == 0 ? "Easy" : currentQuest.Difficulty == 1 ? "Medium" : "Hard";

				Image[] images = _questLines[i].GetComponentsInChildren<Image>();

				Color color = currentQuest.IsCompleted ? _CompletedColor : Color.white;
				foreach (Image image in images)
					image.color = color;
			} else
			{
				_questLines[i].gameObject.SetActive(false);
			}
		}
	}
}

public class QuestStatus
{
	public uint Difficulty;
	public string Instructions;
	public uint Score;
	public bool IsCompleted;
}