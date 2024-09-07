namespace CommonUtils.Sample;

public class ViewModel
{
    private readonly IRevitService _revitService;
    public ViewModel(IRevitService revitService)
    {
        _revitService = revitService;
    }
}
