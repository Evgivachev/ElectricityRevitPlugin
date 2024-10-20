namespace GeneralSubjectDiagram.Views;

using System;
using ViewModels;

/// <summary>
/// Логика взаимодействия для GeneralSubjectWpf.xaml
/// </summary>
public partial class GeneralSubjectView
{
    /// <inheritdoc />
    public GeneralSubjectView(GeneralSubjectViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
    private void GeneralSubjectView_OnContentRendered(object sender, EventArgs e)
    {
        // ((GeneralSubjectViewModel)DataContext).InitializeCommand!.Execute(this);
    }
}
