using general;

namespace maps
{
	
	/**
 * @author Ryan Thorne
 * @dateFinished 10/16/2016
 * @version 1.0
 */
	public class Road : Tile {
		
		public Road() : base(){
			defenseMod = 0;
			attackMod = -1;
			speedMod = 2;
			footMove = 1;
			mountedMove = 1;
			type = '-';
			order = 6;
			groundPass = true;
			airPass = true;
		}
	}
}