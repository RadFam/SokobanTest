using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataForField", menuName = "ScriptableObjects/DataForField", order = 2)]
public class DataForField : ScriptableObject 
{
	public int fieldXsize;
	public int fieldYsize;
	public List<Vector2Int> fieldWalls;
	public List<Vector2Int> fieldTargets;
	public List<Vector2Int> fieldBoxes;
	public Vector2Int playerPosition;
}
