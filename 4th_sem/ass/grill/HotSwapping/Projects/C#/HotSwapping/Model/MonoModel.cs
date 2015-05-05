using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace HotSwapping.Model
{
    public class MonoModel
    {
        public MonoModel(MethodDefinition methodDefinition, Instruction instruction, MethodReference methodReference)
        {
            this.MethodDefinition = methodDefinition;
            this.Instruction = instruction;
            this.MethodReference = methodReference;
        }

        public MethodDefinition MethodDefinition { get; private set; }

        public Instruction Instruction { get; private set; }

        public MethodReference MethodReference { get; private set; }
    }
}
