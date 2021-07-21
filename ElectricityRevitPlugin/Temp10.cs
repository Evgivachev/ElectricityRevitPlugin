using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitParametersCodeGenerator;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Temp10 :DefaultExternalCommand
    {
        protected override Result DoWork(ref string message, ElementSet elements)
        {
            using (var tr = new Transaction(Doc, "Rename"))
            {
                tr.Start();

                var fm = Doc.FamilyManager;
                foreach (FamilyType type in fm.Types)
                {
                    var typeName = type.Name;
                    fm.CurrentType = type;
                    var m = fm.get_Parameter(SharedParametersFile.ADSK_Marka);
                    fm.Set(m, typeName);
                }

               
                tr.Commit();
            }

            return Result.Succeeded;

        }
    }
}
