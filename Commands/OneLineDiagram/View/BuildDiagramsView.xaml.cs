namespace Diagrams.View
{
    using ViewContext;

    public partial class BuildDiagramsView
    {
        public BuildDiagramsView(BuildDiagramsContext context)
        {
            DataContext = context;
            InitializeComponent();
        }
    }
}
