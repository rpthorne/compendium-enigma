using general;

namespace maps
{
	
	/**
 * @author Ryan Thorne
 * @dateFinished 3/28/2015
 * @version 1.0
 */
	public class House : Tile {
		
		public House() : base(){
			defenseMod = 6;
			attackMod = 3;
			speedMod = -1;
			type = 'H';
			order = 5;
			groundPass = true;
			airPass = true;
		}
	}
}