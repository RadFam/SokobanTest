using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileDataObject", order = 1)]
public class TileData : ScriptableObject 
{
	public TileBase[] tiles;

	public bool isObstacle;
	public bool isTarget;	
}
