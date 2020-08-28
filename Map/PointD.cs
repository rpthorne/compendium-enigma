using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

using maps;
using general;

namespace general
{
	/**
	 * @author Ryan Thorne
	 * PointD class is a double variant of the point class it
	 * allows for some basic geometry in addition to standard functions of the point class
	 * @version 1.0
	 */
	 public class PointD
	 {
		 double X;
		 double Y;
		 public PointD()
		 {
			 X = 0;
			 Y = 0;
		 }
		 public PointD(double xCoord, double yCoord)
		 {
			 X = xCoord;
			 Y = yCoord;
		 }
		 //mutators
		 public void set(double xCoord, double yCoord)
		 {
			 X = xCoord;
			 Y = yCoord;
		 }
		 public void translate(double xLength,double yLength)
		 {
			 X += xLength;
			 Y += yLength;
		 }
		 public void rotate90(PointD center)
		 {
			 rotate90(center.getX(),center.getY());
			 //rotate around the center parameter
		 }
		 public void rotate90(Point center)
		 {
			 rotate90(center.getX(), center.getY());
			 //rotate around center taking a point input instead of a PointD
		 }
		 /**
		  * this method rotates the point 90 degrees around a point
		  * @param xCenter
		  * the xCoordinate of the center of teh rotation
		  * @param yCenter
		  * the yCoordinate of the center of the rotation
		  * note:
		  * thiss method is significantly faster than specifying a theta
		  * as in the other methods, use this one first!
		  */
		 public void rotate90(double xCenter, double yCenter)
		 {
			//normalize
			 X -= xCenter;
			 Y -= yCenter;
			 //rotate 90 degrees around the origin
			 double temp = X;
			 X = Y * -1.0;
			 Y = temp;
			 //unnormalize
			 X += xCenter;
			 Y += yCenter;
			 //this is the main method for which the rotation is performed
			//rotate around center taking a pair of coords instead of the pointD
		 }
		 
		 //accessors
		 public double getX()
		 {
			 return X;
		 }
		 public double getY()
		 {
			 return Y;
		 }		 
	 }
}
