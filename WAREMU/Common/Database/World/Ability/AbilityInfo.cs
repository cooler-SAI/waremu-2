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

using FrameWork;

namespace Common
{
    [DataTable(PreCache = false, TableName = "Ability_Infos", DatabaseName = "World")]
    [Serializable]
    public class Ability_Info : DataObject
    {
        [DataElement()]
        public UInt16 Entry;

        [DataElement()]
        public byte Level;

        [DataElement()]
        public byte CareerLine;

        [DataElement]
        public UInt16 AbilityType;

        [DataElement()]
        public int MinRange;

        [DataElement()]
        public int MaxRange;

        [DataElement]
        public int CastTime;

        [DataElement]
        public int CoolDown;

        [DataElement()]
        public byte TargetType;

        [DataElement()]
        public int Damage;

        [DataElement()]
        public UInt16 ActionPoints;
    }
}
