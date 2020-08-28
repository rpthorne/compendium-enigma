
using System;
using general;

namespace mobiles
{
	public class Mobile : MobileCommands
	{
		public const byte GROUND_UNIT = 0;// the number code for a ground-type, foot unit
		public const byte MOUNTED_UNIT = 1;// the number code for a ground-type, mounted unit
		public const byte FLYING_UNIT = 2;// the number code for a flying-type unit
		//values that change
		protected Point loc; //not sure if needed
		protected int attack;
		protected int defense;
		protected int speed;
		protected int hp;
		protected int move;
		protected int level;
		protected int exp;
		protected int team;
		
		//values that don't change
		protected byte type;
		protected String className;
		
		//values that reset per turn
		protected int moveLeft;
		
		//constructors
		public Mobile()
		{
			loc = new Point(-1,-1); //spawns mob outside map;
			className = "";
			type = GROUND_UNIT;
			
			team = 0; // assume that the mobile is on no ones team
			attack = 0;
			defense = 0;
			speed = 50;
			hp = 1;
			move = 0;
			level = 1;
			exp = 0;
			
			// sets to new team
			reset();
		}
		
		
		//resets the mobile's moves for use in a new turn
		public void reset() {
			moveLeft = move;
		}
		
		
		//overridden methods from the MobileCommands Interface

		public Point getPosition() {
			return loc;
		}

		public int getAttack() {
			return attack;
		}
		
		public int getDefense() {
			return defense;
		}
		
		public int getSpeed() {
			return speed;
		}
		
		public int getHP() {
			return hp;
		}
		
		public int getMove() {
			return move;
		}
		
		public int getLevel() {
			return level;
		}
		
		public int getXp() {
			return exp;
		}
		
		public byte getType() {
			return type;
		}
		
		public String getName() {
			return className;
		}
		

		public int getTeam() {
			return team;
		}
	}
}

