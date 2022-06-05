namespace ElectricityRevitPlugin.GeneralSubject
{
    using System;
    using Autodesk.Revit.DB;

    class OutgoingLineParameterUpdater : CableParameterUpdater
    {
        public OutgoingLineParameterUpdater(Element fromElement)
            : base(fromElement)
        {
        }

        public override CollectionOfCheckableItems GetValidateElements(Document document)
        {
            throw new NotImplementedException();
        }
    }
}
