using general;

namespace maps
{
	
	/**
 * @author Ryan Thorne
 * @dateFinished 3/28/2015
 * @version 1.0
 */
	public class Mountain : Tile {
		
		public Mountain() : base(){
			attackMod = 5;
			defenseMod = 3;
			speedMod = 2;
			footMove = 6;
			mountedMove = 15;
			flyMove = 2;
			type = 'A';
			order = 3;
			groundPass = false;
			airPass = true;	
		}
	}
}