
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

using maps;
using general;
using mobiles;

/**
 * @author Ryan Thorne
 * @date 2/21/2015
 */
  
  namespace general
  {
	  public class Group //implements some groupable interface
	  {
		  Point location;
		  int x, y;//note x is the row length, y is the column length. i think
		  public Group()
		  {
		  }
		  public Group(Point p) : this(p, 0, 0)
		  {
		  }
		  public Group(Point p, int newX, int newY)
		  {
			  location = p;
			  x = newX;
			  y = newY;
		  }
		  //getters
		  public int getX()
		  {
			  return x;
		  }
		  public int getY()
		  {
			  return y;
		  }
		  public Point getLoc()
		  {
			  //note: it is generally custom to use the upper left most point as the pivot
			  return location;
		  }
		  
		  //setters
		  public void setSize(int newX, int newY)
		  {
			x = newX;
			y = newY;  
		  }
	  }
  }