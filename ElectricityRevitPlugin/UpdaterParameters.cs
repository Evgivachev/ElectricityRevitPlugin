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
        private List<Func<T,string>> _actions = new List<Func<T, string>>();
        public virtual IEnumerable<T> GetElements()
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

        //public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        //{
        //    var uiApp = commandData.Application;
        //    var uiDoc = uiApp.ActiveUIDocument;
        //    var doc = uiDoc.Document;
        //    var app = uiApp.Application;
        //    var result = Result.Succeeded;

        //    try
        //    {
        //        using (var tr = new Transaction(doc))
        //        {
        //            tr.Start(this.GetType().Name);
        //            parameterUpdater.Execute();
        //            tr.Commit();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        message += e.Message + '\n' + e.StackTrace;
        //        result = Result.Failed;
        //    }
        //    finally
        //    {

        //    }
        //    return result;
        //}
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
