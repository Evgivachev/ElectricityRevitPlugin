namespace ElectricityRevitPlugin.ArchTests;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using Xunit;

//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

public class UnitTest1
{
    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssemblies(typeof(ExampleClass).Assembly, 
            typeof(ForbiddenClass).Assembly).Build();
    
    [Fact]
    public void Test1()
    {

    }
}
