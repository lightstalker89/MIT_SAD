using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HotSwapping.Model
{
    public class CompileModel
    {
        public CompileModel(Assembly assembly, CompilerResults result)
        {
            this.result = result;
            this.assembly = assembly;
        }

        public Assembly assembly { get; private set; }

        public CompilerResults result { get; private set; }
    }
}
