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
    // Valeur Fixe d'un character
    [DataTable(PreCache = false, TableName = "GameObject_protos", DatabaseName = "World")]
    [Serializable]
    public class GameObject_proto : DataObject
    {
        [DataElement(Unique=true,AllowDbNull=false)]
        public uint Entry;

        [DataElement(Varchar=255)]
        public string Name;

        [DataElement()]
        public UInt16 DisplayID;

        [DataElement()]
        public UInt16 Scale;

        [DataElement()]
        public byte Level;

        [DataElement()]
        public byte Faction;

        [DataElement()]
        public uint HealthPoints;

        [DataElement()]
        public uint AttackTime;

        [DataElement()]
        public uint MinDamage;

        [DataElement()]
        public uint MaxDamage;

        [DataElement()]
        public UInt32 TokUnlock;

        [DataElement()]
        public UInt16[] Unks = new UInt16[6];

        public UInt16 GetUnk(int Id)
        {
            if (Id >= Unks.Length)
                return 0;

            return Unks[Id];
        }

        [DataElement(Varchar = 255)]
        public string ScriptName;
    }
}
