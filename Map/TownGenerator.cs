
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
 * @date 10/14/2015
 * this whole class needs an overhaul
 */

namespace maps
{
	class TownGenerator
	{
		//constants
		//defines the type of town being generated : should have three town sizes small med large
		static Dictionary<int, TownParam> standardTowns =  new Dictionary<int, TownParam>
		{
			{(int)TownType.defaultTown, new TownParam(null, 0, 0)},
			{(int)TownType.aloneTown, new TownParam(null, (int)TownRadius.noTown, (int)TownSize.defaultTown)},
			{(int)TownType.villageTown, new TownParam(null, (int)TownRadius.defaultTown, (int)TownSize.smallTown)},
			{(int)TownType.villageTownNoStructs, new TownParam(null, (int)TownRadius.defaultTown, (int)TownSize.smallTown)},
			{(int)TownType.villageTownWithStructs, new TownParam(null, (int)TownRadius.ruralTown, (int)TownSize.smallTown)},
			{(int)TownType.marketTownSmall, new TownParam(null, (int)TownRadius.defaultTown, (int)TownSize.smallTown)},
			{(int)TownType.marketTownBig, new TownParam(null, (int)TownRadius.urbanTown, (int)TownSize.largeTown)},
			{(int)TownType.largeTown, new TownParam(null, (int)TownRadius.urbanTown, (int)TownSize.massiveTown)}
		};
		
		public enum TownType
		{
			aloneTown = 1,		//a lone house with no supporting structures
			villageTown = 2,	//a small village with optional random supporting structures
			villageTownNoStructs = 3, //small village without any structs
			villageTownWithStructs = 4, //must include atleast one struct
			marketTownSmall = 5, // will include a shop of some sort
			marketTownBig = 6, //must include atleast 2 shops up to the maximum
			largeTown = 7, // a larger version of village with more guaranteed houses
			defaultTown = 0		//not yet sure of default behavior? probably make it suche that there 'is no town'
		}
		//this defines a town as a structure
		public struct TownParam
		{
			public Point center;
			public int radius;	//represents the furthest number of tiles a town can be placed radially
			public int size;	//number of buildings in the town
			public TownParam(Point p, int r, int s)
			{
				center = p;
				radius = r;
				size = s;
			}
			public String toString()
			{
				String g =  "location:" + center.toString() + "radius: " + radius + "number of buildings: " + size;
				return g;
			}
		}
		//defines the size of various town sizes by number of buildings
		public readonly int[] RADIUS_WEIGHTS = {10, 7, 4};
		public readonly int[] SIZE_WEIGHTS = {10, 7, 4, 1};
		
		public enum TownRadius
		{
			noTown = 1,
			defaultTown = 4,
			ruralTown = 15,
			urbanTown = 8
		}
		public enum TownSize
		{
			defaultTown = 1,
			smallTown = 4,
			largeTown = 7,
			massiveTown = 13
		}
		
		//due to a declaration of static, the constructor
		/*
		//instance fields
		readonly int x, y;
		Random mp.getRNG();
		//constructors as usual do not use the first two ever
		public TownGenerator() : this(new Random(), 0, 0) {}
		public TownGenerator(Random newRng) : this(newRng, 0, 0) {}
		public TownGenerator(Random newRng, int
		xSize, int ySize)
		{
			x = xSize;
			y = ySize;
			rng = newRng;
		}*/
		/**
		 * generate towns method
		 * this default generator will give the least control over how towns are generated.
		 * @param passableTiles
		 * the map over which the road will be generated note that the dimensions of the board should be equivalent to x and y
		 * @param numTowns
		 * the strict number of towns that will be generated
		 * @postcondition, a town is generated AND the roads required to linke them are added
		 */
		public static bool generateTowns(Map mp)
		{
			//first generate the list of town centers town center
			TownParam[] towns = new TownParam[mp.getBehavior().number_Of_Towns];
			for(int tcLoc = 0; tcLoc < mp.getBehavior().number_Of_Towns; tcLoc++)
			{
				//need a way to make the town size consistent
				towns[tcLoc] = standardTowns[mp.getRNG().nextInt(8)];
				towns[tcLoc].center = new Point(mp.getRNG().nextInt(mp.getX()), mp.getRNG().nextInt(mp.getY()));
				generateTown(mp, towns[tcLoc]);
			}
			return true;
		}
		public static bool generateTown(Map mp, TownParam t)
		{
			Debug.Log(t.toString());
			//check for inBounds
			if(mp.inBounds(t.center))
			{
				Point[] buildings = new Point[t.size];
				for(int iBuilding = 0; iBuilding < t.size; iBuilding++)
				{
					buildings[iBuilding] = new Point( t.center.getX() + mp.getRNG().nextInt(t.radius * 2) - t.radius, t.center.getY() + mp.getRNG().nextInt(t.radius * 2) - t.radius);
					if(mp.inBounds(buildings[iBuilding]))
						mp.set(buildings[iBuilding], new House());
				}
				//O(n)
				//options to connect
				if(mp.getRNG().nextBool())						//TODO rng in here!
					peerPeer(mp, t, buildings);
				else
					starNet(mp, t, buildings);
				
				/*for(int iBuilding = 0; iBuilding < t.size; iBuilding++)
				{
					if(Point.inBounds(mp.getX(), mp.getY(), buildings[iBuilding]))
						passableTiles[buildings[iBuilding].getX(), buildings[iBuilding].getY()] = new House();
				}*/
				
			}
			return true;
		}
		
		
		/**
		 * this is where road tiles are made
		 */
		public static void peerPeer(Map mp, TownParam t, Point[] buildings)
		{
			Debug.Log("peertopeer");
			Path roadPath = new Path();
			for(int iBuilding = 1; iBuilding < t.size; iBuilding++)
			{
				//create road between these two buildings
				
				//legacy code TODELETE
				//roadPath.push(Pathfinder.findPath(passableTiles, buildings[iBuilding - 1], buildings[iBuilding] , (int)Pathfinder.TileCost.road));
				roadPath.push(mp.findPath(buildings[iBuilding - 1], buildings[iBuilding] , (int)Pathfinder.TileCost.road)); 
				while (!roadPath.isEmpty()) 
				{
					Point curPoint = roadPath.pop();
					if(Point.inBounds(mp.getX(), mp.getY(), curPoint))
					{
						Tile curTile = mp.get(curPoint);
						if(curTile.equals(new River()))
							curTile = new Bridge();
						else if(curTile.equals(new House()) || curTile.equals(new Bridge())){}
							//do nothing
						else
							curTile = new Road();
						mp.set(curPoint, curTile);
						//mp.get(roadPath.peek()) = curTile;
					}
				}
			}
			
		}
		/**
		 * this is where road tiles are made
		 */
		public static void starNet(Map mp, TownParam t, Point[] buildings)
		{
			Debug.Log("starNet");
			Path roadPath = new Path();
			for(int iBuilding = 0; iBuilding < t.size; iBuilding++)
			{
				//create road between these two buildings
				
				//TODELETE
				//roadPath.push(Pathfinder.findPath(passableTiles, t.center, buildings[iBuilding] , (int)Pathfinder.TileCost.road));
				roadPath.push(mp.findPath(t.center, buildings[iBuilding], (int)Pathfinder.TileCost.road));
				while (!roadPath.isEmpty()) 
				{
					Point curPoint = roadPath.pop();
					if(Point.inBounds(mp.getDimensions(), curPoint))
					{
						Tile curTile = mp.get(curPoint);
						if(curTile.equals(new River()))
							curTile = new Bridge();
						else if(curTile.equals(new House()) || curTile.equals(new Bridge())){}
							//do nothing
						else
							curTile = new Road();
						mp.set(curPoint, curTile);
					}
				}
			}
		}
	}
}
 