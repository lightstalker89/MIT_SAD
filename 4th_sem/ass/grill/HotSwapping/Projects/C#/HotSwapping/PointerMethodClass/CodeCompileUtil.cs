using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HotSwapping.Enumeration;
using HotSwapping.Model;
using Microsoft.CSharp;

namespace HotSwapping.PointerMethodClass
{
    public class CodeCompileUtil
    {
        private readonly string codeTemplatePointer;
        private readonly string codeTemplatePointerMono;
        private readonly CSharpCodeProvider provider;
        private readonly CompilerParameters parameters;

        public CodeCompileUtil()
        {
            this.provider = new CSharpCodeProvider(new Dictionary<string, string>
            {
                { "CompilerVersion", "v4.0" }
            });
            this.parameters = new CompilerParameters();
            var assemblies = AppDomain.CurrentDomain
                            .GetAssemblies()
                            .Where(a => !a.IsDynamic)
                            .Select(a => a.Location);

            this.parameters.ReferencedAssemblies.AddRange(assemblies.ToArray());
            this.parameters.GenerateInMemory = true;
            this.parameters.OutputAssembly = "tempFile.dll";
            this.parameters.GenerateExecutable = false;

            this.codeTemplatePointer = @"
            using System;
            using System.Collections.Generic;
            using System.ComponentModel;
            using System.Data;
            using System.Drawing;
            using System.Linq;
            using System.Text;
            using System.Windows.Forms;

            namespace HotSwapping
            {{
                public class Container {{
                    public void containerMethod() {{
                        {0}
                    }}
                }}
            }}";

            this.codeTemplatePointerMono = @"
            using System;
            using System.Collections.Generic;
            using System.ComponentModel;
            using System.Data;
            using System.Drawing;
            using System.Linq;
            using System.Text;
            using System.Windows.Forms;

            namespace HotSwapping
            {{
                public class Container {{
                    public static void containerMethod() {{
                        {0}
                    }}
                }}
            }}";
        }

        public CompileModel compileCode(SwappingMethod method, string code)
        {
            string injectedCode = string.Format(
                (method == SwappingMethod.POINTER) ? 
                this.codeTemplatePointer :
                this.codeTemplatePointerMono,
                code);
            CompilerResults result = this.provider.CompileAssemblyFromSource(this.parameters, injectedCode);
            Assembly assembly = result.Errors.HasErrors ? null : result.CompiledAssembly;
            return new CompileModel(assembly, result);
        }
    }
}
