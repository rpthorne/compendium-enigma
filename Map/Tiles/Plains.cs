using general;

namespace maps
{
	
	/**
 * @author Ryan Thorne
 * @dateFinished 3/28/2015
 * @version 1.0
 */
	public class Plains : Tile {
		
		public Plains() : base(){
			type = '*';
			order = 1;
			groundPass = true;
			airPass = true;
		}
	}
}