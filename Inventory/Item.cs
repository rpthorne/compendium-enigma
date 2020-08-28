
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace items
{
	abstract class Item
	{
		//instance fields
		SpriteRenderer inventoryImage;
		String name;
		char type;
		
		public abstract bool equals(Item i);
	}

}