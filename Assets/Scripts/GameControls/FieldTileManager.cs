using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameControls
{
	public class FieldTileManager : FieldManager 
	{
		public Grid fieldGrid;
		public Tilemap filedMap;
		public Camera mainCamera;

		// Size of field in tiles
		[SerializeField]
		int deltaXTile;
		[SerializeField]
		int deltaYTile;
		[SerializeField]
		List<TileData> tileDatas;
		Dictionary<TileBase, TileData> dataFromTiles;
		
		void Start () 
		{
			dataFromTiles = new Dictionary<TileBase, TileData>();

			foreach(TileData tileData in tileDatas)
			{
				foreach(TileBase tile in tileData.tiles)
				{
					dataFromTiles.Add(tile, tileData);
				}
			}	
		}

		public override Vector3 GetAdaptedCoordinates(Vector3 initCoords)
		{
			Vector3Int coordInTiles = filedMap.WorldToCell(initCoords);
			return filedMap.CellToWorld(coordInTiles) + new Vector3(0.5f, 0.5f, 0.0f);
		}

		public override bool IsPassible(Vector3 coords)
		{
			// Get next tile coords
			Vector3Int coordsPos = filedMap.WorldToCell(coords);
			
			if ((coordsPos.x >= -1*deltaXTile/2 && coordsPos.x <= deltaXTile/2) && (coordsPos.y >= -1*deltaYTile/2 && coordsPos.y <= deltaYTile/2))
			{
				TileBase tileOfInterest = filedMap.GetTile(coordsPos);
				return !dataFromTiles[tileOfInterest].isObstacle;
			}

			return false;
		}

		public override bool IsTarget(Vector3 coords)
		{
			Vector3Int coordsPos = filedMap.WorldToCell(coords);
			if ((coordsPos.x >= -1*deltaXTile/2 && coordsPos.x <= deltaXTile/2) && (coordsPos.y >= -1*deltaYTile/2 && coordsPos.y <= deltaYTile/2))
			{
				TileBase tileOfInterest = filedMap.GetTile(coordsPos);

				return dataFromTiles[tileOfInterest].isTarget;
			}

			return false;
		}
	}
}