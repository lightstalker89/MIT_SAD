using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    [Serializable]
    class TestAspect : PostSharp.Aspects.OnMethodBoundaryAspect
    {
        public override void OnEntry(PostSharp.Aspects.MethodExecutionArgs args)
        {
            if(args.Method.IsConstructor)
            {
                var test = args.Method.ReflectedType;
                Console.WriteLine("Module: " + args.Method.Module);
                Console.WriteLine("Constructor: " + args.Method.Name);
            }

            base.OnEntry(args);
        }
    }
}
