using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class ACLineSegment : Conductor
    {
        private bool feederCable;
        private float r;

        public ACLineSegment(long globalId) : base(globalId)
        {
        }

        public bool FeederCable
        {
            get { return feederCable; }
            set { feederCable = value; }
        }

        public float R
        {
            get { return r; }
            set { r = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                ACLineSegment x = (ACLineSegment)obj;
                return (x.feederCable == this.feederCable && x.r == this.r);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation
        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.ACLINESEGMENT_FEEDER:
                case ModelCode.ACLINESEGMENT_R:
                    return true;

                default:
                    return base.HasProperty(t);

            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ACLINESEGMENT_FEEDER:
                    property.SetValue(feederCable);
                    break;

                case ModelCode.ACLINESEGMENT_R:
                    property.SetValue(r);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ACLINESEGMENT_FEEDER:
                    feederCable = property.AsBool();
                    break;

                case ModelCode.ACLINESEGMENT_R:
                    r = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion IAccess implementation
    }
}
