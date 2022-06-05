namespace Diagrams.Abstractions
{
    using Autodesk.Revit.DB;
    using Models;

    public interface IDiagramsDrawer
    {
        public void DrawDiagram(Shield shield);
        public void SetParametersToHead(FamilyInstance head, Shield shield);

        public void DrawLines(Shield shield, View view);
    }
}
