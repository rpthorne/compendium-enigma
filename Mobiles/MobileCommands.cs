
using System;
using general;


namespace mobiles
{
	public interface MobileCommands
	{
		int getTeam();
		Point getPosition();
		int getAttack();
		int getDefense();
		int getSpeed();
		int getHP();
		int getMove();
		int getLevel();
		int getXp();
		byte getType();
		String getName();
	}
}

