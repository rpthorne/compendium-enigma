using System;

namespace general
{
	/**
	 * @author Ryan Thorne
	 * @dateFinished 3/28/2015
	 * @changelog
	 * @version 1.0
	 * @version 1.1
	 * added overloaded method to equals, new default constructor and created an object specific version of distance
	 */
	 public class Point
	 {
		 private int X;
		 private int Y;
		 public Point() : this(0, 0) {}
		 public Point(int newX, int newY)
		 {
			 X = newX;
			 Y = newY;
		 }
		 public int getX()
		 {
			 return X;
		 }
		 public int getY()
		 {
			 return Y;
		 }
		 public bool equals(Point p)
		 {
			 if(p == null)
				 return false;
			 if(this.getX() == p.getX() && this.getY() == p.getY())
				 return true;
			 return false;
		 }
		 public bool equals(int x1, int y1)
		 {
			 if(x1 == this.getX() && y1 == this.getY())
				 return true;
			 return false;
		 }
		 public String toString()
		 {
			 return "(" + X + ", " + Y + ")";
		 }
		 public double distanceTo(Point p)
		 {
			 return distance(this, p);
		 }
		 public static double distance(Point p1, Point p2)
		 {
			 return Math.Sqrt(Math.Pow(p2.getX() - p1.getX(), 2) + Math.Pow(p2.getY() - p1.getY(), 2));
		 }
		 /**
		  * this method tests whether a Point p is within the rectangle formed 
		  * when the bound1 and bound2 are treated as verteces of a rektangle.
		  * inclusive on the lower bound, exclusive on the upper bound
		  */
		 public static bool inBounds(Point bound1, Point bound2, Point p)
		 {
			 return (p.getX() >= bound1.getX() && p.getY() >= bound1.getY() && p.getX() < bound2.getY() && p.getY() < bound2.getY());
		 }
		 public static bool inBounds(Point bound, Point p)
		 {
			 return (p.getX() >= 0 && p.getY() >= 0 && p.getX() < bound.getX() && p.getY() < bound.getY());
		 }
		 public static bool inBounds(int xbound, int ybound, Point p)
		 {
			 return (p.getX() >= 0 && p.getY() >= 0 && p.getX() < xbound && p.getY() < ybound);
		 }
		 public static bool onEdge(Point bound, Point p)
		 {
			 return onEdge(bound.getX(), bound.getY(), p.getX(), p.getY());
		 }
		 public static bool onEdge(int xbound, int ybound, Point p)
		 {
			 return onEdge(xbound, ybound, p.getX(), p.getY());
		 }
		 public static bool onEdge(int xbound, int ybound, int xLoc, int yLoc)
		 {
			 return (xLoc == 0 || yLoc == 0 || xLoc == xbound - 1 || yLoc == ybound - 1);
		 }
		 public static bool inBoundsRadial(Point center, int radius, Point p)
		 {
			 return  (distance(center, p) < radius);
		 }
	 }
}