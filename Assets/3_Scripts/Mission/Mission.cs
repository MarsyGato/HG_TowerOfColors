using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Mission", menuName = "Mission/Mission", order = 0)]
public class Mission : ScriptableObject
{
	public uint Difficulty = 0;
	public string Instructions = "BlaBlaBla";
	public string Id;
	public Reward Reward;

	public uint Quantity = 1;

}