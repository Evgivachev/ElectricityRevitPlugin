using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RevitParametersCodeGenerator
{
    public class SharedParametersFileGenerator
    {
        private string NameSpace { get; set; } = "RevitParametersCodeGenerator";
        public string ClassName { get; set; } = "SharedParametersFile";

        private Parameter AddParameter(string line)
        {
            var param = new Parameter(line);
            return param;
        }

        private string Head =>
            $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace {NameSpace}
{{
    public static class SharedParametersFile
    {{";

        private string Bottom =>
            @"  }
}
";

        private IEnumerable<Parameter> ReadFile(string file)
        {
            using var stream = new StreamReader(file, Encoding.Default);
            while (true)
            {
                var line = stream.ReadLine();
                if (line is null)
                    break;
                if (!line.StartsWith("PARAM"))
                    continue;
                yield return AddParameter(line);
            }
        }

        public string GetClassFile(string file)
        {
            var result = new StringBuilder();
            result.AppendLine(Head);
            var parameters = new HashSet<string>();
            foreach (var parameter in ReadFile(file))
            {
                var parameterName = parameter.Name;
                var n = 1;
                while (parameters.Contains(parameterName))
                    parameterName = $"{parameter.Name}_{n++}";
                result.AppendLine(parameter.GetCodeString(parameterName));
                parameters.Add(parameterName);
            }

            result.AppendLine(Bottom);
            return result.ToString();
        }
    }
}
