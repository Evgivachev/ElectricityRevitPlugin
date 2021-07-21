using System;
using System.CodeDom;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace RevitParametersCodeGenerator
{

    public class Parameter
    {
        public Parameter(string line)
        {
            var array = line.Split(new[] { '\t' });
            if (array[0] != "PARAM")
                throw new ArgumentException("Line is not parameter definition");
            Guid = new Guid(array[1]);
            DefaultName = array[2];
            DataType = array[3];
            DataCategory = array[4];
            Group = array[5];
            Visible = int.Parse(array[6]) == 1;
            Description = array[7];
            UserModifiable = int.Parse(array[8]) == 1;
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid Guid { get; }
        public string DefaultName { get; }
        public string DataType { get; }
        public string DataCategory { get; }
        public string Group { get; }
        public bool Visible { get; }
        public string Description { get; }
        public bool UserModifiable { get; }

        public string Name => Tr2(DefaultName);


        private static string Tr2(string s)
        {
            var startWithNumberPattern = new Regex("\\A[0-9]*_|\\A[0-9]*");
            if (startWithNumberPattern.IsMatch(s))
            {
                var match = startWithNumberPattern.Match(s);
                s = startWithNumberPattern.Replace(s, "") + match;
            }
            var ret = new StringBuilder();
            char[] rus =
            {
                'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й',
                'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц',
                'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я'
            };
            string[] eng =
            {
                "A", "B", "V", "G", "D", "E", "E", "ZH", "Z", "I", "Y",
                "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "KH", "TS",
                "CH", "SH", "SHCH", null, "Y", null, "E", "YU", "YA"
            };
            var nextIsUpper = false;
            foreach (var t in s)
            {
                var isSpacesPattern = new Regex("\\s");
                if (t == '_'||isSpacesPattern.IsMatch(t.ToString()))
                {
                    ret.Append('_');
                    nextIsUpper = true;
                    continue;
                }
                var flag = false;
                for (int i = 0; i < rus.Length; i++)
                {
                    if (char.ToUpper(t) == rus[i])
                    {
                        if (eng[i] == null)
                            continue;
                        ret.Append(char.IsLower(t)&&!nextIsUpper ? eng[i].ToLower() : eng[i]);
                        nextIsUpper = false;
                        flag = true;
                        break;
                    }
                }
                if (flag)
                    continue;
                var pattern = new Regex("[a-z]|[0-9]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                if (pattern.IsMatch(t.ToString()))
                {
                    ret.Append(t);
                    nextIsUpper = false;
                }
            }

            return ret.ToString();
        }

        public string GetCodeString(string name = null)
        {
            if (name == null)
                name = Name;
            return $@"/// <summary>
///{DefaultName}{(string.IsNullOrEmpty(Description) ? null : "\n///" + $"{Description}")}
/// </summary>
public static Guid {name} = new Guid(""{Guid.ToString()}"");";
        }
    }
}