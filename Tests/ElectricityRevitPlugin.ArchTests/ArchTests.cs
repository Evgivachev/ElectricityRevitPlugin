namespace ElectricityRevitPlugin.ArchTests;

using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using GroupByGost.Domain;
using Xunit;

//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

public class ArchTests
{
    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssemblies(typeof(Element).Assembly).Build();

    //use As() to give your variables a custom description
    private readonly IObjectProvider<IType> DomainLayer =
        Types().That().ResideInNamespace("\\.Domain", true).As("Domain Layer");

    private readonly IObjectProvider<IType> AppLayer =
        Types().That().ResideInNamespace("\\.Application", true).As("App Layer");

    private readonly IObjectProvider<IType> InfraLayer =
        Types().That().ResideInNamespace("\\.Infrastructure", true).As("Infrastructure Layer");

    private const string AutodeskPattern = "^Autodesk.*";

    [Fact]
    public void TypesShouldBeInCorrectLayer()
    {
        IArchRule domainNotDependentAppLayer =
            Classes().That().Are(DomainLayer).Should()
                .NotDependOnAny(AppLayer);

        IArchRule domainNotDependentInfraLayer =
            Classes().That().Are(DomainLayer).Should()
                .NotDependOnAny(InfraLayer);

        IArchRule appNotDependentInfraLayer =
            Classes().That().Are(AppLayer).Should()
                .NotDependOnAny(InfraLayer);

        //you can also combine your rules
        var combinedArchRule =
            domainNotDependentAppLayer
                .And(domainNotDependentInfraLayer)
                .And(appNotDependentInfraLayer);

        combinedArchRule.Check(Architecture);
    }

    [Fact]
    public void DomainShouldNotDependOnAutodesk()
    {
        var rule = Classes().That().Are(DomainLayer).Should()
            .NotDependOnAny(AutodeskPattern, true);
        rule.Check(Architecture);
    }
    
    [Fact]
    public void BllShouldNotDependOnAutodesk()
    {
        var rule = Classes().That().Are(AppLayer).Should()
            .NotDependOnAny(AutodeskPattern, true);
        rule.Check(Architecture);
    }
}
