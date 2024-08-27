namespace Diagrams.View
{
    using System.Windows;
    using ViewContext;

    public partial class BuildDiagramsView : Window
    {
        public BuildDiagramsView(BuildDiagramsContext context)
        {
            DataContext = context;
            InitializeComponent();
        }
    }
}
