using System;

namespace general
{

/**
 * @author Ryan Thorne
 * simply a queue
 * @version 1.0
 */
	public class Path {
		private int size;
		private Node head;
		
		public Path()
		{
			head = null;
			size = 0;
		}
		public void push(Point p)
		{
			head = new Node(p, head);
			size++;    
		}
		/**
	 * adds another path to the existing one
	 * @param p
	 * the path to add
	 * note this method will add the parameter path TO THE END of the queue
	 */
		public void push(Path p)
		{
			if(p != null)
				while(!p.isEmpty())
					this.push(p.pop());
		}
		public Point pop()
		{
			Node temp = head;
			head = head.prev;
			return temp.loc;
		}
		public Point peek()
		{
			return head.loc;
		}
		public bool isEmpty()
		{
			if(head == null)
				return true;
			return false;
		}
		public int getSize()
		{
			return size;
		}
		private class Node
		{
			public Point loc;
			public Node prev;
			public Node(Point p, Node n)
			{
				prev = n;
				loc = p;
			}
		}
		public String toString()
		{
			Path temp = new Path();
			String ret = "[";
			while(!this.isEmpty())
			{
				temp.push(this.pop());
				ret = ret + temp.peek().toString() + "\n";
			}
			while(!temp.isEmpty())
			{
				this.push(temp.pop());
			}
			return ret + " ]";
			
		}
	}
}

