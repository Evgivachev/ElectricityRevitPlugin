using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;

namespace ElectricityRevitPlugin.GeneralSubject
{
    class OutgoingLineParameterUpdater : CableParameterUpdater
    {
        public OutgoingLineParameterUpdater(Element fromElement) : base(fromElement)
        {

        }

        public override MyCollectionOfCheckableItems GetValidateElements(Document document)
        {
            throw new NotImplementedException();
        }
    }
}
