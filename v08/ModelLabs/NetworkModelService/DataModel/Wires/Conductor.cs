using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Conductor : ConductingEquipment
    {
        private ConductorMaterialKind conductorMaterial;
        private float length;

        public Conductor(long globalId) : base(globalId)
        {
        }

        public ConductorMaterialKind ConductorMaterial
        {
            get { return conductorMaterial; }
            set { conductorMaterial = value; }
        }

        public float Length
        {
            get { return length; }
            set { length = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Conductor x = (Conductor)obj;
                return (x.conductorMaterial == this.conductorMaterial && x.length == this.length);
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
                case ModelCode.CONDUCTOR_CONDMAT:
                case ModelCode.CONDUCTOR_LENGTH:
                    return true;

                default:
                    return base.HasProperty(t);

            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CONDUCTOR_CONDMAT:
                    property.SetValue((short)conductorMaterial);
                    break;

                case ModelCode.CONDUCTOR_LENGTH:
                    property.SetValue(length);
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
                case ModelCode.CONDUCTOR_CONDMAT:
                    conductorMaterial = (ConductorMaterialKind)property.AsEnum();
                    break;

                case ModelCode.CONDUCTOR_LENGTH:
                    length = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion IAccess implementation
    }
}
