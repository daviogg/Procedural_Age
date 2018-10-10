
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	#region Take prefabs
	[Header("Prefabs")]
	public GameObject player;
	public GameObject enemy;
	public GameObject wall;
	[Space(10)]
	[Header("Tiles")]
	public GameObject[] tiles;
	[HideInInspector]
	public Transform environment;
	[HideInInspector]
	public  Transform enemies;
	#endregion


	#region Tiles and boss amount and spawning time
	public List<Vector3> createdTiles;
	public int tileAmount;
	public int tileSize;
	public float waitTime;
	public int enemyAmount = 10;
	#endregion


	#region Random variables
	public float chanceUp;
	public float chanceRight;
	public float chanceDown;
	#endregion


	#region Wall variables
	private float min_Y = 99999999;
	private float max_Y = 0;
	private float min_X = 99999999;
	private float max_X = 0;
	private float xAmount;
	private float yAmount;

	public float extraWallX;
	public float extraWallY;
	#endregion


	#region Start method initilize coroutine

	void Awake(){
		Instantiate(player, new Vector3(0,0,0), Quaternion.identity);
	}
	void Start()
	{
		environment = new GameObject().transform;
		environment.name = "Environment";

		enemies = new GameObject().transform;
		enemies.name = "Enemies";

		StartCoroutine(GenerateLevel());
	}
	#endregion


	IEnumerator GenerateLevel()
	{
		for (int i = 0; i < tileAmount; i++)
		{
			float dir = Random.Range(0f, 1f);
			int tile = Random.Range(0, tiles.Length);

			CreateTile(tile);
			CallMoveGen(dir);

			yield return new WaitForSeconds(waitTime);

			if(i == tileAmount -1)
			{
				Finish();
			}
		}

		yield return 0;	
	}

	void CallMoveGen(float randDir)
	{
		if(randDir < chanceUp)
		{
			MoveGen(0);
		}else if(randDir < chanceRight){
			MoveGen(1);
		}else if(randDir < chanceDown)
		{
			MoveGen(2);
		}
		else
		{
			MoveGen(3);
		}

	}

	void MoveGen(int dir)
	{
		switch (dir)
		{
			case 0:

				transform.position = new Vector3(transform.position.x, transform.position.y + tileSize, 0f );
				break;
			case 1:

				transform.position = new Vector3(transform.position.x + tileSize, transform.position.y, 0f);
				break;
			case 2:

				transform.position = new Vector3(transform.position.x , transform.position.y - tileSize, 0f);
				break;
			case 3:
				transform.position = new Vector3(transform.position.x - tileSize, transform.position.y, 0f);
				break;
			default:
				break;
		}


	}
	
	void CreateTile(int tileIndex)
	{
		
		if (!createdTiles.Contains(transform.position))
		{
			GameObject tileObject;
			tileObject = Instantiate(tiles[tileIndex], transform.position, transform.rotation) as GameObject;
			createdTiles.Add(tileObject.transform.position);
			tileObject.transform.parent = environment;
		}
		else
		{
			tileAmount++;
		}
		
	}

	void Finish()
	{
		CreateWallValues();
		CreateWalls();
		SpawnObjects();
	}

	void CreateWallValues()
	{
		for (int i = 0; i < createdTiles.Count; i++)
		{
			if (createdTiles[i].y < min_Y)
			{
				min_Y = createdTiles[i].y;
			}

			if (createdTiles[i].y > max_Y)
			{
				max_Y = createdTiles[i].y;
			}

			if (createdTiles[i].x < min_X)
			{
				min_X = createdTiles[i].x;
			}

			if (createdTiles[i].x > max_X)
			{
				max_X= createdTiles[i].x;
			}

			xAmount = ((max_X - min_Y )/ tileSize) + extraWallX;
			yAmount = ((max_Y - min_Y) / tileSize) + extraWallY;
		}
	}

	void CreateWalls()
	{
		float _x = min_X - (extraWallX * tileSize) / 2;
		float _y = min_Y - (extraWallY * tileSize) / 2;

		for (int x = 0; x < xAmount; x++)
		{
			for (int y = 0; y < xAmount; y++)
			{
				if(!createdTiles.Contains(
					new Vector3( _x + (x*tileSize), _y + (y*tileSize))  ))
				{
					GameObject wallObj = Instantiate(wall, new Vector3(_x + (x * tileSize), _y + (y * tileSize)), transform.rotation);
					wallObj.transform.parent = environment;
				}
			}
		}
	}

	void SpawnObjects()
	{
        player.transform.position = createdTiles[Random.Range(0, createdTiles.Count)];
        player.transform.rotation = Quaternion.identity;

        for (int i = 0; i < enemyAmount; i++)
		{
			GameObject enemyObj = Instantiate(enemy, createdTiles[Random.Range(0, createdTiles.Count)], Quaternion.identity);
			enemyObj.transform.parent = enemies;
		}
	}

}
