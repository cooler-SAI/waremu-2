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
using System.Drawing;
using System.IO;

using Common;

namespace WorldServer
{
    public class HeighMapInfo
    {
        public HeighMapInfo(int ZoneID)
        {
            this.ZoneID = ZoneID;
        }

        public int ZoneID;
        public Bitmap Offset;
        public Bitmap Terrain;

        private bool Loaded = false;

        public int GetHeight(int PinX, int PinY)
        {
            Load();

            if (Offset == null || Terrain == null)
                return -1;

            PinX = (int)((float)PinX / 64f);
            PinY = (int)((float)PinY / 64f);

            if (PinX < 0 || PinX > Offset.Width || PinX > Terrain.Width)
                return -1;

            if (PinY < 0 || PinY > Offset.Height || PinY > Terrain.Height)
                return -1;

            float fZValue = 0;

            try
            {
                {
                    Color iColor = Offset.GetPixel(PinX, PinY);
                    fZValue += iColor.R * 31; // 0 -> 30
                }

                {
                    Color iColor = Terrain.GetPixel(PinX, PinY);
                    fZValue += iColor.R;
                }
            }
            catch (Exception e)
            {
                FrameWork.Log.Error("HeightMap", e.ToString());
            }

            fZValue *= 16;

            return (int)fZValue-30;
        }

        public void Load()
        {
            if (Loaded)
                return;

            Loaded = true;

            try
            {
                Offset = new Bitmap(Program.Config.ZoneFolder + "zone" + String.Format("{0:000}", ZoneID) + "/offset.png"); // /zones/zone003/offset.png
                Terrain = new Bitmap(Program.Config.ZoneFolder + "zone" + String.Format("{0:000}", ZoneID) + "/terrain.png"); // /zones/zone003/offset.png
            }
            catch(Exception e)
            {
                FrameWork.Log.Error("HeightMap", "[" + ZoneID + "] Invalid HeightMap \n " + e.ToString());
            }
        }
    }

    static public class HeightMapMgr
    {
        static public Dictionary<int, HeighMapInfo> Heights = new Dictionary<int, HeighMapInfo>();

        static public int GetHeight(int ZoneID, int PinX, int PinY)
        {
            HeighMapInfo Info;
            if (!Heights.TryGetValue(ZoneID, out Info))
            {
                FrameWork.Log.Success("HeightMap", "["+ZoneID+"] Loading Height Map..");
                Info = new HeighMapInfo(ZoneID);
                Heights.Add(ZoneID, Info);
            }

            return Info.GetHeight(PinX, PinY);
        }
    }
}
