﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Re4QuadExtremeEditor.src.Class.CustomDelegates;

namespace Re4QuadExtremeEditor.src.Class.ObjMethods
{
    public class EtcModelMethodsForGL
    {
        public ReturnVector3 GetPosition;

        public ReturnVector3 GetScale;

        public ReturnMatrix4 GetAngle;

        public ReturnUshort GetEtcModelID;

        public ReturnOldRotationAngles GetOldRotation; // for OldGL;
    }
}
