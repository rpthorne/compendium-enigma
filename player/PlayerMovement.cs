using UnityEngine;
using System.Collections;

//ryan Thorne
//camera controls::
/**
 * @author Ryan Thorne
 * @designation, camera controls
 * @changelog
 * version 0.1
 * goals: separate usre input from movement methods.
 */


using maps;
using general;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

namespace player
{


	public class PlayerMovement : MonoBehaviour {

		private int stall;
		private GameObject cursor;
		
		//number of ticks before 
		public int REPEAT_SCROLL_DELAY = 30;
		
		public float DIRECTIONAL_DEADZONE;
		
		private Point curPlayerDirectionalInput;
		private Point prevPlayerDirectionalInput;
		
		private int curCursorPos;
		
		// Use this for initialization
		void Start () {
			stall = REPEAT_SCROLL_DELAY;
			curCursorPos = 1;
			/*
			 *	0	left
			 *	1	right
			 *	  0	upper
			 *	  1	lower 
			*/
			
			//cursor = gameObject.Find("UICursor");
		}
		// Update is called once per frame
		void Update () {
			prevPlayerDirectionalInput = curPlayerDirectionalInput;
			curPlayerDirectionalInput = getInput();
			if(curPlayerDirectionalInput.equals(prevPlayerDirectionalInput))
				stall++;
			else
				stall = 0;
			
			//move
			if(stall == 0 || stall > REPEAT_SCROLL_DELAY)
			{
				int xdir = curPlayerDirectionalInput.getX();
				int ydir = curPlayerDirectionalInput.getY();
				if(xdir == 1)
					curCursorPos = curCursorPos | 2;
				if(xdir == -1)
					curCursorPos = curCursorPos & 2;
				if(ydir == -1)
					curCursorPos = curCursorPos | 1;
				if(ydir == 1)
					curCursorPos = curCursorPos & 1;
				transform.Translate(xdir, ydir, 0);
			}
			
			//now evaluate where the UICursor goes, also determine if screen moves 
			
		}
		
		Point getInput()
		{
			int x, y;
			float vertical = Input.GetAxis("Vertical");
			if(vertical >= DIRECTIONAL_DEADZONE)
				y = 1;
			else if(vertical <= -DIRECTIONAL_DEADZONE)
				y = -1;
			else 
				y = 0;
			float horizontal = Input.GetAxis("Horizontal");
			if(horizontal >= DIRECTIONAL_DEADZONE)
				x = 1;
			else if(horizontal <= -DIRECTIONAL_DEADZONE)
				x = -1;
			else
				x = 0;
			return new Point(x, y);
		}
		
		
		
		//IEnumerator updatePosition(Vector3 end)
		//{
			//finding distance due to computational cheapness over frames 
			//tentative to change
			//float sqrdist = (cursor.transform - end).sqrMagnitude();
			
			
			
		//}
		
		
		
	}
}