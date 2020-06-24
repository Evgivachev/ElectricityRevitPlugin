using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;


namespace ElectricityRevitPlugin
{
    public class UpdaterParameters<T> where T : Element
    {
        private readonly Document _doc;
        private readonly BuiltInCategory _category;

        public UpdaterParameters(Document document, BuiltInCategory category)
        {
            _doc = document;
            _category = category;
        }
        private readonly List<Func<T,string>> _actions = new List<Func<T, string>>();

        protected virtual IEnumerable<T> GetElements()
        {
            var fec = new FilteredElementCollector(_doc)
                .OfCategory(_category)
                .OfClass(typeof(T))
                .WhereElementIsNotElementType()
                .OfType<T>();
            return fec;

        }

        public void AddAction(IUpdaterParameters<T> updater)
        {
            _actions.Add(updater.UpdateParameters);
        }

        public void Execute()
        {
            var els = GetElements();
            foreach (var el in els)
            {
                foreach (var a in _actions)
                {
                    a.Invoke(el);
                }
            }
        }

    }
}
