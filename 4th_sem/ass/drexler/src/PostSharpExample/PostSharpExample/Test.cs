using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PostSharp;
using PostSharp.Extensibility;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;

//[assembly: PostSharpExample.Test(AttributeTargetTypes = "PostSharpExample.Animal")]


namespace PostSharpExample
{
    [Serializable]
    [Test(AttributeExclude = true)]
    class Test : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            if(args.Method != null)
            {
                var a = args.Method.IsConstructor;
            }

            base.OnEntry(args);
        }
    }
}
