
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

using maps;
using general;
using mobiles;

/**
 * @author Ryan Thorne
 * @date 2/1/2015
 */
  
 
namespace maps
{
	class MountainGenerator
	{
		//constants
		enum MountainType
		{
			smoothMtnRanges = 1
		}
		/*Random mp.getRNG();
		int mp.getX(), mp.getY();
		//constructors
		public MountainGenerator() : this(new Random(), 0, 0)
		{
		}
		public MountainGenerator(Random newRNG) : this(newRNG, 0, 0)
		{
		}
		public MountainGenerator(Random newRNG, int xSize, int ySize)
		{
			mp.getX() = xSize;
			mp.getY() = ySize;
			mp.getRNG() = newRNG;
		}*/
		public static bool[,] generateMountains(Map mp)
		{	
			int type = mp.getBehavior().mtnType;
			int mtnCoverage = mp.getBehavior().mtnCoverage;
			bool[,] mtns;
			switch(type)
			{
				case (int)MountainType.smoothMtnRanges :
					mtns = smoothMountainRange(mp, mtnCoverage);
					break;
				default:
					mtns = mountainRange(mp, mtnCoverage);
					break;
			}
			for(int i = 0; i < mp.getX(); i++)
				for(int j = 0; j < mp.getY(); j++)
					if(mtns[i, j])
						mp.set(new Point(i, j), new Mountain());//set the forest tiles here :>
			return mtns;
		}
		private static bool[,] smoothMountainRange(Map mp, int mtnCoverage)
		{
			bool[,] mtnMap = mountainRange(mp, mtnCoverage + 5);
			return smoothing(mp, mtnMap);
		}
		private static bool[,] mountainRange(Map mp, int mtnCoverage)
		{
			bool[,] mtnMap = new bool[mp.getX(), mp.getY()];
			for(int i = 0; i < mp.getX(); i++)
				for(int j = 0; j < mp.getY(); j++)
					mtnMap[i, j] = false;
			// inserts mountain coverage
			// number of mountain structures is based on the square area of the map
			int area = mp.getX() * mp.getY();
			int mountainClusters = mtnCoverage * area / 200;
			for (int currentCluster = 0; currentCluster < mountainClusters; currentCluster++) 
			{
				int xRNG = mp.getRNG().nextInt(mp.getX());
				int yRNG = mp.getRNG().nextInt(mp.getY());
				mtnMap[xRNG,yRNG] = true;
				int clusterSize = mp.getRNG().nextInt(5) + 5;		//TODO figure out a way to relate this to the already given numbers
				for (int mtn = 0; mtn < clusterSize; mtn++) 
				{
				int dir = mp.getRNG().nextInt(4);
						if (dir == 0 && xRNG < mp.getX() - 1)
							xRNG++;
						else if (dir == 1 && xRNG > 0)
							xRNG--;
						else if (dir == 2 && yRNG < mp.getY() - 1)
							yRNG++;
						else if (dir == 3 && yRNG > 0)
							yRNG--;
						else
							mtn++; // not an unsafe method: can never result in an		//double check this 2/2/2016 changed 'mtn--' to 'mtn++'
								// infinite loop
						mtnMap[xRNG,yRNG] = true;
				}
			}
			return mtnMap;
		}
		//credit for smoothing algorithm comes from Sebastian Lague
		private static bool[,] smoothing(Map mp, bool[,] mtnMap)
		{
			for(int xPos = 1; xPos < mtnMap.GetLength(0) - 1; xPos++)
				for(int yPos = 1; yPos < mtnMap.GetLength(1) - 1; yPos++)
					if(mtnMap[xPos, yPos])
					{
						int neighbors = 0;
						for(int xAdjacent = xPos - 1; xAdjacent <= xPos + 1; xAdjacent++)
							for(int yAdjacent = yPos - 1; yAdjacent <= yPos + 1; yAdjacent++)
								if(xAdjacent != xPos || yAdjacent != yPos)
									if(mtnMap[xAdjacent, yAdjacent])
										neighbors++;
						if(neighbors < 4 && mtnMap[xPos, yPos] == true)//only do this if the neighbors are greater than four AND the mountain is already there
							mtnMap[xPos, yPos] = false;
					}
			return mtnMap;
		}
	}
}