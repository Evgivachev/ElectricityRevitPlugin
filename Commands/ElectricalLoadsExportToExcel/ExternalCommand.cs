namespace ElectricalLoadsExportToExcel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using MessageBox = System.Windows.MessageBox;

    /// <summary>
    /// Revit external command.
    /// </summary>	
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public sealed partial class ExternalCommand : IExternalCommand, IExternalCommandAvailability
    {
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var result = Result.Failed;
            try
            {
                var uiApp = commandData?.Application;
                var uiDoc = uiApp?.ActiveUIDocument;
                var app = uiApp?.Application;
                var doc = uiDoc?.Document;
                using var trGr = new TransactionGroup(doc, "trGrName");
                if (TransactionStatus.Started == trGr.Start())
                {
                    /* Here do your work or the set of
                     * works... */
                    var allShields = new FilteredElementCollector(doc)
                        .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                        .OfClass(typeof(FamilyInstance))
                        .Cast<FamilyInstance>()
                        .Where(x =>
                        {
                            var name = x.Name;
                            var uString = x.LookupParameter("Напряжение в щите").AsValueString().Split(' ')[0];
                            if (double.TryParse(uString, out var u) && u < 200) return false;
                            var flag = x.MEPModel?
                                .GetElectricalSystems()?
                                .Any();
                            return flag.HasValue && flag.Value;
                        }).ToArray();
                    var shieldsDictionary = allShields.ToDictionary(x => x.UniqueId);
                    var form = new SelectShields(allShields);
                    form.OkButton.Click += (sender, args) =>
                    {
                        try
                        {
                            var selectShield = new List<string>();
                            foreach (TreeNode node in form.ShieldsTreeView.Nodes)
                            {
                                foreach (TreeNode sheetNode in node.Nodes)
                                {
                                    var name = sheetNode.Name;
                                    if (sheetNode.Checked) selectShield.Add(name);
                                }
                            }

                            DoWork(commandData, selectShield);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + '\n' + ex.StackTrace, "Error");
                            result = Result.Failed;
                        }
                        finally
                        {
                            form.Close();
                        }
                    };
                    Application.Run(form);
                    trGr.RollBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + '\n' + ex.StackTrace, "Error");
                result = Result.Failed;
            }

            return result;
        }

        /// <inheritdoc />
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return applicationData.ActiveUIDocument?.Document is not null;
        }
    }
}
