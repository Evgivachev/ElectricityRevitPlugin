namespace ElectricityRevitPlugin;

using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
class Temp8 : DefaultExternalCommand
{
    protected override Result DoWork(ref string message, ElementSet elements)
    {
        try
        {
            var els = new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .OfType<ElectricalSystem>();
            using (var tr = new Transaction(Doc))
            {
                tr.Start("sdsdsds");
                foreach (var system in els)
                {
                    //Создать в проекте временный параметр с типом длина
                    var tmbDParameter = system.LookupParameter("ТЭ Д");
                    //Создать в проекте временный параметр с типом число
                    var tmbPParameter = system.LookupParameter("ТЭ П");
                    var first = 4;
                    //Копорование Длины и Потель напряжения во временные параметры
                    if (first == 1)
                    {
                        var d = system.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d")).AsDouble();
                        double.TryParse(
                            system.get_Parameter(new Guid("b4954a6d-3d42-44ff-b700-e308cf0fcc46")).AsString(),
                            NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
                            out var du);
                        tmbDParameter.Set(UnitUtils.ConvertToInternalUnits(d, UnitTypeId.Meters));
                        tmbPParameter.Set(du);
                    }
                    //TODO
                    //Удвлить из проекта параметры Длина для ОС и Потери напряжения для ОС и добавить одноименные общие параметры
                    //Копирование длины и потерь обратно в параметры
                    else if (first == 2)
                    {
                        var dParameter = system.get_Parameter(new Guid("387ba243-768e-45cf-9c22-ce1b5650fe3d"));
                        var duParameter = system.get_Parameter(new Guid("b4954a6d-3d42-44ff-b700-e308cf0fcc46"));
                        dParameter.Set(tmbDParameter.AsDouble());
                        duParameter.Set(tmbPParameter.AsDouble());
                    }
                    //TODO Добавить общие параметры Резерввная группа и Контрольные цепи и Запретить изменение и Запретить изменение для наименования нагрузки
                    else if (first == 3)
                    {
                        var reserve0 = system.GetParameters("Резервная группа").First(p => !p.IsShared);
                        var control0 = system.GetParameters("Контрольные цепи").First(p => !p.IsShared);
                        var reserve = system.get_Parameter(new Guid("cd2dc469-276a-40f4-bd34-c6ab2ae05348"));
                        var control = system.get_Parameter(new Guid("0f13e1e5-71bb-4b0f-b3dc-18054c25e1ee"));
                        reserve.Set(reserve0.AsInteger());
                        control.Set(control0.AsInteger());
                    }
                    else if (first == 4)
                    {
                        //Запретить изменение
                        system.get_Parameter(new Guid("be64f474-c030-40cf-9975-6eaebe087a84"))
                            .Set(0);
                        //Запретить изменение наименование нагрузки
                        system.get_Parameter(new Guid("5de14719-6968-4655-9457-94825e70b623")).Set(0);
                    }
                }

                tr.Commit();
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message + "\n" + e.StackTrace);
        }

        return Result.Succeeded;
    }
}
