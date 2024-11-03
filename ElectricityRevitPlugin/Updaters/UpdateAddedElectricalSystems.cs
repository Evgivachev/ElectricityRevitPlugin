namespace ElectricityRevitPlugin.Updaters;

using System;
using System.Linq;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using CommonUtils.Helpers;
using UpdateParametersInCircuits;

/// <summary>
/// Установка всех булевых параметров в 0
/// Установка траектории электрической цепи через все элементы
/// </summary>
public class UpdateAddedElectricalSystems : MyUpdater
{
    public UpdateAddedElectricalSystems(AddInId id)
        : base(id)
    {
    }

    protected override Guid UpdaterGuid { get; } = new("E4B5B915-4274-42E7-BE4E-AC3866326E92");

    protected override string Name { get; } = "Обновление вновь добавленных электрических цепей";

    protected override ChangePriority ChangePriority { get; } = ChangePriority.Annotations;
    protected override string AdditionalInformation { get; } = "Обновление вновь добавленных электрических цепей";
    public override ElementFilter ElementFilter { get; } = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalCircuit);

    protected override void ExecuteInner(UpdaterData data)
    {
        try
        {
            var doc = data.GetDocument();
            var command = new SetModeOfElectricalSystemToAllElementsExternalCommand();
            var systems = data.GetAddedElementIds()
                .Select(x => doc.GetElement(x))
                .OfType<ElectricalSystem>();
            foreach (var system in systems)
            {
                command.UpdateParameters(system);
                foreach (Parameter parameter in system.Parameters)
                {
                    if (!parameter.HasValue && parameter.StorageType == StorageType.Integer
                                            && parameter.UserModifiable
                                            && parameter.Definition.GetDataType() == SpecTypeId.Boolean.YesNo)
                        parameter.Set(0);
                }

                system.get_Parameter(SharedParametersFile.Koeffitsient_Sprosa_V_SHCHitakh).Set(1.0);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show($"{e.Message}\n{e.StackTrace}");
        }
    }
}
