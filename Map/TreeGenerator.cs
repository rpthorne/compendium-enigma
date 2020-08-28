

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
 * @date 12/28/2015
 */
  
 
namespace maps
{

	class TreeGenerator
	{
		private static readonly double ZERO = .000001;
		//constants
		private enum LargeForest
		{
			min = 3,
			max = 6,
			density = 400
		};
		private enum SmallForest
		{
			min = 1,
			max = 3,
			density = 500
		};
		private enum Glade
		{
			min = 1,
			max = 2,
			density = 100
		};
		public enum CoverageTypes
		{
			smallForests = 1,
			largeForests = 2,
			smoothForests = 3,
			randomTrees = 0
		};
		/*
		//instance fields 
		private int mp.getX(), mp.getY();
		//if my understanding of OOP is correct this should 
		//reference the map's RNG and have equal interference 
		//with said RNG
		private Random rng;
		
		
		public TreeGenerator(Random importRng): this(importRng, 0, 0)
		{
		}
		
		public TreeGenerator(Random importRng, int mapX, int mapY)
		{
			rng = importRng;
			mp.getX() = mapX;
			mp.getY() = mapY;
		}*/
		
		/**
		 * @return
		 * this method should return a array of trues and falses designating tree coverage
		 * it will --not-- give you a map of the appropriate tile types. dont use it like that!
		 * @param generationType
		 * generation types are as follows
		 * (0) default randomly determine if a tree should be placed based on
		 *  	treeCoverage as a percentage
		 * (1) type 1: generate tree coverage with forest clumps in random 
		 *  	places based on the treeCoverage
		 * (2) generate larger forests with small 'glades' of nonforested areas
		 * (3) doubly-random version of default tree coverage- has less randomness
		 * 		and a more bell curve treecoverage value
		 * (4)
		 * (6-15) custom defined treeCoverage methods see note at bottom
		 * @param treeCoverage
		 * 		this is a percentage of treecoveage across the map
		 * also note if you want to implement further treeCoverage types you can
		 * do so in an extended class implementing the extraCoverage0,
		 * extraCoverage1, extraCoverage2 etc. up to 9
		 * (up to 10 are implementeable w/o modifying source)
		 * 
		 */
		public static bool[,] generateTrees(Map mp)
		{
			int generationType = mp.getBehavior().tree_Generation_Form;
			int treeCoverage = mp.getBehavior().tree_Coverage_Percentage;
			bool[,] trees;
			switch(generationType)
			{
			case 1://small circular forest clusters
				trees = smallForestCoverage(mp, treeCoverage);
					break;
			case 2://large forests with 'glades'
				trees = largeForestCoverage(mp, treeCoverage);
					break;
			case 3:
				trees = defaultCoverageWithSmoothing(mp, treeCoverage);
					break;
			case 4:
				trees = defaultCoverageWithSmoothing(mp, treeCoverage);
					break;
				/*
			case 6:
				trees = extraCoverage0(treeCoverage);
				
			case 7:
				trees = extraCoverage1(treeCoverage);
				
			case 8:
				trees = extraCoverage2(treeCoverage);
				
			case 9:
				trees = extraCoverage3(treeCoverage);
				
			case 10:
				trees = extraCoverage4(treeCoverage);
				
			case 11:
				trees = extraCoverage5(treeCoverage);
				
			case 12:
				trees = extraCoverage6(treeCoverage);
				
			case 13:
				trees = extraCoverage7(treeCoverage);
				
			case 14:
				trees = extraCoverage8(treeCoverage);
				
			case 15:
				trees = extraCoverage9(treeCoverage);
				*/
			default:
				trees = defaultTreeCoverage(mp, treeCoverage);
					break;
			}
			for(int i = 0; i < mp.getX(); i++)
				for(int j = 0; j < mp.getY(); j++)
					if(trees[i, j])
						mp.set(new Point(i, j), new Trees());//set the forest tiles here :>
			return trees;
		}
		
		private static bool[,] largeForestCoverage(Map mp, int treeCoverage)
		{
			bool[,] treeMap = new bool[mp.getX(), mp.getY()];
			int[,] treeProbability = clusterGenerator(mp, treeCoverage, (int)LargeForest.density, (int)LargeForest.min, (int)LargeForest.max);
			int[,] removeTreeProbability = clusterGenerator(mp, treeCoverage, (int)Glade.density, (int)Glade.min, (int)Glade. max);
			//here is where the large forest creator differs from the small one, it createes smaller 'clearings that reduce the odds of a tree occuring there
			//note: if a treeprobability tile becomes less than 0 here, it will ensure that no tree occurs there	
			for(int i = 0; i < mp.getX(); i++)
				for(int j = 0; j < mp.getY(); j++)
					treeProbability[i,j] -= removeTreeProbability[i,j];
			//then translate that map into acual coordinates using an rng				->RNG!
			for(int i = 0; i < mp.getX(); i++)
				for(int j = 0; j < mp.getY(); j++)
					if(mp.getRNG().nextInt(100) < treeProbability[i,j])
						treeMap[i,j] = true;
					else
						treeMap[i,j] = false;
			return treeMap;
		}
		/*
		 * diagram this out later brah and provide a link
		 */
		private static bool[,] smallForestCoverage(Map mp, int treeCoverage)
		{
			bool[,] treeMap = new bool[mp.getX(), mp.getY()];
			int[,] treeProbability = clusterGenerator(mp, treeCoverage, (int)SmallForest.density, (int)SmallForest.min, (int)SmallForest.max);

			//then translate that map into acual coordinates using an rng				->RNG!
				for(int i = 0; i < mp.getX(); i++)
					for(int j = 0; j < mp.getY(); j++)
						if(mp.getRNG().nextInt(100) < treeProbability[i,j])
							treeMap[i,j] = true;
						else
							treeMap[i,j] = false;
			return treeMap;
		}
		private static bool[,] defaultCoverageWithSmoothing(Map mp, int treeCoverage)
		{
			bool[,] treeMap = defaultTreeCoverage(mp, treeCoverage);
			return smoothing(mp, treeMap);
		}
		private static bool[,] defaultTreeCoverage(Map mp, int treeCoverage)
		{
			bool[,] treeMap = new bool[mp.getX(),mp.getY()];
			for(int xPos = 0; xPos < mp.getX(); xPos++)
				for(int yPos = 0; yPos < mp.getY(); yPos++)
					if(mp.getRNG().nextInt(100) < treeCoverage)
						treeMap[xPos,yPos] = true;
					else
						treeMap[xPos,yPos] = false;
			return treeMap;
		}
		//credit for smoothing algorithm comes from Sebastian Lague
		private static bool[,] smoothing(Map mp, bool[,] treeMap)
		{
            bool[] threshold = { true, true, true };
            return lifeSmoothing(mp, treeMap, threshold, true);
		}

        //threshold is expected to be about size 8 
        private static bool[,] lifeSmoothing(Map mp, bool[,] treeMap, bool[] threshold, bool isReductiveOnly)
        {
            bool[] correctThreshold = new bool[8];
            for (int i = 0; i < correctThreshold.Length; i++)
                if (i >= threshold.Length)
                    correctThreshold[i] = false;
                else
                    correctThreshold[i] = threshold[i];

            for (int xPos = 1; xPos < treeMap.GetLength(0) - 1; xPos++)
                for (int yPos = 1; yPos < treeMap.GetLength(1) - 1; yPos++)
                    if (!isReductiveOnly || treeMap[xPos, yPos])
                    {
                        int neighbors = 0;
                        for (int xAdjacent = xPos - 1; xAdjacent <= xPos + 1; xAdjacent++)
                            for (int yAdjacent = yPos - 1; yAdjacent <= yPos + 1; yAdjacent++)
                                if (xAdjacent != xPos || yAdjacent != yPos)
                                    if (treeMap[xAdjacent, yAdjacent])
                                        neighbors++;
                        if (correctThreshold[neighbors])
                            treeMap[xPos, yPos] = false;    
                    }
            return treeMap;
        }


		private static int[,] clusterGenerator(Map mp, int coverage, int density, int minClusterSize,int maxClusterSize)
		{
			//first determine the number of clusters and their specific radii
			int[,] clusterMap = new int[mp.getX(), mp.getY()];
			for(int clusterNum = 0; clusterNum < (coverage * mp.getX() * mp.getY() / density); clusterNum++)
			{
				int radius = mp.getRNG().nextInt(maxClusterSize - minClusterSize) + minClusterSize;
				clusterNum += (radius - 1) ; //larger clusters have a greater impact on the number of possible clusters
				//then determine for each circle the percentage of treecoverages
				//  first compile a list of all points that lie along the circle and determine their orientations
				int xCenter = mp.getRNG().nextInt(mp.getX());
				int yCenter = mp.getRNG().nextInt(mp.getY());
				PointD center = new PointD(xCenter + .5, yCenter + .5);
				PointD[] pointList = new PointD[radius * 8 + 1];
				PointD[] mergeList = new PointD[radius * 2];
				int counter = 0;
				for(int i = (int)center.getX() - radius + 1; i <= (int)center.getX(); i++)
				{ 
					double b = Math.Sqrt(radius * radius - (center.getX() - i) * (center.getX() - i));
					mergeList[counter++] = new PointD(i, center.getY() + b);
				}
				for(int j = (int)center.getY() + 1; j <= (int)center.getY() + radius; j++)
				{
					double a = Math.Sqrt(radius * radius - (j - center.getY()) * (j - center.getY()));
					mergeList[counter++] = new PointD(center.getX() - a, j);
				}
				//sort them in ascending mp.getX() position order //use merge sort
				int yloc = radius;
				int xloc = 0;
				counter = 0;
				while(xloc < radius && yloc < 2 * radius)
				{
					if(mergeList[xloc].getX() > mergeList[yloc].getX())
					{
						pointList[counter++] = mergeList[yloc];
						yloc++;
					}
					else
					{
						pointList[counter++] = mergeList[xloc];
						xloc++;
					}
				}
				while(xloc < radius)
				{
					pointList[counter++] = mergeList[xloc];
					xloc++;
				}
				while(yloc < 2 * radius)
				{
					pointList[counter++] = mergeList[yloc];
					yloc++;
				}
				//counter should be the position of the next, unfilled, index
				//now duplicate the quadrant
				for(int i = (2 * radius) - 1; i >= 0; i--)
					pointList[counter++] = new PointD(2 * center.getX() - pointList[i].getX(),pointList[i].getY());
				//now duplicate the half
				for(int j = (4 * radius) - 1; j >= 0; j--)
					pointList[counter++] = new PointD(pointList[j].getX(),2 * center.getY() - pointList[j].getY());
				//double the first one into the last place to find the area of the structure when it loops
				pointList[counter] = (new PointD(pointList[0].getX(), pointList[0].getY()));
				//phew half way done! now calculate areas with them
				
				
				//take the first two points via for loop
				for(int listPos = 0; listPos < pointList.Length - 1; listPos++)
				{
					//determine which point in the grid the area is being observed for
					//find midpoint, truncate decimal
					PointD alpha = pointList[listPos];		//	first point 
					PointD beta = pointList[listPos + 1];	//	second point
					Point position = new Point((int)Math.Floor((alpha.getX() + beta.getX()) / 2), (int)Math.Floor((alpha.getY() + beta.getY()) / 2));
					//do rotations here
					//so sue me its not as efficient as it could be
					//probably should have done integrals here hehe
					PointD alphat = new PointD(alpha.getX(),alpha.getY());		//temporary alpha
					PointD betat = new PointD(beta.getX(), beta.getY());		//temporary beta
					while(!((Math.Abs(betat.getX() - 1.0 - position.getX()) <= ZERO && Math.Abs(alphat.getY() - position.getY()) <= ZERO)
						||	(Math.Abs(betat.getX() - 1.0 - position.getX()) <= ZERO && Math.Abs(alphat.getX() - position.getX()) <= ZERO)
						||	(Math.Abs(alphat.getX() - position.getX()) <= ZERO  && Math.Abs(betat.getY() - 1.0 - position.getY()) <= ZERO)))
					{
						//the added .5 is so that we rotate around the center of teh point and don't rotate to an erroneous position
						alphat.rotate90(position.getX() + .5, position.getY() + .5);
						betat.rotate90(position.getX() + .5, position.getY() + .5);	
					}
					alpha = alphat;
					beta = betat;
					//select which form the line takes
						/*
						 *  |-----|	|--B--|	|-----|
						 *  |     B	| /FFF|	|     |
						 *  |    /|	|/FFFF|	|  ---B
						 *  |   /F|	AFFFFF|	A--FFF|
						 *  |--A--|	|-----|	|-----|
						 * 		1 		2		3
						 */
					double area = 0;
					//case 3
					if(alpha.getX() - beta.getX() == -1.0)
						area = (alpha.getY() + beta.getY()) / 2.0 - position.getY();
					//case 1
					else if(position.getY() == alpha.getY())
						area = (beta.getX() - alpha.getX()) * (beta.getY() - alpha.getY()) / 2.0;
					//case 2
					else if(position.getX() == alpha.getX())
						area = 1.0 - (beta.getX() - alpha.getX()) * (beta.getY() - alpha.getY()) / 2.0;
					else
						area = -1.0;
					area *= 100;
					//put area in the appropriate grid location (test if out of bounds too)
					if(position.getX()  >= 0 && position.getX() < mp.getX() && position.getY() >= 0 && position.getY() < mp.getY())
						clusterMap[position.getX(), position.getY()] += (int)area;
					for(int i = 0; i < radius; i++)
						for(int j = 0; j < Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(i,2)); j++)
						{
							int boundryx, boundryy;
							boundryx = (int) (center.getX()) + i;
							boundryy = (int) (center.getY()) + j;
							if(boundryx < mp.getX() && boundryx >= 0 && boundryy >= 0 && boundryy < mp.getY())
								if(clusterMap[boundryx, boundryy] == 0)
									clusterMap[boundryx, boundryy] = 100;
							boundryx = (int) (center.getX()) + i;
							boundryy = (int) (center.getY()) - j;
							if(boundryx < mp.getX() && boundryx >= 0 && boundryy >= 0 && boundryy < mp.getY())
								if(clusterMap[boundryx, boundryy] == 0)
									clusterMap[boundryx, boundryy] = 100;
							boundryx = (int) (center.getX()) - i;
							boundryy = (int) (center.getY()) + j;
							if(boundryx < mp.getX() && boundryx >= 0 && boundryy >= 0 && boundryy < mp.getY())
								if(clusterMap[boundryx, boundryy] == 0)
									clusterMap[boundryx, boundryy] = 100;
							boundryx = (int) (center.getX()) - i;
							boundryy = (int) (center.getY()) - j;
							if(boundryx < mp.getX() && boundryx >= 0 && boundryy >= 0 && boundryy < mp.getY())
								if(clusterMap[boundryx, boundryy] == 0)
									clusterMap[boundryx, boundryy] = 100;
						}
				}
			}			
			return clusterMap;
		}		
	}
}