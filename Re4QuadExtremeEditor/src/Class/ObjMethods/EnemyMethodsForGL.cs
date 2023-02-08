﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Re4QuadExtremeEditor.src.Class.CustomDelegates;

namespace Re4QuadExtremeEditor.src.Class.ObjMethods
{
    public class EnemyMethodsForGL
    {
        public ReturnVector3 GetPosition;

        public ReturnMatrix4 GetRotation;

        public ReturnUshort GetEnemyModelID;

        public ReturnUshort GetEnemyRoom;

        public ReturnByte GetEnableState;

        public ReturnOldRotationAngles GetOldRotation; // for OldGL;
    }
}
