
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
 * @date 2/3/2015
 */
  

namespace maps
{
	class RiverGenerator
	{
		//constants
		public enum RiverType
		{
			triplePointRivers = 1,
			nPointRivers = 2,
			defaultRivers = 0
		}
		public enum alternatePathingType
		{
			none,
			some,
			all
		}
		public enum smoothPoints
		{
			smoothPoints,
			randomPoints
		}
		/*//instance fields
		readonly int mp.getX(), mp.getY();
		Random mp.getRNG();
		/*
		//constructors
		public RiverGenerator() : this(new Random(), 0, 0) {}
		public RiverGenerator(Random newRng) : this(newRng, 0, 0) {}
		public RiverGenerator(Random newRng, int xSize, int ySize)
		{
			mp.getX() = xSize;
			mp.getY() = ySize;
			mp.getRNG() = newRng;
		}
		//main method
		*/
		
		//this river generates to the exclusion of mountain tiles
		
		// public bool[,] generateRivers(Map mp, bool[] alternatePathing, Point[] locRange)
		// {
			// Tile[,] passableTiles = new Tile[mp.getX(), mp.getY()];
			// for(int i = 0; i < mp.getX(); i++)
				// for(int j = 0; j < mp.getY(); j++)
					// passableTiles[i, j] = new Plains();
			// return generateRivers(mp, passableTiles, null, null);
		// }
		//this method randomly generates through points, start and end points and alternate pathing based on the rivertype int and the integer number of throughpoints
		public static bool[,] generateRivers(Map mp)
		{
			int freePoints = mp.getBehavior().free_River_Points;
			int alternatePathing = mp.getBehavior().alternate_Path;
			//int smoothingPoints = mp.getBehavior().smoothing_Points;
			for(int attemptNo = 0; attemptNo < mp.getBehavior().ReasonableNumRiverAttempts; attemptNo++)
			{
				Debug.Log("attempt no:" + attemptNo);
				bool northToSouth = mp.getRNG().nextBool();
				Point start = new Point(northToSouth ? mp.getRNG().nextInt(mp.getX()) : 0, northToSouth ? 0 : mp.getRNG().nextInt(mp.getY()));
				Point end = new Point(northToSouth ? mp.getRNG().nextInt(mp.getX()) : mp.getX() - 1, northToSouth ? mp.getY() - 1 : mp.getRNG().nextInt(mp.getY()));
				Point[] locRange = new Point[freePoints + 2];
				bool[] alternatePathingArray = new bool[freePoints + 1];
				locRange[0] = start;
				locRange[locRange.Length - 1] = end;
				for(int i = 0; i < freePoints; i++)
				{
					//first determine whether each path should allow for alternate pathing
					switch(alternatePathing)
					{
						case (int)alternatePathingType.none:
							alternatePathingArray[i] = false;
							break;
						case (int)alternatePathingType.some:
							alternatePathingArray[i] = mp.getRNG().nextBool();
							break;
						case (int)alternatePathingType.all:
							alternatePathingArray[i] = true;
							break;
						default:
							alternatePathingArray[i] = false;
							break;
					}
					//next create points
					locRange[i + 1] = new Point(mp.getRNG().nextInt(mp.getX()), mp.getRNG().nextInt(mp.getY()));
				}
				
					switch(alternatePathing)
					{
						case (int)alternatePathingType.none:
							alternatePathingArray[freePoints] = false;
							break;
						case (int)alternatePathingType.some:
							alternatePathingArray[freePoints] = mp.getRNG().nextBool();
							break;
						case (int)alternatePathingType.all:
							alternatePathingArray[freePoints] = true;
							break;
						default:
							alternatePathingArray[freePoints] = false;
							break;
					}
				// if(smoothingPoints == (int)smoothPoints.smoothPoints)
				// {
					// //blah blah blah
					// //dunno what to implement here as smoothing rivers may be a little difficult
				// }
				//need ot verify whether a valid river has been produced.
				
				bool[,] success = generateRivers(mp, alternatePathingArray, locRange);
				if(success != null)
					return success;
			}
			return null;
		}
		/**
		 * generateRiver method
		 * this method will take input and delive a river that connects several points and contains alternate pathing if instructed.
		 * @param passableTiles
		 * this is the map of tiles that the river will be built on. this is necesary to provide a decent looking river that 'snakes' through the terrain.
		 * @param alternatePathing 
		 * this parameter is of size locRange - 1 and determines whether the index section of the journey will use alternat pathing.
		 * \n -- if this method is not passed any booleans, it will assume false.
		 * \n -- if this method is not passed enough booleans, it will use the last indexed bool as the basis for any sections beyond the highest one indexed.
		 * @param locRange
		 * this parameter indicates the points that the river should pass through. with the first point, the 'start' at index 0, and the 'end' at index size - 1.
		 * \n --if this method is not passed any points, it will generate two points on a north-south or east-west basis.
		 * \n --if this method is passed one point, it will generate an endpoint and use the one given as a start point.
		 * @ver 1.0
		 * @date 2/17/2015
		 */
		public static bool[,] generateRivers(Map mp, bool[] alternatePathing, Point[] locRange)
		{
			if(locRange.Length < 1)
			{
			bool northToSouth = mp.getRNG().nextBool();
			Point start = new Point(northToSouth ? mp.getRNG().nextInt(mp.getX()) : 0, northToSouth ? 0 : mp.getRNG().nextInt(mp.getY()));
			Point end = new Point(northToSouth ? mp.getRNG().nextInt(mp.getX()) : mp.getY() - 1, northToSouth ? mp.getX() - 1 : mp.getRNG().nextInt(mp.getY()));
			locRange = new Point[2] {start, end};
			}
			else if(locRange.Length < 2)
			{//need to now determine an end point.
				int endDir = mp.getRNG().nextInt(4);
				Point end;
				switch(endDir)
				{
					case 0:
					end = new Point(0, mp.getRNG().nextInt(mp.getY()));
					break;
					case 1:
					end = new Point(mp.getX() - 1, mp.getRNG().nextInt(mp.getY()));
					break;
					case 2:
					end = new Point(mp.getRNG().nextInt(mp.getX()), 0);
					break;
					default:
					end = new Point(mp.getRNG().nextInt(mp.getX()), 0);
					break;
				}
				//now concat the new endpoint onto the existing array
				//will it go out of scope?
				//probably
				Point tempStart = locRange[1];
				//is this a proper reDim?
				locRange = new Point[2] {tempStart, end};
			}
			if(alternatePathing == null)
				alternatePathing = new bool[1] {false};
			bool[,] toReturn = nPointRiver(mp, alternatePathing, locRange);
			if(toReturn != null)
			{	
				toReturn[locRange[locRange.Length - 1].getX(), locRange[locRange.Length - 1].getY()] = true;
				toReturn[locRange[0].getX(), locRange[0].getY()] = true;
			}
			return toReturn;
			
		}
		//worker method uses pathfinding!
		private static bool[,] nPointRiver(Map mp, bool[] alternatePathing, Point[] locRange)
		{
			if(locRange.Length  < 2)
				return null;
			bool[,] river = new bool[mp.getX(), mp.getY()];
			Path riverPath = new Path();
			for(int pointIterator = 0; pointIterator < locRange.Length - 1; pointIterator++)
			{
				int alternateIterator = pointIterator;
				if(alternatePathing.Length <= alternateIterator)
					alternateIterator = alternatePathing.Length - 1;
				Path addPath = mp.findPath(alternatePathing[alternateIterator], locRange[pointIterator], locRange[pointIterator + 1], (int)Pathfinder.TileCost.river);
				if(addPath == null)
					return null;
				riverPath.push(addPath);
			}
			Debug.Log("succesfully made river, drawing");
			if(!riverPath.isEmpty())
				riverPath.push(locRange[0]);
			while (!riverPath.isEmpty()) 
			{
				mp.set(riverPath.peek(), new River());
				river[riverPath.peek().getX(), riverPath.peek().getY()] = true;
				if(mp.getRNG().nextInt(100) < 3)
				{
					mp.set(riverPath.peek(), new Bridge());
					river[riverPath.peek().getX(), riverPath.peek().getY()] = false;
				}
				riverPath.pop();
			}
			return river;
		}
		//workerMethods
	}
}