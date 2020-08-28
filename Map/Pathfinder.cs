
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

using maps;
using general;
using mobiles;

/*
 * @author Ryan Thorne
 * this whole class is unused.
 * it can be deleted or removed at will
 */
namespace maps
{
	
	public static class Pathfinder
	{
		//in this class we will store a dictionary to hold our various movement parameters and costs in one place so that they can be reused over and over.
		//TODO update pathfinder accordingly
		private static readonly Dictionary<int, OrderCost> tileWeight =  new Dictionary<int, OrderCost>
		{
			{0, new OrderCost(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)}, // sentinel values
			{1, new OrderCost(0, 1, 2, 0, 3, 1, 1, 0, 2, 1)}, //mobile units
			{2, new OrderCost(0, 1, 1, 1, 1, 1, 1, 0, 1, 1)}, //flying units
			{3, new OrderCost(0, 10, 20, 0, 1, 0, 1, 0, 25, 0)}, //river generation
			{4, new OrderCost(0, 3, 9, 30, 30, 1, 1, 0, 6, 1)}, //road generation
			{5, new OrderCost(0, 1, 2, 0, 0, 0, 1, 0, 2, 0)} //house generation
		};
		//add more as needed
		
		private struct OrderCost
		{
			//add more variables as more tiles are added
			//the number in the comment represents the corresponding 
			int tile; 		//0
			int plains; 	//1
			int trees;		//2
			int mountains;	//3
			int river;		//4
			int house;		//5
			int road;		//6
			int castle;		//7
			int hill;		//8
			int bridge;		//9
			// make sure to add values in consistent order
			public OrderCost(int ntile, int nplains, int ntrees, int nmountains, int nriver, int nhouse, int nroad, int ncastle, int nhill, int nbridge)
			{
			tile = ntile;
			plains = nplains;
			trees = ntrees;
			mountains = nmountains;
			river = nriver;
			house = nhouse;
			road = nroad;
			castle = ncastle;
			hill = nhill;
			bridge = nbridge;
			}
			//retruns the selected tile movement cost for the pathfinder
			public int getCost(int selection)
			{
				switch(selection)
				{
					case 1:
					return plains;
					case 2:
					return trees;
					case 3:
					return mountains;
					case 4:
					return river;
					case 5:
					return house;
					case 6:
					return road;
					case 7:
					return castle;
					case 8:
					return hill;
					case 9:
					return bridge;
					default:
					return tile;
				}
			}
			
		}
		
		
		public enum TileCost
		{
			defaultCost = 0,
			mobile = 1,
			flying = 2,
			river = 3,
			road = 4,
			town = 5
		}
		//private Tile[,] grid;
		//private int x, y;
		/**
		 * ANode Class
		 * goal: to represent the map and its characteristics in a Graph/tree fashion
		 * it is also a necesarry function for the A* algorithm which is implemented here
		 * @author Ryan Thorne
		 * credit for A* goes to Peter Hart, Nils Nilsson and Bertram Raphael.
		 * based on the 1959 Dijkstra algorithm from Edsger Dijkstra.
		 */
		private class ANode
		{
			public Point loc;
			public ANode parent;
			public float distanceTraveled;

			public ANode (int x, int y)
			{
				loc = new Point (x, y);
				parent = null;
				distanceTraveled = float.MaxValue;
			}

			public String toString ()
			{
				if (parent == null)
					return loc.toString () + "no parent\n";
				return loc.toString () + " " + parent.loc.toString () + " " + distanceTraveled + "\n";
			}
		}
		/**
		 * FindPath function
		 * goal: to find the shortest path from the start location to the end location 
		 * @author Ryan Thorne
		 * credit for A* goes to Peter Hart, Nils Nilsson and Bertram Raphael.
		 * based on the 1959 Dijkstra algorithm from Edsger Dijkstra.
		 * good luck figuring this out if you're not already familiar with the A* pathfinding algorithm
		 * note: this procedure will return null instead of throwing exceptions	
		 * @param passableTiles
		 * this is the map upon which you are generating over
		 * @param rng
		 * the random number generator as used in prior code.
		 * @param start
		 * the starting position of the path
		 * @param end
		 * the ending position of the path
		 * @return
		 * the A* shortest path from start to finish
		 */
		public static Path findPath(Tile[,] passableTiles, Point start, Point end)
		{
			return findPath(passableTiles, false, start, end, (int)TileCost.mobile);
		}
		
		public static Path findPath(Tile[,] passableTiles, bool alternatePath, Point start, Point end)
		{
			return findPath(passableTiles, alternatePath, start, end, (int)TileCost.mobile);
		}
		public static Path findPath(Tile[,] passableTiles, Point start, Point end, int tileCost)
		{
			return findPath(passableTiles, false, start, end, tileCost);
		}
		
		
		/**
		 * FindPath function
		 * goal: to find the shortest path from the start location to the end location 
		 * @author Ryan Thorne
		 * credit for A* goes to Peter Hart, Nils Nilsson and Bertram Raphael.
		 * based on the 1959 Dijkstra algorithm from Edsger Dijkstra.
		 * good luck figuring this out if you're not already familiar with the A* pathfinding algorithm
		 * note: this procedure will return null instead of throwing exceptions
		 * @param passableTiles
		 * this is the map upon which you are generating over
		 * @param alternatePath
		 * determines whether or not the alternate pathing will be used
		 * @param start
		 * the starting position of the path
		 * @param end
		 * the ending position of the path
		 * @param pathfindingCosts
		 * choose this based on the dictionary definitions to determine what each tile costs
		 * @return
		 * the A* shortest path from start to finish
		 */
		public static Path findPath(Tile[,] passableTiles, bool alternatePath, Point start, Point end, int pathfindingCosts)
		{
			int x = passableTiles.GetLength(0);
			int y = passableTiles.GetLength(1);
			OrderCost o = tileWeight[pathfindingCosts];
			//haha i have no error checking sucks to be you!
			//generate randomization values
			bool goof1;// goof2;
			if (alternatePath) {
				goof1 = true;
				//goof2 = rng.nextbool(); TODO find a place for a second goof in the program
			} else {
				goof1 = false;
				//goof2 = false;
			}
			if (start.equals (null) || end.equals (null))
				return null;
			if (start.getX () < 0 || start.getX () >= x)
				return null;
			if (start.getY () < 0 || start.getY () >= y)
				return null;
			if (end.getX () < 0 || end.getX () >= x)
				return null;
			if (end.getY () < 0 || end.getY () >= y)
				return null;
			LinkedList<ANode> openSet = new LinkedList<ANode> ();
			LinkedList<ANode> closedSet = new LinkedList<ANode> ();
			ANode[, ] map = new ANode[x, y];
			//add mountains to the closed set of tiles not considered for paths
			//also create the mape of ANodes that allow for parenting
			for (int xIterate = 0; xIterate < x; xIterate++)
				for (int yIterate = 0; yIterate < y; yIterate++)
				{
					map[xIterate, yIterate] = new ANode (xIterate, yIterate);
					//we need to determine which items are allowed to pass by//TODO
					//if the movecost to travel through this tile is 0, we cannot pass
					if (o.getCost(passableTiles[xIterate, yIterate].getOrder()) == 0)
						closedSet.AddLast (map [xIterate, yIterate]);
				}
			if (closedSet.Contains (map [start.getX (), start.getY ()]))
				return null;
			if (closedSet.Contains (map [end.getX(), end.getY()]))
				return null;
			
			//set initial values for the pathfinding
			openSet.AddLast (map [start.getX (), start.getY ()]);
			map [start.getX (), start.getY ()].distanceTraveled = 0;

			//start pathfinding
			while (openSet.First != null) {
				//determine next node to check
				ANode currentNode = null;
				float closestNodeDistance = float.MaxValue;
				foreach (ANode iterator in openSet) {
					if (iterator.distanceTraveled + o.getCost(passableTiles[iterator.loc.getX(), iterator.loc.getY()].getOrder()) + Point.distance (iterator.loc, end) < closestNodeDistance) {
						if (!goof1)
							closestNodeDistance = (float)(iterator.distanceTraveled + o.getCost(passableTiles[iterator.loc.getX(), iterator.loc.getY()].getOrder()) + Point.distance (iterator.loc, end));
						currentNode = iterator;
					}
				}			
				//here is where we check to see if we have reached the end
				if (currentNode.loc.equals (end)) 
				{
					//since we are at the end, we must build our path to pass back
					Path p = new Path ();
					while (currentNode.parent != null) {
						p.push (currentNode.loc);
						currentNode = currentNode.parent;
					}
					return p;
				}
			
				closedSet.AddLast (currentNode);
				openSet.Remove (currentNode);
				//find neighbors and set their parents
				//this is the relaxation step
				//find first neighbor (one to the right)
				if (currentNode.loc.getX () != 0)
				if (!closedSet.Contains (map [currentNode.loc.getX () - 1, currentNode.loc.getY ()])) 
				{
					float newDistance = currentNode.distanceTraveled
						+ (passableTiles[currentNode.loc.getX () - 1, currentNode.loc.getY ()].getMove(0) * 10);
					if (!openSet.Contains (map [currentNode.loc.getX () - 1, currentNode.loc.getY ()]) ||
						newDistance < map [currentNode.loc.getX () - 1, currentNode.loc.getY ()].distanceTraveled) 
					{
						//set neighbors parent as currentNode
						map [currentNode.loc.getX () - 1, currentNode.loc.getY ()].distanceTraveled = newDistance;
						map [currentNode.loc.getX () - 1, currentNode.loc.getY ()].parent = currentNode;
						if (!openSet.Contains (map [currentNode.loc.getX () - 1, currentNode.loc.getY ()]))
							openSet.AddLast (map [currentNode.loc.getX () - 1, currentNode.loc.getY ()]);
					}
				}
				//find second neighbor (one to the left)
				if (currentNode.loc.getX () + 1 != x)
				if (!closedSet.Contains (map [currentNode.loc.getX () + 1, currentNode.loc.getY ()])) 
				{
					float newDistance = currentNode.distanceTraveled
						+ (passableTiles[currentNode.loc.getX () + 1, currentNode.loc.getY ()].getMove(0) * 10);
					if (!openSet.Contains (map [currentNode.loc.getX () + 1, currentNode.loc.getY ()]) ||
						newDistance < map [currentNode.loc.getX () + 1, currentNode.loc.getY ()].distanceTraveled) 
					{
						//set neighbors parent as currentNode
						map [currentNode.loc.getX () + 1, currentNode.loc.getY ()].distanceTraveled = newDistance;
						map [currentNode.loc.getX () + 1, currentNode.loc.getY ()].parent = currentNode;
						if (!openSet.Contains (map [currentNode.loc.getX () + 1, currentNode.loc.getY ()]))
							openSet.AddLast (map [currentNode.loc.getX () + 1, currentNode.loc.getY ()]);
					}
				}
				//find third neighbor (one above)
				if (currentNode.loc.getY () != 0)
				if (!closedSet.Contains (map [currentNode.loc.getX (), currentNode.loc.getY () - 1])) 
				{
					float newDistance = currentNode.distanceTraveled
						+ (passableTiles[currentNode.loc.getX (), currentNode.loc.getY () - 1].getMove(0) * 10);
					if (!openSet.Contains (map [currentNode.loc.getX (), currentNode.loc.getY () - 1]) ||
						newDistance < map [currentNode.loc.getX (), currentNode.loc.getY () - 1].distanceTraveled) 
					{
						//set neighbors parent as currentNode
						map [currentNode.loc.getX (), currentNode.loc.getY () - 1].distanceTraveled = newDistance;
						map [currentNode.loc.getX (), currentNode.loc.getY () - 1].parent = currentNode;
						if (!openSet.Contains (map [currentNode.loc.getX (), currentNode.loc.getY () - 1]))
							openSet.AddLast (map [currentNode.loc.getX (), currentNode.loc.getY () - 1]);
					}
				}
				//find fourth neighbor (one below)
				if (currentNode.loc.getY () + 1 != y)
				if (!closedSet.Contains (map [currentNode.loc.getX (), currentNode.loc.getY () + 1])) 
				{
					float newDistance = currentNode.distanceTraveled
						+ (passableTiles[currentNode.loc.getX (), currentNode.loc.getY () + 1].getMove(0) * 10);
					if (!openSet.Contains (map [currentNode.loc.getX (), currentNode.loc.getY () + 1]) ||
						newDistance < map [currentNode.loc.getX (), currentNode.loc.getY () + 1].distanceTraveled) 
					{
						//set neighbors parent as currentNode
						map [currentNode.loc.getX (), currentNode.loc.getY () + 1].distanceTraveled = newDistance;
						map [currentNode.loc.getX (), currentNode.loc.getY () + 1].parent = currentNode;
						if (!openSet.Contains (map [currentNode.loc.getX (), currentNode.loc.getY () + 1]))
							openSet.AddLast (map [currentNode.loc.getX (), currentNode.loc.getY () + 1]);
					}
				}
			
			}
			//there is no path:. return null as diagnostics information or as indication that tile cannot reach other tile w/o flying
			return null;
		}
		//constructers
		/*public Pathfinder(Tile[,] newGrid)
		{
			grid = newGrid;
			x = grid.GetLength(0);
			y = grid.GetLength(1);
		}*/
	}
}