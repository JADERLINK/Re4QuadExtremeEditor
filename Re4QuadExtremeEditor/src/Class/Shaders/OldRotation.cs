using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src.Class
{
    public struct OldRotation
    {
        public static readonly OldRotation Identity = new OldRotation(0, 0, 0, ObjRotationOrder.NoRotation);

        public float XangleRadius { get; }
        public float YangleRadius { get; }
        public float ZangleRadius { get; }

        public float XangleDegrees { get => XangleRadius * (180/MathHelper.Pi); }
        public float YangleDegrees { get => YangleRadius * (180/MathHelper.Pi); }
        public float ZangleDegrees { get => ZangleRadius * (180/MathHelper.Pi); }

        public ObjRotationOrder GetObjRotationOrder { get;}

        public OldRotation(float XangleRadius, float YangleRadius, float ZangleRadius, ObjRotationOrder RotationOrder) 
        {
            this.XangleRadius = XangleRadius;
            this.YangleRadius = YangleRadius;
            this.ZangleRadius = ZangleRadius;
            GetObjRotationOrder = RotationOrder;
        }
    }
}
