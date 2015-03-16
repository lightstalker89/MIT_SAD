using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Hosting;
using System.Text;
using System.Windows.Forms;
using HotSwapping.Enumeration;
using HotSwapping.Helper;
using HotSwapping.Model;
using HotSwapping.MonoCecilClass;
using HotSwapping.PointerMethodClass;
using Mono.Cecil;
using SwappingLibrary;

namespace HotSwapping
{
    public partial class Form1 : Form
    {
        private readonly CodeCompileUtil codeCompileUtil;
        private readonly SwapContainer swapContainer;
        private readonly MonoSwapperUtil monoSwapperUtil;

        public Form1()
        {
            this.InitializeComponent();
            this.codeCompileUtil = new CodeCompileUtil();
            this.swapContainer = new SwapContainer();
            this.monoSwapperUtil = new MonoSwapperUtil();
        }

        #region pointer Method
        private void btnInject_Click(object sender, EventArgs e)
        {
            this.btnInject.Enabled = false;
            string code = this.txbCode.Text;
            CompileModel model = this.codeCompileUtil.compileCode(SwappingMethod.POINTER, code);
            if (model.result.Errors.HasErrors)
            {
                this.btnInject.Enabled = true;
                string errors = this.getErrors(model.result);
                this.txbError.Text = string.Empty;
                this.txbError.Text = errors;
                this.txbError.Visible = true;
            }
            else
            {
                this.hidePointerMethodErrors();
                this.swappMethod(model.assembly);
            }
        }

        private string getErrors(CompilerResults result)
        {
            StringBuilder sb = new StringBuilder();

            foreach (CompilerError error in result.Errors)
            {
                sb.AppendLine(string.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
            }
            return sb.ToString();
        }

        private void hidePointerMethodErrors()
        {
            this.txbError.Visible = false;
            this.txbError.Text = string.Empty;
        }

        private void hideMoniMethodErrors()
        {
            this.txbMonoError.Visible = false;
            this.txbMonoError.Text = string.Empty;
        }

        private void swappMethod(System.Reflection.Assembly assembly)
        {
            Type program = assembly.GetType("HotSwapping.Container");
            MethodBase swapMethod = program.GetMethod("containerMethod", BindingFlags.Instance | BindingFlags.Public);
            MethodBase originMethod = typeof(SwapContainer).GetMethod("swapMe", BindingFlags.Instance | BindingFlags.Public);

            MethodBase[] methods = new MethodBase[]
            {
                    swapMethod,
                    originMethod,
            };

            MethodPointerUtil.ReplaceMethod(methods[0], methods[1]);
            MessageBox.Show(@"Successfully swapped.");
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            this.btnInject.Enabled = true;
            Type type = typeof(SwapContainer);
            MethodInfo methodInfo = type.GetMethod("swapMe", Type.EmptyTypes);
            object instance = Activator.CreateInstance(type);
            methodInfo.Invoke(instance, null);

            SwapContainer test = new SwapContainer();
            test.swapMe();

            this.swapContainer.swapMe();
        }
        #endregion

        #region Mono cecil
        private void btnInjectCode_Click(object sender, EventArgs e)
        {
            this.btnInjectCode.Enabled = false;
            string code = this.txbMonoCode.Text;
            CompileModel model = this.codeCompileUtil.compileCode(SwappingMethod.MONO_CECIL, code);
            if (model.result.Errors.HasErrors)
            {
                this.btnInjectCode.Enabled = true;
                string errors = this.getErrors(model.result);
                this.txbMonoError.Text = string.Empty;
                this.txbMonoError.Text = errors;
                this.txbMonoError.Visible = true;
            }
            else
            {
                this.hideMoniMethodErrors();
                this.monoSwapperUtil.swappMethods(model.assembly);
                this.btnInjectCode.Enabled = true;
                MessageBox.Show(@"Successfully swapped.");
            }
        }
        #endregion

        private void btnExecuteCode_Click(object sender, EventArgs e)
        {
            // Creating a new appdomain
            AppDomainSetup setup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                ShadowCopyFiles = "true",
                // CachePath = @"C:\Users\Flo\Desktop\HotSwapping\HotSwapping\bin\Debug\Test",
                ShadowCopyDirectories = PathHelper.getAssemblyPath(),
            };
            AppDomain newDomain = AppDomain.CreateDomain("tempDomain", null, setup); // Create an instance of loader class in new appdomain
            var runner = (ShadowCopyWorker)newDomain.CreateInstanceAndUnwrap(typeof(ShadowCopyWorker).Assembly.FullName, typeof(ShadowCopyWorker).FullName);
            runner.doWorkWithShadow();
        }
    }
}
