using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
	[SerializeField] private List<Mission> _missionsEasy = new List<Mission>();
	[SerializeField] private List<Mission> _missionsMedium = new List<Mission>();
	[SerializeField] private List<Mission> _missionsHard = new List<Mission>();
	[SerializeField] private List<Mission> _currentMissions = new List<Mission>();

	[SerializeField] private QuestUI _QuestUI;

	private Dictionary<Mission, uint> _missionCounters = new Dictionary<Mission, uint>();
	private Dictionary<Mission, bool> _missionsAccomplished = new Dictionary<Mission, bool>();

	public List<Mission> CurrentMissions => _currentMissions;
	public Dictionary<Mission, uint> MissionCounters => _missionCounters;
	public Dictionary<Mission, bool> MissionsAccomplished => _missionsAccomplished;

	public static System.Action<string, uint> MissionUpdate;

	private void Awake()
	{
		_QuestUI.EnableQuestUI(RemoteConfig.MISSIONS_ENABLED);
		gameObject.SetActive(RemoteConfig.MISSIONS_ENABLED);
	}

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
		GetNewRandomMissions();

		MissionUpdate += GetMissionUpdate;
	}

	private void SetCounters() 
	{
		_missionCounters = new Dictionary<Mission, uint>();

		foreach (Mission mission in _currentMissions)
			_missionCounters.Add(mission, 0);
	}

	public void GetMissionUpdate(string pMission, uint pQuantity = 1)
	{
		Mission currentMission;

		if (_currentMissions.Count > 0)
		{
			for (int i = _currentMissions.Count - 1; i >= 0; i--)
			{
				currentMission = _currentMissions[i];

				if (currentMission.Id == pMission)
				{
					_missionCounters[currentMission] += pQuantity;

					if (_missionCounters[currentMission] >= currentMission.Quantity)
					{
						if (!_missionsAccomplished[currentMission])
						{
							_missionsAccomplished[currentMission] = true;
							GiveReward(currentMission.Reward); 
							
							bool checkFullyAccomplished = true;

							foreach (Mission mission in _currentMissions)
								if (!_missionsAccomplished[mission]) checkFullyAccomplished = false;

							if (checkFullyAccomplished)
								GetNewRandomMissions();
						}
					}
				}
			}
		}

		if (RemoteConfig.MISSIONS_ENABLED)
			SendDatasToUI();
	}

	private void GiveReward(Reward pReward)
	{
		Debug.Log(pReward._Name);
	}

	public void GetNewRandomMissions()
	{
		_missionsAccomplished = new Dictionary<Mission, bool>();

		Mission mission = _currentMissions[0];
		while (mission == _currentMissions[0]) mission = _missionsEasy[UnityEngine.Random.Range(0, _missionsEasy.Count)] ;
		_currentMissions[0] = mission;
		_missionsAccomplished.Add(mission, false);

		mission = _currentMissions[1];
		while (mission == _currentMissions[1]) mission = _missionsMedium[UnityEngine.Random.Range(0, _missionsMedium.Count)];
		_currentMissions[1] = mission;
		_missionsAccomplished.Add(mission, false);

		mission = _currentMissions[2];
		while (mission == _currentMissions[2]) mission = _missionsHard[UnityEngine.Random.Range(0, _missionsHard.Count)];
		_currentMissions[2] = mission;
		_missionsAccomplished.Add(mission, false);

		SetCounters();

		if (RemoteConfig.MISSIONS_ENABLED)
			SendDatasToUI();
	}

	private void SendDatasToUI()
	{
		List<QuestStatus> status = new List<QuestStatus>();

		int length = _currentMissions.Count;

		QuestStatus iteratedStatus;
		Mission iteratedMission;

		for (int i = 0; i < length; i++)
		{
			iteratedStatus = new QuestStatus();
			iteratedMission = _currentMissions[i];

			iteratedStatus.Difficulty = iteratedMission.Difficulty;
			iteratedStatus.Instructions = iteratedMission.Instructions;
			iteratedStatus.IsCompleted = _missionsAccomplished[iteratedMission];
			iteratedStatus.Score = iteratedStatus.IsCompleted  ? iteratedMission.Quantity : _missionCounters[iteratedMission];

			status.Add(iteratedStatus);
		}
		
		_QuestUI.UpdateLines(status);
	}

	private void OnDestroy()
	{

		MissionUpdate -= GetMissionUpdate;
	}
}
