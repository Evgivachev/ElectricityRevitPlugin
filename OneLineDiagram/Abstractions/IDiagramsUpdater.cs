namespace Diagrams.Abstractions
{
    using Autodesk.Revit.DB;

    public interface IDiagramsUpdater
    {
        public void UpdateDiagram(View view);

    }
}
