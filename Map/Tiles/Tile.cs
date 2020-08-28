using general;
using mobiles;
using System;

namespace maps
{
	
	/**
 * @author Ryan Thorne
 * @dateFinished 3/28/2015
 * @version 1.0
 * this is the default representation of any tile it MUST NOT be used as a tile in the game
 */
	public class Tile {
		public Point loc; //redundant, not yet sure if needed
		
		public Mobile m; // this is where the unit located here is stored
		
		protected short defenseMod;
		protected short attackMod;
		protected short speedMod;
		protected short footMove;
		protected short mountedMove;
		protected short flyMove;	//also unsure query flying units?
		protected char type;	//for non-graphical visualizations, determines which type of tile it is: mountain, river etc: also used to determine sprite order on things like roads(bridges, endings) houses(estates)
		protected short order;	//this is used to determine what the order of each tiles position is for the pathfinder DO NOT MODIFY THIS VALUE!
		protected bool groundPass;
		protected bool airPass;
		public Tile()
		{
			m = null;
			defenseMod = 0;
			attackMod = 0;
			speedMod = 0;
			footMove = 1;
			mountedMove = 1;
			flyMove = 1;
			type = 'Q';
			groundPass = false;
			airPass = false;
			order = 0;
		}
		public short getMove(short type)
		{
			if(type == 0)
				return footMove;
			if(type == 1)
				return mountedMove;
			return flyMove;
		}
		public short getAtk()
		{
			return attackMod;
		}
		public short getDef()
		{
			return defenseMod;
		}
		public short getSpd()
		{
			return speedMod;
		}
		public char getType()
		{
			return type;
		}

		public bool getGPass()
		{
			return groundPass;
		}
		public bool getAPass()
		{
			return airPass;
		}
		public String toString()
		{
			return type + "";
		}
		public int getOrder()
		{
			return order;
		}
		public bool equals(Tile t)
		{
			if(t != null)
				if(t.getType() == this.type)
					return true;
			else if (this == null)
				return true;
			return false;
		}
	}

}

