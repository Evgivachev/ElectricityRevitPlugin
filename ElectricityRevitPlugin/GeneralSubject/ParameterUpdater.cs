using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI.Selection;

namespace ElectricityRevitPlugin.GeneralSubject
{
    public abstract class ParameterUpdater
    {
        public static Guid ReflectionClassNameGuid = new Guid("6c36d5e8-7863-4efb-accf-894a5aa95cc1");
        public static Guid ConnectedElementId = new Guid("dca1fe51-4090-4178-9f12-a83aa5986266");
        private readonly ElementId _id;
        private readonly Element _fromElement;
        protected readonly Document Doc;
        protected ParameterUpdater(Element fromElement)
        {
            _fromElement = fromElement;
            _id = _fromElement.Id;
            Doc= _fromElement.Document;
        }
        protected ParameterUpdater()
        {

        }

        public abstract CollectionOfCheckableItems GetValidateElements(Document document);

        protected Dictionary<dynamic, dynamic> ParametersDictionary = new Dictionary<dynamic, dynamic>();

        public virtual FamilyInstance InsertInstance(FamilySymbol familySymbol, XYZ point)
        {
            var instance = Doc.Create.NewFamilyInstance(point, familySymbol, Doc.ActiveView);
            SetParameters(instance);
            return instance;
        }
        protected Dictionary<string, Func<object, dynamic>> FuncParametricDictionary = new Dictionary<string, Func<object, dynamic>>();

        public virtual void SetParameters(Element toElement)
        {
            var p = toElement.get_Parameter(new Guid("be64f474-c030-40cf-9975-6eaebe087a84"))?.AsInteger() == 1;
            if (p)
                return;
            SetSameParameters(toElement);
            Doc.Regenerate();
            SetParametersFromParametersDictionary(toElement);
            SetParametersFromFuncDictionary(toElement);
        }
        protected virtual void SetParametersFromParametersDictionary(Element toElement)
        {
            foreach (var pair in ParametersDictionary)
            {
                var fromP = (Parameter)_fromElement.get_Parameter(pair.Key);
                var toP =(Parameter) toElement.get_Parameter(pair.Value);
                if(fromP is null || toP is null)
                    throw new NullReferenceException();
                var flag  = toP.Set(fromP.GetValueDynamic());
            }
        }

        protected void SetParametersFromFuncDictionary(Element toElement)
        {
            foreach (var func in FuncParametricDictionary)
            {
                var toP = toElement.LookupParameter(func.Key);
                if(toP is null)
                    throw new NullReferenceException();
                var value = func.Value.Invoke(_fromElement);
                if(value is null)
                    continue;
                toP.Set(value);
            }
        }

        public void SetSameParameters(Element toElement)
        {
            foreach (Parameter fromP in _fromElement.Parameters)
            {
                if(!fromP.IsShared)
                    continue;
                var fromGuid = fromP.GUID;
                var toP = toElement.get_Parameter(fromGuid);
                if(toP is null || toP.IsReadOnly ||!toP.IsShared)
                    continue;
                var flag = toP.Set(fromP.GetValueDynamic());
                if(!flag)
                    Debug.Print($"{toP.Definition.Name} is wrong" );
            }
            toElement.get_Parameter(ConnectedElementId).Set(_fromElement.Id.IntegerValue.ToString());
        }

        //protected XYZ PickPoint()
        //{
        //    ObjectSnapTypes snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Intersections;
        //    XYZ point = UiDoc.Selection.PickPoint(snapTypes, "Select an end point or intersection");
        //    return point;
        //}
    }
}
