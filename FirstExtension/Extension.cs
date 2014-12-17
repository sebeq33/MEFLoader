using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using MEFTest;

//Compilation output set to ..\MEFTest\bin\Debug\Extensions @see Project properties
namespace FirstExtension
{
    [Export(typeof(TestLibs.ILoadable))]
    public class Extension : TestLibs.ILoadable
    {
        private static string returnMyStr()
        {
            return ("Hello World");
        }

        public string getResult()
        {
            return ("Result");
        }

        [Export(typeof(TestLibs.DelegateToStr))] //Export a simple var containing a callable function (delegate)
        public TestLibs.DelegateToStr Func = returnMyStr;

    }
}
