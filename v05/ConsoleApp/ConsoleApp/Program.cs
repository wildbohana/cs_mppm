using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            // BaseVoltage - prvi
            var bv1gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.BASEVOLTAGE, -1);
            // Iz BaseVoltage
            var bv1prop1 = new Property(ModelCode.BASEVOLTAGE_NOMINALVOLTAGE, 110f);
            // Iz IdentifiedObject
            var bv1prop2 = new Property(ModelCode.IDOBJ_MRID, "bv1");
            var bv1prop3 = new Property(ModelCode.IDOBJ_NAME, "bv1");
            var bv1prop4 = new Property(ModelCode.IDOBJ_DESCRIPTION, "bv1");
            // Sve propertije stavljamo u listu
            var bv1props = new List<Property> { bv1prop1, bv1prop2, bv1prop3, bv1prop4 };

            // Instanciranje prvog BaseVoltage
            var bv1 = new ResourceDescription(bv1gid, bv1props);


            // BaseVoltage - drugi
            var bv2gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.BASEVOLTAGE, -2);
            // Iz BaseVoltage
            var bv2prop1 = new Property(ModelCode.BASEVOLTAGE_NOMINALVOLTAGE, 20f);
            // Iz IdentifiedObject
            var bv2prop2 = new Property(ModelCode.IDOBJ_MRID, "bv2");
            var bv2prop3 = new Property(ModelCode.IDOBJ_NAME, "bv2");
            var bv2prop4 = new Property(ModelCode.IDOBJ_DESCRIPTION, "bv2");
            // Sve propertije stavljamo u listu
            var bv2props = new List<Property> { bv2prop1, bv2prop2, bv2prop3, bv2prop4 };

            // Instanciranje drugog BaseVoltage
            var bv2 = new ResourceDescription(bv2gid, bv2props);


            // Lokacija
            var locgid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.LOCATION, -1);
            var loc = new ResourceDescription(locgid);


            // Transformator
            var trafogid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POWERTR, -1);
            // Iz svake nasleđene klase po jedan properti (demonstracije radi)
            var trafoprop1 = new Property(ModelCode.POWERTR_FUNC, (short)TransformerFunction.Grounding);
            var trafoprop2 = new Property(ModelCode.EQUIPMENT_ISPRIVATE, true);
            var trafoprop3 = new Property(ModelCode.PSR_CUSTOMTYPE, "nesto");
            var trafoprop4 = new Property(ModelCode.IDOBJ_NAME, "tr1");
            var trafoprop5 = new Property(ModelCode.PSR_LOCATION, locgid);  // Povezivanje sa lokacijom
            // Sve propertije stavljamo u listu
            var trafoprops = new List<Property> { trafoprop1, trafoprop2, trafoprop3, trafoprop4, trafoprop5 };

            // Instanciranje transformatora
            var trafo = new ResourceDescription(trafogid, trafoprops);


            // TransformerWinding - prvi
            var tw1gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POWERTRWINDING, -1);
            // Iz svake nasleđene klase po jedan properti (demonstracije radi)
            var tw1prop1 = new Property(ModelCode.CONDEQ_BASVOLTAGE, bv1gid);  // Spajanje sa BV1
            var tw1prop2 = new Property(ModelCode.EQUIPMENT_ISUNDERGROUND, false);
            var tw1prop3 = new Property(ModelCode.PSR_CUSTOMTYPE, "nesto");
            var tw1prop4 = new Property(ModelCode.IDOBJ_NAME, "tw1");
            var tw1prop5 = new Property(ModelCode.POWERTRWINDING_POWERTRW, trafogid);  // Povezivanje sa trafoom
            // Sve propertije stavljamo u listu
            var tw1props = new List<Property> { tw1prop1, tw1prop2, tw1prop3, tw1prop4, tw1prop5 };

            // Instanciranje prvog TW
            var tw1 = new ResourceDescription(tw1gid, tw1props);


            // TransformerWinding - drugi
            var tw2gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POWERTRWINDING, -2);
            // Iz svake nasleđene klase po jedan properti (demonstracije radi)
            var tw2prop1 = new Property(ModelCode.CONDEQ_BASVOLTAGE, bv2gid);  // Spajanje sa BV2
            var tw2prop2 = new Property(ModelCode.EQUIPMENT_ISUNDERGROUND, true);
            var tw2prop3 = new Property(ModelCode.PSR_CUSTOMTYPE, "nesto");
            var tw2prop4 = new Property(ModelCode.IDOBJ_NAME, "tw2");
            var tw2prop5 = new Property(ModelCode.POWERTRWINDING_POWERTRW, trafogid);  // Povezivanje sa trafoom
            // Sve propertije stavljamo u listu
            var tw2props = new List<Property> { tw2prop1, tw2prop2, tw2prop3, tw2prop4, tw2prop5 };

            // Instanciranje prvog TW
            var tw2 = new ResourceDescription(tw2gid, tw2props);


            // Delta
            var delta = new Delta();
            delta.AddDeltaOperation(DeltaOpType.Insert, bv1, false);
            delta.AddDeltaOperation(DeltaOpType.Insert, bv2, false);
            delta.AddDeltaOperation(DeltaOpType.Insert, tw1, false);
            delta.AddDeltaOperation(DeltaOpType.Insert, tw2, false);
            delta.AddDeltaOperation(DeltaOpType.Insert, trafo, false);


            // Export u XML
            using (var writer = new System.Xml.XmlTextWriter("test.xml", Encoding.UTF8))
            {
                delta.ExportToXml(writer);
            }
        }
    }
}
