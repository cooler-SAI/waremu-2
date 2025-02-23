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

using Common;
using FrameWork;
namespace WorldServer
{
    public class PQuestObject : Object
    {
        public PQuest_Info Info;

        public PQuestObject()
            : base()
        {

        }

        public PQuestObject(PQuest_Info Info)
            : this()
        {
            this.Info = Info;
            Name = Info.Name;
        }

        public override void OnLoad()
        {
            X = (int)Info.PinX;
            Y = (int)Info.PinY;
            Z = 16384;
            SetOffset(Info.OffX, Info.OffY);
            Region.UpdateRange(this);

            base.OnLoad();
        }

        public override void SendMeTo(Player Plr)
        {
            // TODO
            // Send Quest Info && Current Stage && Current Players
        }
    }
}
