﻿/*
 * Copyright (C) 2011 APS
 *	http://AllPrivateServer.com
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldServer
{
	public class Point2D : IPoint2D
	{
		[Obsolete("Use HEADING_TO_RADIAN and RADIAN_TO_HEADING")] public const double HEADING_CONST =
			651.89864690440329530934789477382;

		/// <summary>
		/// The factor to convert a heading value to radians
		/// </summary>
		/// <remarks>
		/// Heading to degrees = heading * (360 / 4096)
		/// Degrees to radians = degrees * (PI / 180)
		/// </remarks>
		public const double HEADING_TO_RADIAN = (360.0/4096.0)*(Math.PI/180.0);

		/// <summary>
		/// The factor to convert radians to a heading value
		/// </summary>
		/// <remarks>
		/// Radians to degrees = radian * (180 / PI)
		/// Degrees to heading = degrees * (4096 / 360)
		/// </remarks>
		public const double RADIAN_TO_HEADING = (180.0/Math.PI)*(4096.0/360.0);

		/// <summary>
		/// The X coord of this point
		/// </summary>
		protected int m_x;

		/// <summary>
		/// The Y coord of this point
		/// </summary>
		protected int m_y;

		/// <summary>
		/// Constructs a new 2D point object
		/// </summary>
		public Point2D() : this(0, 0)
		{
		}

		/// <summary>
		/// Constructs a new 2D point object
		/// </summary>
		/// <param name="x">The X coord</param>
		/// <param name="y">The Y coord</param>
		public Point2D(int x, int y)
		{
			m_x = x;
			m_y = y;
		}

		/// <summary>
		/// Constructs a new 2D point object
		/// </summary>
		/// <param name="point">The 2D point</param>
		public Point2D(IPoint2D point) : this(point.X, point.Y)
		{
		}

		#region IPoint2D Members

		/// <summary>
		/// X coord of this point
		/// </summary>
		public virtual int X
		{
			get { return m_x; }
			set { m_x = value; }
		}

		/// <summary>
		/// Y coord of this point
		/// </summary>
		public virtual int Y
		{
			get { return m_y; }
			set { m_y = value; }
		}

		// Coordinate calculation functions in DOL are standard trigonometric functions, but
		// with some adjustments to account for the different coordinate system that DOL uses
		// compared to the standard Cartesian coordinates used in trigonometry.
		//
		// Cartesian grid:
		//        90
		//         |
		// 180 --------- 0
		//         |
		//        270
		//        
		// DOL Heading grid:
		//       2048
		//         |
		// 1024 ------- 3072
		//         |
		//         0
		// 
		// The Cartesian grid is 0 at the right side of the X-axis and increases counter-clockwise.
		// The DOL Heading grid is 0 at the bottom of the Y-axis and increases clockwise.
		// General trigonometry and the System.Math library use the Cartesian grid.

		/// <summary>
		/// Get the heading to a point
		/// </summary>
		/// <param name="point">Target point</param>
		/// <returns>Heading to target point</returns>
		public ushort GetHeading(IPoint2D point)
		{
			float dx = point.X - X;
			float dy = point.Y - Y;

			double heading = Math.Atan2(-dx, dy)*RADIAN_TO_HEADING;

			if (heading < 0)
				heading += 4096;

			return (ushort) heading;
		}

		/// <summary>
		/// Get the point at the given heading and distance
		/// </summary>
		/// <param name="gameHeading">DOL Heading</param>
		/// <param name="distance">Distance to point</param>
		/// <returns>Point at the given heading and distance</returns>
		public Point2D GetPointFromHeading(ushort heading, int distance)
		{
            distance = (int)((float)distance*13.2f);
			double angle = heading*HEADING_TO_RADIAN;
			double targetX = X - (Math.Sin(angle)*distance);
			double targetY = Y + (Math.Cos(angle)*distance);

			var point = new Point2D();

			if (targetX > 0)
				point.X = (int) targetX;
			else
				point.X = 0;

			if (targetY > 0)
				point.Y = (int) targetY;
			else
				point.Y = 0;

			return point;
		}

        /// <summary>
        /// Get the point at the given heading and distance back
        /// </summary>
        /// <param name="gameHeading">DOL Heading</param>
        /// <param name="distance">Distance to point</param>
        /// <returns>Point at the given heading and distance</returns>
        public Point2D GetBackPoint(ushort heading,int distance)
        {
            Point2D Front = GetPointFromHeading(heading, distance);
            Point2D Pos = Front.GetPointFromHeading(Front.GetHeading(this), distance*2);

            return Pos;
        }

		/// <summary>
		/// Get the distance to a point
		/// </summary>
		/// <remarks>
		/// If you don't actually need the distance value, it is faster
		/// to use IsWithinRadius (since it avoids the square root calculation)
		/// </remarks>
		/// <param name="point">Target point</param>
		/// <returns>Distance to point</returns>
		public int GetDistance(IPoint2D point)
		{
			double dx = (double) X - point.X;
			double dy = (double) Y - point.Y;
            double Range = Math.Sqrt(dx * dx + dy * dy) / 13.2F;
            if (Range < 15)
                Range /= 2;

            return (int)(Range);
		}

		public virtual void Clear()
		{
			X = 0;
			Y = 0;
		}

		#endregion

		/// <summary>
		/// Creates the string representation of this point
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("({0}, {1})", m_x.ToString(), m_y.ToString());
		}

		/// <summary>
		/// Determine if another point is within a given radius
		/// </summary>
		/// <param name="point">Target point</param>
		/// <param name="radius">Radius</param>
		/// <returns>True if the point is within the radius, otherwise false</returns>
		public bool IsWithinRadius(IPoint2D point, int radius)
		{
			if (radius > ushort.MaxValue)
			{
				return GetDistance(point) <= radius;
			}

			uint rsquared = (uint) radius*(uint) radius;

			int dx = X - point.X;

			long dist = ((long) dx)*dx;

			if (dist > rsquared)
			{
				return false;
			}

			int dy = Y - point.Y;

			dist += ((long) dy)*dy;

			if (dist > rsquared)
			{
				return false;
			}

			return true;
		}
	}
}
