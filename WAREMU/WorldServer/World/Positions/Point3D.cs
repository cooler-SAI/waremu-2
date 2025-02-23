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
    public class Point3D : Point2D, IPoint3D
    {
        /// <summary>
        /// The Z coord of this point
        /// </summary>
        protected int m_z;

        /// <summary>
        /// Constructs a new 3D point object
        /// </summary>
        public Point3D()
            : base(0, 0)
        {
        }

        /// <summary>
        /// Constructs a new 3D point object
        /// </summary>
        /// <param name="x">The X coord</param>
        /// <param name="y">The Y coord</param>
        /// <param name="z">The Z coord</param>
        public Point3D(int x, int y, int z)
            : base(x, y)
        {
            m_z = z;
        }

        /// <summary>
        /// Constructs a new 3D point object
        /// </summary>
        /// <param name="point">2D point</param>
        /// <param name="z">Z coord</param>
        public Point3D(IPoint2D point, int z)
            : this(point.X, point.Y, z)
        {
        }

        /// <summary>
        /// Constructs a new 3D point object
        /// </summary>
        /// <param name="point">3D point</param>
        public Point3D(IPoint3D point)
            : this(point.X, point.Y, point.Z)
        {
        }

        #region IPoint3D Members

        /// <summary>
        /// Z coord of this point
        /// </summary>
        public virtual int Z
        {
            get { return m_z; }
            set { m_z = value; }
        }

        public override void Clear()
        {
            base.Clear();
            Z = 0;
        }

        #endregion

        /// <summary>
        /// Creates the string representation of this point
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", m_x.ToString(), m_y.ToString(), m_z.ToString());
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
        public virtual int GetDistanceTo(IPoint3D point)
        {
            double dx = (double)X - point.X;
            double dy = (double)Y - point.Y;
            double dz = (double)Z/2 - point.Z/2;

            return (int)(Math.Sqrt(dx * dx + dy * dy + dz * dz) /13.2f);
        }

        /// <summary>
        /// Get the distance to a point (with z-axis adjustment)
        /// </summary>
        /// <param name="point">Target point</param>
        /// <param name="zfactor">Z-axis factor - use values between 0 and 1 to decrease influence of Z-axis</param>
        /// <returns>Adjusted distance to point</returns>
        public virtual int GetDistanceTo(IPoint3D point, double zfactor)
        {
            double dx = (double)X - point.X;
            double dy = (double)Y - point.Y;
            double dz = (double)((Z/2 - point.Z/2) * zfactor);

            return (int)(Math.Sqrt(dx * dx + dy * dy + dz * dz) / 13.2f);
        }

        /// <summary>
        /// Determine if another point is within a given radius
        /// </summary>
        /// <param name="point">Target point</param>
        /// <param name="radius">Radius</param>
        /// <returns>True if the point is within the radius, otherwise false</returns>
        public bool IsWithinRadius(IPoint3D point, int radius)
        {
            return IsWithinRadius(point, radius, false);
        }


        /// <summary>
        /// Determine if another point is within a given radius, optionally ignoring Z values
        /// </summary>
        /// <param name="point">Target point</param>
        /// <param name="radius">Radius</param>
        /// <param name="ignoreZ">ignore Z</param>
        /// <returns>True if the point is within the radius, otherwise false</returns>
        public bool IsWithinRadius(IPoint3D point, int radius, bool ignoreZ)
        {
            int Dist = 0;
            
            if(!ignoreZ)
                Dist = GetDistanceTo(point);
            else
                Dist = GetDistance(point);

            return Dist < radius;
        }
    }
}
