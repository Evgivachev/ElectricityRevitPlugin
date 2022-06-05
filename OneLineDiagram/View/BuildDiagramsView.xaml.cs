using System.Windows;

namespace Diagrams.View
{
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

