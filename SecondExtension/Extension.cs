using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using MEFTest;

//Compilation output set to ..\MEFTest\bin\Debug\Extensions @see Project properties
namespace SecondExtension
{
    [Export(typeof(TestLibs.ILoadable))]
    public class Extension : TestLibs.ILoadable
    {
        [Export(typeof(TestLibs.DelegateToStr))] //Export a function directly as delegate content
        private static string returnMyStr()
        {
            return ("Hello World 2");
        }

        public string getResult()
        {
            return ("Result 2");
        }
    }
}
