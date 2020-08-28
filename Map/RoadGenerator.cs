
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
 * @date 2/21/2015
 */
  

namespace maps
{
	
	class RoadGenerator
	{
		//define road options
		//can be generated as having a magnetic attraction to rivers
		//rivers have a high cost to cross, will exhaust other options before building a bridge
		//need to include a crossingRiver flag in order to tell generator to put a mountain there.
		//first pass though will follow a strict magnetism towards rivers
		
		//come back to this once villages have been created
		/*
		public bool[,] generateRoads(Tile[,] passableTiles)
		{
			
		}
		
		//createRoad
		public bool[,] createRoad(Tile[,] passableTiles, Point start, Point end)
		{
			
		}*/
		
		
		//	Pathfinder.findPath(passableTiles, start, end, (int)Pathfinder.TileCost.road)
				
		
		
	}
}	