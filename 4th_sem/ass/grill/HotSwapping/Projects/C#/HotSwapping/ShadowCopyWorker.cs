using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using HotSwapping.Helper;
using SwappingLibrary;

namespace HotSwapping
{
    public class ShadowCopyWorker : MarshalByRefObject
    {
        public void doWorkWithShadow()
        {
            AppDomainSetup setup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                ShadowCopyFiles = "true",
                // CachePath = @"C:\Users\Flo\Desktop\HotSwapping\HotSwapping\bin\Debug\Test",
                ShadowCopyDirectories = PathHelper.getAssemblyPath(),
            };
            AppDomain newDomain = AppDomain.CreateDomain("tempDomain2", null, setup); //Create an instance of loader class in new appdomain
            var swappingLibrary = newDomain.CreateInstanceAndUnwrap("SwappingLibrary", "SwappingLibrary.SwappingClass");
            Type type = swappingLibrary.GetType();
            MethodInfo methodInfo = type.GetMethod("swappingMethod", Type.EmptyTypes);
            object instance = Activator.CreateInstance(type);
            methodInfo.Invoke(instance, null);

            // Unload the application domain and its resources
            AppDomain.Unload(newDomain); 
        }
    }
}
