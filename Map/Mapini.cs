

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
	public class Mapini : MonoBehaviour
	{
		//wtf?
		public Random rng;
		//undetermined as to what this will do
		public int seed;
		//not for you
		private readonly Mobile SENTINEL = new Mobile ();
		public int x; //dimension one
		public int y; //dimension two
		//defines the likelyhood of a tree occuring somewhere on the map. with advanced methods of tree generation this is an approximation only
		public int tree_Coverage_Percentage;
		//defines which type of trees are generated.
		public int tree_Generation_Form;
		//defines which type of mountains are generated.
		public int mtnType;
		//defines the likelyhood of a mountain occuring somewhere on the map. with advanced methods of mountain generation this is an approximation only
		public int mtnCoverage;
		//defines the number of points to which a constructed river will snap to when finding a path through the map
		public int free_River_Points;
		//selects which form of smoothing the river generator will use. useless at the moment
		public int U_smoothing_Points;
		//tells the river to use 'alternate' snaking pathing. 0 for none, 1 for some, and 2 for all
		public int alternate_Path;
		//tells the machine to attempt this number of river attempts when constructing a river before declaring a failure and not constructing a river
		public int ReasonableNumRiverAttempts;
		//does nothing
		public int default_Map_Coverage_Type;
		//sets the number of towns that will naturally spawn on the map
		public int number_Of_Towns;
		//shows the actual percentage of trees generated on the map
		public int treeCount;
		void Awake ()
		{
			//inivals = GetComponent<mapini>().mapini(); // do not use! associates mapini with incorrect value!
			
			if(seed == -1)
			{
				Random rngtmp = new Random();
				seed = rng.nextInt(20000);
				rng = rngtmp;
			}
		}
	}
	
}