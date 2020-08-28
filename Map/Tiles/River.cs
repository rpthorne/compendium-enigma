using general;

namespace maps
{

	/**
 * @author Ryan Thorne
 * @dateFinished 3/28/2015
 * @version 1.0
 */
	public class River : Tile 
	{
		
		public River() : base(){
			attackMod = -4;
			speedMod = -3;
			footMove = 4;
			mountedMove = 8;
			type = '_';
			order = 4;
			groundPass = true;
			airPass = true;
		}
	}
}
