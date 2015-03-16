using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using HotSwapping.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;
using OpCodes = Mono.Cecil.Cil.OpCodes;

namespace HotSwapping.MonoCecilClass
{
    public class MonoSwapperUtil
    {

        private readonly AssemblyDefinition swappingAssembly;

        public MonoSwapperUtil()
        {
            this.swappingAssembly = AssemblyDefinition.ReadAssembly("SwappingLibrary.dll");
        }

        public void swappMethods(Assembly assembly)
        {
            AssemblyDefinition tempAssembly = AssemblyDefinition.ReadAssembly("tempfile.dll");
            TypeDefinition tempClass = tempAssembly.MainModule.Types.FirstOrDefault(x => x.Name == "Container");

            MethodDefinition injectMethod = tempClass.Methods.FirstOrDefault(x => x.Name == "containerMethod");

            // Get the SwappingClass from the dll
            TypeDefinition swappingClass = this.swappingAssembly.MainModule.Types.FirstOrDefault(x => x.Name == "SwappingClass");

            // Get the swappingMethod from the SwappingLibrary
            MethodDefinition swappMethod = swappingClass.Methods.FirstOrDefault(x => x.Name == "swappingMethod");

            // Nun "injezieren" wir die Methode "Test()" in die Klasse Form1
            if (swappMethod != null && injectMethod != null)
            {
                var ilProcessor = swappMethod.Body.GetILProcessor();

                swappMethod.Body.Instructions.Clear();
                MethodReference methodReference = this.importMethod(injectMethod);
                Instruction inst = ilProcessor.Create(OpCodes.Call, methodReference);
                ilProcessor.Append(inst);

                // Write new library back to disk
                this.swappingAssembly.Write("SwappingLibrary.dll");
            }
        }

        private MethodReference importMethod(MethodDefinition injectMethod)
        {
            return this.swappingAssembly.MainModule.Import(injectMethod);
        }
    }
}
