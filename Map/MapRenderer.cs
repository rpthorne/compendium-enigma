// /**
//  * @author Ryan Thorne
//  * @version 1.0
//  * @date  6 / 11  / 15 
//  */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

using maps;
using general;
using mobiles;


namespace maps
{
	public class MapRenderer : MonoBehaviour
	{
		public enum directions
		{
			Left,
			Up_Left,
			Up,
			Up_Right,
			Right,
			Down_Right,
			Down,
			Down_Left
		}
		
		
		public Map currentMap;
		public Mapini inivals;
		private Random rng;
		public float mountainScale;
		//this is where the art assets are held in arrays to be individually rendered
		public GameObject[] plainsTiles;
		public GameObject[] plainsSprites;
		public GameObject[] treeTiles;
		public GameObject[] treeSprites;
		public GameObject[] treeSpritesNearWater;
		public GameObject[] mountainTiles;
		public GameObject[] hillTiles;
		public GameObject[] houseTiles;
		public GameObject[] riverTiles;
		public GameObject[] roadTiles;
		public GameObject[] bridgeTiles;
		
		public GameObject[] mountainTops1_1;
		public GameObject[] mountainTops2_2;
		public GameObject[] mountainTops2_1;
		public GameObject[] footHills;
		
		public GameObject[] hillSprites;
		
		public GameObject[] fullGradient;
		public GameObject[] insideEdgeGradient;//note: make sure that the pivots are the points toward the center
		public GameObject[] outsideEdgeGradient;
		public GameObject[] leftEdgeGradient;
		public GameObject[] rightEdgeGradient;
		
		public GameObject nullTile;
		public GameObject waterTile;
		private Transform tileHolder;
		private Transform defaultLayer;
		private Transform waterLayer;
		//private BoxCollider2D bx;

		void Awake ()
		{
			//inivals = GetComponent<mapini>().mapini(); // do not use! associates mapini with incorrect value!
			renderLevel ();
			rng = inivals.rng;
		}

		void renderLevel ()
		{
			//generate the map for the game to read
			makeMap();
			tileHolder = new GameObject ("Map").transform;
			//bx = gameObject.AddComponent<BoxCollider2D>();
			//tileHolder.gameObject.AddComponent(bx);
			defaultLayer = new GameObject ("Plains").transform;
			defaultLayer.transform.SetParent(tileHolder);
			waterLayer = new GameObject ("Water").transform;
			waterLayer.transform.SetParent(tileHolder);
			
			//get and show the actual tree TileWeight	
			int[] treeTileWeight = new int[treeTiles.Length];
			for(int i = 0; i < treeTileWeight.Length; i++)
				treeTileWeight[i] = 100 / (i + 1);
			
			//initalize the mountain rendering list
			Group[] mountainGroups = groupTiles(currentMap, new Mountain());
			//debugging line to determine mountain groups
			/*for(int mtnCnt = 0; mtnCnt < mountainGroups.Length; mtnCnt++)
				Debug.Log(mountainGroups[mtnCnt].getLoc().getX() + " " + mountainGroups[mtnCnt].getLoc().getY() + " " + mountainGroups[mtnCnt].getX() + " " + mountainGroups[mtnCnt].getY() + " ");*/
			
			//make all locations of the specific tile type
			for (int xPos = 0; xPos < currentMap.getX(); xPos++) 
				for (int yPos = 0; yPos < currentMap.getY(); yPos++) 
				{
					GameObject toInitialize = nullTile;
					GameObject aesthetic = null;
					GameObject mountain = null;
					GameObject smoothingLayer = null;
					GameObject[] gradientLayer = new GameObject[4];
					switch (currentMap.get (xPos, yPos).getType ()) 
					{
						//initialize plains here
						case '*':
							toInitialize = plainsTiles [rng.nextInt(plainsTiles.Length)];
							aesthetic = plainsSprites[rng.nextInt(plainsSprites.Length)];
							//now determine what is next to the plainsTiles
							if(hasNeighbor(xPos, yPos, new Trees()))
							{
								/*bool[] surroundingForests = neighborDirection(xPos, yPos, new Trees());
								for(int i = 0; i < gradientLayer.Length; i++)
								{
									if(surroundingForests[2 * i] && surroundingForests[2 * i + 2])
										gradientLayer[i] = insideEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
									else if(surroundingForests[2 * i])
										gradientLayer[i] = rightEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
									else if(surroundingForests[2 * i + 2])
										gradientLayer[i] = leftEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
									else if(surroundingForests[2 * i + 1])
										gradientLayer[i] = outsideEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
									else
										gradientLayer[i] = null;
								}*/
							}
							break;
							//initialize forests
						case '^':
							toInitialize = treeTiles[rng.weightedInt(treeTileWeight)];
							if(hasNeighbor(xPos, yPos, new River()))
							{
								aesthetic = treeSpritesNearWater[rng.nextInt(treeSpritesNearWater.Length)];
							}
							else
							{
								aesthetic = treeSprites[rng.nextInt(treeSprites.Length)];
							}
							bool[] surroundingForests = neighborDirection(xPos, yPos, new Trees());
								for(int i = 0; i < gradientLayer.Length; i++)
								{
									if(surroundingForests[2 * i] && surroundingForests[2 * i + 1] && surroundingForests[2 * i + 2])
										gradientLayer[i] = fullGradient[rng.nextInt(insideEdgeGradient.Length)];
									else if(surroundingForests[2 * i] && surroundingForests[2 * i + 2])
										gradientLayer[i] = insideEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
									else if(surroundingForests[2 * i])
										gradientLayer[i] = leftEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
									else if(surroundingForests[2 * i + 2])
										gradientLayer[i] = rightEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
										
									else
										gradientLayer[i] = outsideEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
								}
							
							break;
							//here be mountains
						case 'A':
						for(int mtnCnt = 0; mtnCnt < mountainGroups.Length; mtnCnt++)	
							if(xPos == mountainGroups[mtnCnt].getLoc().getX() && yPos == mountainGroups[mtnCnt].getLoc().getY())
							{
								if(mountainGroups[mtnCnt].getX() == 1 && mountainGroups[mtnCnt].getY() == 1)
									mountain = mountainTops1_1[rng.nextInt(mountainTops1_1.Length)];
								else if( mountainGroups[mtnCnt].getY() >= 2)//implies large volumus mountains
								{
									mountain = mountainTops2_2[rng.nextInt(mountainTops2_2.Length)];
									//now add mountains spaced 16px -or- .5 units apart untill cannot anymore
									/*for(int mtnI = 0; mtnI < mountainGroups[mtnCnt].getY(); mtnI++)
										for(int mtnL = 1; mtnL < (mountainGroups[mtnCnt].getX() - 1)* (1 / mountainScale);  mtnL++)
										{
											GameObject newMountain;
											newMountain = Instantiate (mountain, new Vector3 (xPos + mtnL * 1f * mountainScale, yPos + mtnI, 8 + (.0078125f * mtnL) + (.03125f * mtnI * 2)), Quaternion.identity) as GameObject;
										}//*/
								}
								else if(mountainGroups[mtnCnt].getX() >= 2)//implies long straight mountains
								{
									mountain = mountainTops2_1[rng.nextInt(mountainTops2_1.Length)];
									//now add mountains spaced 16px -or- .5 units apart untill cannot anymore
									/*for(int mtnL = 1; mtnL < (mountainGroups[mtnCnt].getX() - 1)* (1 / mountainScale);  mtnL++)
									{
										GameObject newMountain;
										newMountain = Instantiate (mountain, new Vector3 (xPos + mtnL * 1f * mountainScale, yPos, 8 + (.03125f * mtnL)), Quaternion.identity) as GameObject;
									}//*/
								}	
								else
									mountain = mountainTops1_1[rng.nextInt(mountainTops1_1.Length)];
								mtnCnt++;
							}
							if(!(new Mountain()).equals(getNeighbor(xPos, yPos, (int)directions.Down)))
								aesthetic = footHills[rng.nextInt(footHills.Length)];
							toInitialize = mountainTiles [rng.nextInt(mountainTiles.Length)];
							break;
							//initialize hills
						case 'n':
							toInitialize = hillTiles [rng.nextInt(hillTiles.Length)];
							aesthetic = hillSprites [rng.nextInt(hillSprites.Length)];
							break;
							//initialize water
						case '_':
							toInitialize = nullTile;//riverTiles [rng.Range (0, riverTiles.Length)];
							if(hasNeighbor(xPos, yPos, new Trees(), new Plains(), new Mountain(), new House(), new Hill()))
							{
								bool[] surroundingTiles = neighborDirection(xPos, yPos, new Trees(), new Plains(), new Mountain(), new House(), new Hill());
								for(int i = 0; i < gradientLayer.Length; i++)
								{
									if(surroundingTiles[2 * i] && surroundingTiles[2 * i + 2])
										gradientLayer[i] = insideEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
									else if(surroundingTiles[2 * i])
										gradientLayer[i] = leftEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
									else if(surroundingTiles[2 * i + 2])
										gradientLayer[i] = rightEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
									else if(surroundingTiles[2 * i + 1])
										gradientLayer[i] = outsideEdgeGradient[rng.nextInt(insideEdgeGradient.Length)];
									else
										gradientLayer[i] = null;
								}
							}
							break;
							//initialize a house
						case 'H':
							toInitialize = houseTiles [rng.nextInt(houseTiles.Length)];
							break;
							//initialize road tiles
						case '-':
							toInitialize = roadTiles [rng.nextInt(roadTiles.Length)];
							break;
							//initialize a bridge
						case 'B':
							toInitialize = bridgeTiles [rng.nextInt(bridgeTiles.Length)];
							break;
							
						default:
							toInitialize = nullTile;
							break;
						}
						GameObject newInstance = null;
						//floor layer
						if(toInitialize != null)
						{
							newInstance = Instantiate (toInitialize, new Vector3 (xPos, yPos, 9), Quaternion.identity) as GameObject;
							newInstance.transform.SetParent (tileHolder);
						}
						//water layer
						GameObject water = Instantiate (waterTile, new Vector3 (xPos, yPos, 10), Quaternion.identity) as GameObject;
						water.transform.SetParent (newInstance.GetComponent<Transform>());
						//mountain layer
						if(mountain != null)
						{
							GameObject mountainInitialize = Instantiate (mountain, new Vector3 (xPos, yPos, 8), Quaternion.identity) as GameObject;
							if(newInstance != null)
								mountainInitialize.transform.SetParent (newInstance.GetComponent<Transform>());
						}
						//gradients layer
						for(int i = 0; i < gradientLayer.Length; i++)
						{
							if(gradientLayer[i] != null)
							{
							GameObject gradientInitialize = Instantiate (gradientLayer[i], new Vector3 (xPos, yPos, 7), Quaternion.identity) as GameObject;
							gradientInitialize.transform.Rotate(Vector3.back * 90 * (i ));
							Color transparent = gradientInitialize.GetComponent<SpriteRenderer>().color;
							transparent.a = 2f;
							gradientInitialize.GetComponent<SpriteRenderer>().color = transparent;
							if(newInstance != null)
								gradientInitialize.transform.SetParent (newInstance.GetComponent<Transform>());
							}
						}
						//aesthetic layer
						if(aesthetic != null)
						{
							float xError = (float) (((int) Random.Range(-6, 6)) / 32.0);
							float yError = (float) (((int) Random.Range(-6, 6)) / 32.0);
							GameObject aestheticInstance = Instantiate (aesthetic, new Vector3 (xPos + xError, yPos + yError, 6), Quaternion.identity) as GameObject;
							aestheticInstance.transform.SetParent (newInstance.GetComponent<Transform>());
						}
				}

		}
		bool hasNeighbor(int xLoc, int yLoc, params Tile[] key)//has neighbor tests for diagonals too overloaded for arrays of tiles
		{
			for(int xNeighbor = xLoc - 1; xNeighbor <= xLoc + 1; xNeighbor++) 
				for(int yNeighbor = yLoc - 1; yNeighbor <= yLoc + 1; yNeighbor++)
					if(xNeighbor >= 0 && xNeighbor < currentMap.getX() 
						&& yNeighbor >= 0 && yNeighbor < currentMap.getY())//in bounds check
						if(xNeighbor != xLoc || yNeighbor != xLoc)
							for(int ikey = 0; ikey < key.Length; ikey++)
								if(currentMap.get(xNeighbor, yNeighbor).equals(key[ikey]))
									return true;
			return false;
		}
		bool[] neighborDirection(int xLoc, int yLoc, params Tile[] key)
		{
			bool[] direction = new bool[9];
			for(int i = 0; i < direction.Length; i++)
			{
				Tile temp;
				switch(i)
				{
					case (int)directions.Left:
					temp = getNeighbor(xLoc, yLoc, i);
					break;
					case (int)directions.Right:
					temp = getNeighbor(xLoc, yLoc, i);
					break;
					case (int)directions.Up:
					temp = getNeighbor(xLoc, yLoc, i);
					break;
					case (int)directions.Down:
					temp = getNeighbor(xLoc, yLoc, i);
					break;
					case (int)directions.Up_Left:
					temp = getNeighbor(xLoc, yLoc, i);
					break;
					case (int)directions.Up_Right:
					temp = getNeighbor(xLoc, yLoc, i);
					break;
					case (int)directions.Down_Left:
					temp = getNeighbor(xLoc, yLoc, i);
					break;
					case (int)directions.Down_Right:
					temp = getNeighbor(xLoc, yLoc, i);
					break;
					case ((int)directions.Down_Left + 1):
					temp = getNeighbor(xLoc, yLoc, i - 8);//cosmic reacharound
					break;
					default:
					temp = null;
					break;
				}
				if(temp != null)
				{
					direction[i] = false;
					for(int iKey = 0; iKey < key.Length; iKey++)
						if(temp.equals(key[iKey]))
							direction[i] = true;
				}
				else
					direction[i] = false;
			}
			return direction;
		}
		Tile getNeighbor(int xLoc, int yLoc, int direction)
		{
			int xNeighbor, yNeighbor;
			switch(direction)
			{
				case (int)directions.Left:
				xNeighbor = xLoc - 1;
				yNeighbor = yLoc;
				break;
				case (int)directions.Right:
				xNeighbor = xLoc + 1;
				yNeighbor = yLoc;
				break;
				case (int)directions.Up:
				xNeighbor = xLoc;
				yNeighbor = yLoc + 1;
				break;
				case (int)directions.Down:
				xNeighbor = xLoc;
				yNeighbor = yLoc - 1;
				break;
				case (int)directions.Up_Left:
				xNeighbor = xLoc - 1;
				yNeighbor = yLoc + 1;
				break;
				case (int)directions.Up_Right:
				xNeighbor = xLoc + 1;
				yNeighbor = yLoc + 1;
				break;
				case (int)directions.Down_Left:
				xNeighbor = xLoc - 1;
				yNeighbor = yLoc - 1;
				break;
				case (int)directions.Down_Right:
				xNeighbor = xLoc + 1;
				yNeighbor = yLoc - 1;
				break;
				default:
				return null;
			}
			if(xNeighbor >= 0 && xNeighbor < currentMap.getX() 
				&& yNeighbor >= 0 && yNeighbor < currentMap.getY())
				return currentMap.get(xNeighbor, yNeighbor);
			return null;
		}
		//thinking of making this a static method ??
		//also speed the efficiency of this up O(n^3)
		public Group[] groupTiles(Map m, params Tile[] type)
		{
			LinkedList<Group> groupList = new LinkedList<Group> ();
			bool[,] openList = new bool[m.getX(), m.getY()];
			for(int xLoc = 0; xLoc < m.getX(); xLoc++)
				for(int yLoc = 0; yLoc < m.getY(); yLoc++)
				{
					openList[xLoc, yLoc] = false;//unusable in the current strategy
					for(int iKey = 0; iKey < type.Length; iKey++)
						if(m.get(xLoc, yLoc).equals(type[iKey]))
							openList[xLoc, yLoc] = true;//this is a tile that we want to group
				}
			for(int xLoc = 0; xLoc < m.getX(); xLoc++)
				for(int yLoc = 0; yLoc < m.getY(); yLoc++)
					if(openList[xLoc, yLoc])
					{//now form a group
						bool rightable = true;
						bool upable = true;
						int xSize = 1;
						int ySize = 1;
						while((rightable || upable) && (xSize < 2 && ySize < 2))
						{
							//go right if !rightable then wont even check
							for(int i = 0; i < ySize && rightable; i++)
							{
								
								rightable = false;
								for(int iKey = 0; iKey < type.Length; iKey++)
								{
									if(type[iKey].equals(getNeighbor(xLoc + xSize - 1, yLoc + i, (int)directions.Right)))
										if(openList[xLoc + xSize, yLoc + i])
											rightable = true;
								}
							}
							if(rightable)
								xSize++;
							for(int j = 0; j < xSize && upable ; j++)
							{
								upable = false;
								for(int iKey = 0; iKey < type.Length; iKey++)
								{
									if(type[iKey].equals(getNeighbor(xLoc + j, yLoc + ySize - 1, (int)directions.Up)))
										if(openList[xLoc + j, yLoc + ySize])
											upable = true;
								}
							}
							if(upable && rightable)
								ySize++;
							else if(upable)
								upable = false;
						}
						Group temp = new Group(new Point(xLoc, yLoc), xSize, ySize);
						//now remove the grouped members from the openList
						for(int i = 0; i < xSize; i++)
							for(int j = 0; j < ySize; j++)
							{
								openList[xLoc + i, yLoc + j] = false;
							}
						//add temp Group to Linked List
						groupList.AddLast(temp);
					}
			Group[] g = new Group[groupList.Count];
			for(int i = 0; i < g.Length; i++)
			{
				g[i] = groupList.Last.Value;
				groupList.RemoveLast();
			}
			return g;
		}
		
		public void makeMap()
		{
			currentMap = new Map(inivals.seed, inivals);
			//change so mapini file is read to the 
			TreeGenerator.generateTrees(currentMap);
			// Debug.Log("made trees");
			MountainGenerator.generateMountains(currentMap);
			// Debug.Log("made mountains");
			RiverGenerator.generateRivers(currentMap);
			//Debug.Log("made rivers");
			TownGenerator.generateTowns(currentMap);
			//Debug.Log("made town");
			//RoadGenerator.generateRoads()//do later
		}
	}
}
