using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace BimRenRes.AddParametersToFamilyInstance;

class ParameterSetter
{
    private Dictionary<string, List<ExternalDefinition>> _notAddedParameters = new Dictionary<string, List<ExternalDefinition>>();
    public void AddParameters(Document doc, IEnumerable<ExternalDefinition> parameters, BuiltInParameterGroup group, bool isInstance)
    {
            var spf = doc.Application.OpenSharedParameterFile();
            _notAddedParameters = new Dictionary<string, List<ExternalDefinition>>();
            foreach (ExternalDefinition p in parameters)
            {
                try
                {
                    doc.FamilyManager.AddParameter(p, group, isInstance);
                }
                catch (Exception e)
                {
                    var m = e.Message;
                    var st = e.StackTrace;
                    if (!_notAddedParameters.ContainsKey(m))
                        _notAddedParameters[m] = new List<ExternalDefinition>();
                    _notAddedParameters[m].Add(p);
                }
            }
        }
    public string GetErrorMessage()
    {
            if (_notAddedParameters.Count == 0)
                return null;
            var error = new StringBuilder();
            error.AppendLine("Не удалось добавить параметры");
            foreach (var pair in _notAddedParameters)
            {
                error.AppendLine($"{pair.Key}:");
                foreach (var p in pair.Value)
                {
                    error.AppendLine('\t' + p.Name);
                }
            }
            return error.ToString();
        }
}