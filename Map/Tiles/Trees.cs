using general;

namespace maps
{
	
	/**
 * @author Ryan Thorne
 * @dateFinished 3/28/2015
 * @version 1.0
 */
	public class Trees : Tile {
		
		public Trees() : base(){
			defenseMod = 4;
			attackMod = 1;
			speedMod = -2;
			footMove = 2;
			mountedMove = 2;
			type = '^';
			order = 2;
			groundPass = true;
			airPass = true;
		}
	}
}