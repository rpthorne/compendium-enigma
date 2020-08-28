
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
 * 
 */
namespace maps
{
	public class Map
	{
		private Tile[,] grid;
		public Random rng;
		private int seed;
		public int x;
		public int y;
		private Mapini behavior;
		
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
		public Map(): this(0, 0, 0)
		{}
		public Map(int new_seed, int new_x, int new_y)
		{
			seed = new_seed;
			x = new_x;
			y = new_y;
			Random.seed = seed;
			behavior = null;
			rng = new Random();
			grid = new Tile[x, y];
			for(int i = 0; i < x; i++)
				for(int j = 0; j < y; j++)
					grid[i,j] = new Plains();
			//Debug.Log("i am making a map!");
			//Debug.Log("my dimensions are: " + x + " and " + y);
			//print();
		}
		
		//you should always be using this one
		public Map(int new_seed, Mapini new_ini)
		{
			seed = new_seed;
			x = new_ini.x;
			y = new_ini.y;
			behavior = new_ini;
			Random.seed = seed;
			rng = new Random();
			grid = new Tile[x, y];
			for(int i = 0; i < x; i++)
				for(int j = 0; j < y; j++)
					grid[i,j] = new Plains();
		}
		// diagnostic tool prints the map configuration to the debug console
		public void print()
		{
			String message = "";
			for(int xPos = 0; xPos < y; xPos++)
			{
				message += "[" + grid[xPos, 0].getType();
				for(int yPos = 1; yPos < x; yPos++)
					message += " " + grid[xPos, yPos].getType();
				message += "]";
				Debug.Log(message);
				message = "";
			}
		}		
		//here is the set functions that has been needed in order for other method to modify the map
		public void set(Point p, Tile t)
		{
			if(inBounds(p))
				grid[p.getX(), p.getY()] = t;
		}
		public int getX()
		{
			return x;
		}
		
		public int getY()
		{
			return y;
		}
		public Point getDimensions()
		{
			return new Point(x, y);
		}
		public Random ()
		{
			return rng;
		}
		public Mapini getBehavior()
		{
			return behavior;
		}
		public Tile get(int xPos, int yPos)
		{
			if(inBounds(xPos, yPos))
				return grid[xPos, yPos];
			//throw new OutOfBoundsException();
			Debug.Log("OutOfBoundsException");
			return null;
		}
		public Tile get(Point p)
		{
			if(inBounds(p))
				return grid[p.getX(), p.getY()];
			//throw new OutOfBoundsException();
			Debug.Log("OutOfBoundsException");
			return null;
		}
		/**
		 * 11/29/2016
		 * public getNeighbor
		 * returns the tile in the specified direction NEXT to the location specified
		 * params
		 * @param xPos
		 * xlocation of the original tile
		 * @param yPos
		 * y location of the original tile
		 * @param direction
		 * look for the tile in the specified direction. the direction 
		 * is a member of the public enum directions and should be called accordingly.
		 * @return
		 * the Tile object that is directly adjacent in the direction specified to the location specified.
		 */
		public Tile getNeighbor(int xPos, int yPos, int direction)
		{
			direction %= 8;	//normalize to some direction
			switch(direction)
			{
				case (int)directions.Left:
				xPos--;
				break;
				case (int)directions.Down_Left:
				xPos--;
				yPos--;
				break;
				case (int)directions.Down:
				yPos--;
				break;
				case (int)directions.Down_Right:
				xPos++;
				yPos--;
				break;
				case (int)directions.Right:
				xPos++;
				break;
				case (int)directions.Up_Right:
				xPos++;
				yPos++;
				break;
				case (int)directions.Up:
				yPos++;
				break;
				case (int)directions.Up_Left:
				xPos--;
				yPos++;
				break;
				default:
				return null;
			}
			return get(xPos, yPos);
		}
		
		/**
		 * overloaded version of getNeighbor, refer to the integer version for documentation
		 * instead this version accepts a point instead of an xloc and a yloc.
		 * purely for convinience
		 */
		public Tile getNeighbor(Point location, int direction)
		{
			return getNeighbor(location.getX(), location.getY(), direction);
		}
		
		/**
		 * NeighborDirecctions
		 * 11/30/2016
		 * this method is used to make a more detailed version of hasNeighbor
		 * @param xPos
		 * xlocation of the original tile
		 * @param yPos
		 * y location of the original tile
		 * @params key
		 * an array (or multiple items separated by comma's) that describes which Tile types are questioned to be
		 * adjacent to the original tile
		 * @return
		 * a boolean array of size directions + 1 so that the first element can be accessed
		 * the first and last index of the array. this has helpful benefits to deterrmmining quarter tile shaders
		 * and similar peices
		 */
		public bool[] neighborDirection(int xLoc, int yLoc, params Tile[] key)
		{
			bool[] direction  = new bool[9];
			for(int i = 0; i < 9; i++)
			{
				direction[i] = false;
				Tile temp = getNeighbor(xLoc, yLoc, i % 8); 
				if(temp != null)//need null check in case neighbor is out of bounds
					for(int iKey = 0; iKey < key.Length; iKey++)
						if(temp.equals(key[iKey]))
							direction[i] = true;
			}
			return direction;
		}
		
		/**
		 * overloaded version of neighborDirection, refer to the integer version for documentation
		 * instead this version accepts a point instead of an xloc and a yloc.
		 * purely for convinience
		 */
		public bool[] neighborDirection(Point location, params Tile[] key)
		{
			return neighborDirection(location.getX(), location.getY(), key);
		}
		
		/**
		 * 11/29/2016
		 * has neighbor determines if the specified tile types are adjacent (diagonals included) to the tile at the * location specified.
		 * @param xPos
		 * xlocation of the original tile
		 * @param yPos
		 * y location of the original tile
		 * @params key
		 * an array (or multiple items separated by comma's) that describes which Tile types are questioned to be
		 * adjacent to the original tile
		 * @return
		 * boolean telling whether the tiles in key are adjacent in some way or not at all.
		 */
		public bool hasNeighbor(int xLoc, int yLoc, params Tile[] key)
		{
			for(int direction = 0; direction < 8; direction++)
			{
				Tile curTile = getNeighbor(xLoc, yLoc, direction);
				if(curTile != null)//make sure member is in bounds
					for(int iKey = 0; iKey < key.Length; iKey++)
						if(curTile.equals(key[iKey]))
							return true;
			}
			return false;
		}
		
		public bool inBounds(Point p)
		{
			if(p == null)
				//throw new NullPointerException();
				Debug.Log("NullPointerException");
			return (p.getX() >= 0 && p.getY() >= 0 && p.getX() < x && p.getY() < y);
		}
		public bool inBounds(int xPos, int yPos)
		{
			return (xPos >= 0 && yPos >= 0 && xPos < x && yPos < y);
		}
		//in this class we will store a dictionary to hold our various movement parameters and costs in one place so that they can be reused over and over.
		//TODO set so that these can be tweaked via variables in the editor
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
		
        
        //implementation of OrderCost is notably interface based. 
        //this may have assistance to adjusting how the cost is calculated if we can make elevation a part of the path cost
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
		 * @param start
		 * the starting position of the path
		 * @param end
		 * the ending position of the path
		 * @return
		 * the A* shortest path from start to finish
		 */
		public Path findPath(Point start, Point end)
		{
			return findPath(false, start, end, (int)TileCost.mobile);
		}
		
		public Path findPath(bool alternatePath, Point start, Point end)//idk why i included this method
		{
			return findPath(alternatePath, start, end, (int)TileCost.mobile);
		}
		public Path findPath(Point start, Point end, int tileCost)
		{
			return findPath(false, start, end, tileCost);
		}
		
		
		/**
		 * FindPath function
		 * goal: to find the shortest path from the start location to the end location 
		 * @author Ryan Thorne
		 * credit for A* goes to Peter Hart, Nils Nilsson and Bertram Raphael.
		 * based on the 1959 Dijkstra algorithm from Edsger Dijkstra.
		 * good luck figuring this out if you're not already familiar with the A* pathfinding algorithm
		 * note: this procedure will return null instead of throwing exceptions
		 * @param grid
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
		public Path findPath(bool alternatePath, Point start, Point end, int pathfindingCosts)
		{
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
					if (o.getCost(grid[xIterate, yIterate].getOrder()) == 0)
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
					if (iterator.distanceTraveled + o.getCost(grid[iterator.loc.getX(), iterator.loc.getY()].getOrder()) + Point.distance (iterator.loc, end) < closestNodeDistance) {
						if (!goof1)
							closestNodeDistance = (float)(iterator.distanceTraveled + o.getCost(grid[iterator.loc.getX(), iterator.loc.getY()].getOrder()) + Point.distance (iterator.loc, end));
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
						+ (grid[currentNode.loc.getX () - 1, currentNode.loc.getY ()].getMove(0) * 10);
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
						+ (grid[currentNode.loc.getX () + 1, currentNode.loc.getY ()].getMove(0) * 10);
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
						+ (grid[currentNode.loc.getX (), currentNode.loc.getY () - 1].getMove(0) * 10);
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
						+ (grid[currentNode.loc.getX (), currentNode.loc.getY () + 1].getMove(0) * 10);
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
	}
}