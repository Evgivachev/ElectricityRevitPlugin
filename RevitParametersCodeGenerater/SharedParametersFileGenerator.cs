using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitParametersCodeGenerator
{
    public class SharedParametersFileGenerator
    {
        public string NameSpace { get; set; } = "RevitParametersCodeGenerator";
        public string ClassName { get; set; } = "SharedParametersFile";

        private readonly List<Parameter> _parameters = new List<Parameter>();
        public Parameter AddParameter(string line)
        {
            var param = new Parameter(line);
            _parameters.Add(param);
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

        public IEnumerable<Parameter> ReadFile(string file)
        {
            using (var stream = new StreamReader(file, System.Text.Encoding.Default))
            {
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
        }

        public string GetClassFile(string file)
        {
            var result = new StringBuilder();
            result.AppendLine(Head);
            var parameters = new HashSet<string>();
            foreach (var parameter in ReadFile(file))
            {
                string parameterName = parameter.Name;
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
