using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Re4QuadExtremeEditor.src.Class.CustomDelegates;

namespace Re4QuadExtremeEditor.src.Class.ObjMethods
{
    public class ExtrasMethodsForGL
    {
        public ReturnSpecialType GetSpecialType;

        // warp Door position, ladder up position, GrappleGun start positon 
        public ReturnVector3 GetFirtPosition;

        public ReturnMatrix4 GetWarpRotation;
        public ReturnOldRotationAngles GetWarpAltRotation;

        public ReturnMatrix4 GetLocationAndLadderRotation;
        public ReturnOldRotationAngles GetLocationAndLadderAltRotation;

        public ReturnSbyte GetLadderStepCount;

        public ReturnVector3 GetAshleyPoint;

        public ReturnVector2Array GetAshleyHidingZoneCorner;

        public ReturnVector3 GetGrappleGunEndPosition;

        public ReturnVector3 GetGrappleGunThirdPosition;

        public ReturnMatrix4 GetGrappleGunFacingAngleRotation;
        public ReturnOldRotationAngles GetGrappleGunFacingAngleAltRotation;

        public ReturnByte GetGrappleGunParameter3;

    }
}
