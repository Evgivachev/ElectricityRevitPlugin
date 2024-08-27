using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;

namespace ShieldPanel.SelectModelOfShield;

public class ShieldParametersIUpdater : IUpdater
{
    private UpdaterId _uId;

    public ShieldParametersIUpdater(Guid guid)
    {
        _uId = new UpdaterId(new AddInId(new Guid("53f09a69-306b-4d78-a818-ed38750126f2")), guid);
    }

    public void Execute(UpdaterData data)
    {
        var doc = data.GetDocument();
        var els = data.GetModifiedElementIds();
        foreach (var elId in els)
        {
            var el = doc.GetElement(elId);
            UpdateParameter(el);
        }
    }

    public UpdaterId GetUpdaterId()
    {
        return _uId;
    }

    public ChangePriority GetChangePriority()
    {
        return ChangePriority.MEPCalculations;
    }

    public string GetUpdaterName()
    {
        return "Обновление параметров при изменении оболочки щита";
    }

    public string GetAdditionalInformation()
    {
        return "AdditionalInformation";
    }

    private void UpdateParameter(Element el)
    {
        var from = new[]
        {
            el.LookupParameter("Ширина щита по каталогу"),
            el.LookupParameter("Высота щита по каталогу"),
            el.LookupParameter("Глубина щита по каталогу"),
            el.LookupParameter("Описание щита для спецификации"),
            el.LookupParameter("Артикул щита для спецификации"),
            el.LookupParameter("Тип щита для спецификации"),
            el.LookupParameter("Изготовитель щита для спецификации"),
            el.LookupParameter("Единица измерения щита для спецификации"),
            el.LookupParameter("Вес щита для спецификации"),
            el.LookupParameter("Расстояние между рядами")

        };
        var to = new[]
        {
            el.LookupParameter("Ширина щита"),
            el.LookupParameter("Высота щита"),
            el.LookupParameter("Глубина щита"),
            el.LookupParameter("Описание для спецификации"),
            el.LookupParameter("Артикул"),
            el.LookupParameter("Тип, марка, обозначение документа"),
            el.LookupParameter("Завод-изготовитель"),
            el.LookupParameter("Единица измерения"),
            el.LookupParameter("Вес"),
            el.LookupParameter("Расстояние между рядами DIN")

        };
        for (var i = 0; i < from.Length && i < to.Length; i++)
            try
            {
                if (from[i].HasValue)
                {
                    var value = GetValue(from[i]);
                    bool? flag;
                    flag = to[i]?.Set(value);
                }
                else
                {
                    if (to[i].StorageType == StorageType.String)
                        to[i].Set(string.Empty);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace);
            }

    }

    private dynamic GetValue(Parameter p)
    {
        var t = p.StorageType;
        switch (t)
        {
            case StorageType.Double:
                return p.AsDouble();
                break;
            case StorageType.ElementId:
                return p.AsElementId();
            case StorageType.Integer:
                return p.AsInteger();
            case StorageType.String:
                return p.AsString();
            default: return p.AsValueString();
        }
    }
}
