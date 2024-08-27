using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ShieldPanel.ViewOfDevicesOfShield;

public class ParametersOfShield
{
    private Dictionary<string, Parameter> _cl1Parameter = new();
    private Dictionary<string, Parameter> _cl2Parameter = new();
    private Dictionary<string, Parameter> _nm1Parameter = new();
    private Dictionary<string, Parameter> _nm2Parameter = new();
    private int _countOfParametersInFamilyInstance = 51;
    public ParametersOfShield(FamilyInstance sh)
    {
        var allParameters = sh.Parameters;

        foreach (Parameter parameter in allParameters)
        {
            var name = parameter.Definition.Name;
            if (name.StartsWith("К ОУ1_АВ$"))
                _cl1Parameter[name.Substring(9)] = parameter;
            else if (name.StartsWith("К ОУ2_АВ$"))
                _cl2Parameter[name.Substring(9)] = parameter;
            else if (name.StartsWith("КМ ОУ1_АВ$"))
                _nm1Parameter[name.Substring(10)] = parameter;
            else if (name.StartsWith("КМ ОУ2_АВ$"))
                _nm2Parameter[name.Substring(10)] = parameter;
        }
    }

    public void SetParameter(int number, double cl1, double nm1, double cl2, double nm2)
    {
        //Количество забитых параметров в семейство щита
        if (number > _countOfParametersInFamilyInstance)
            return;
        var nStr = number.ToString();
        _cl1Parameter[nStr].Set(cl1);
        _cl2Parameter[nStr].Set(cl2);
        _nm1Parameter[nStr].Set(nm1);
        _nm2Parameter[nStr].Set(nm2);
    }

    public void ToZero(int number)
    {
        var nStr = number.ToString();
        var flag = int.TryParse(nStr, out var nInt);
        if (!flag || nInt > _countOfParametersInFamilyInstance)
            return;
        var value = 0.0;
        var parameters = new[]
        {
            _cl1Parameter[nStr],
            _cl2Parameter[nStr],
            _nm1Parameter[nStr],
            _nm2Parameter[nStr]
        };
        var names = parameters.Select(x => x.Definition.Name).ToArray();
        var result = parameters.Select(p => p.Set(value)).ToArray();
        //_cl1Parameter[nStr].Set(value);
        //_cl2Parameter[nStr].Set(value);
        //_nm1Parameter[nStr].Set(value);
        //_nm2Parameter[nStr].Set(value);
    }

    public void ToZeroToEnd(int number)
    {
        for (var i = number; i <= _cl2Parameter.Count; i++)
            ToZero(i);
    }
}
