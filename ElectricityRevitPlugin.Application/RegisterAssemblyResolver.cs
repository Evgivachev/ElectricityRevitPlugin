namespace ElectricityRevitPlugin.Application;

using System.Reflection;

public class RegisterAssemblyResolver : ISyncBackGroundService
{

    public void Execute()
    {
        var currentDomain = AppDomain.CurrentDomain;
        currentDomain.AssemblyResolve += MyResolveEventHandler;
    }

    private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
    {
        var directory = new FileInfo(typeof(ISyncBackGroundService).Assembly.Location).Directory!;
        var dlls = directory.GetFiles("*.dll");
        var file = dlls.FirstOrDefault(f => f.Name.Substring(0, f.Name.Length - 4) == args.Name.Split(", ".ToCharArray())[0]);
        if (file == null)
            return null;
        return Assembly.Load(file.FullName);
    }
}
