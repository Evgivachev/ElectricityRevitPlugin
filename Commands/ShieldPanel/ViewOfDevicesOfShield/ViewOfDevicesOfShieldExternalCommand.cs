namespace ShieldPanel.ViewOfDevicesOfShield;

using System;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class ViewOfDevicesOfShieldExternalCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var result = Result.Succeeded;
        var application = commandData?.Application;
        var activeUiDocument = application?.ActiveUIDocument;
        var document = activeUiDocument?.Document;

        try
        {
            using (var transactionGroup = new TransactionGroup(document, "trGrName"))
            {
                if (TransactionStatus.Started == transactionGroup.Start())
                {
                    var selection = activeUiDocument.Selection;
                    var shieldId = selection.PickObject(ObjectType.Element).ElementId;
                    var shield = document.GetElement(shieldId) as FamilyInstance;

                    var processing = new ShieldProcessing(shield);
                    using (var tr = new Transaction(document))
                    {
                        tr.Start("ViewOfShield");
                        processing.SetParametersOfThis();
                        tr.Commit();
                    }
                }
                transactionGroup.Assimilate();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error");
            result = Result.Failed;
        }
        return result;
    }
}
