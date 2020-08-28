using general;

namespace maps
{
	
	/**
 * @author Ryan Thorne
 * @dateFinished 3/28/2015
 * @version 1.0
 */
	public class Hill : Tile {
		
		public Hill() : base(){
			defenseMod = 2;
			speedMod = -1;
			footMove = 4;
			mountedMove = 7;
			type = 'n';
			order = 8;
			groundPass = true;
			airPass = true;	
		}
	}
}