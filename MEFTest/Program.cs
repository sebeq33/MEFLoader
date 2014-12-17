//@author Sebastien Bequignon (bequignon.sebastien@gmail.com)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Forms; //for openFileDialog
using System.ComponentModel.Composition.Primitives;

//src:
//http://msdn.microsoft.com/en-us/library/dd460648.aspx
//http://www.c-sharpcorner.com/UploadFile/cb88b2/getting-started-with-mef-to-load-wpf-user-controls-from-dll/
//http://marlongrech.wordpress.com/2010/05/23/mefedmvvm-v1-0-explained/
//http://laedit.developpez.com/CSharp/MEF/Programming_Guide/?page=page_2

namespace MEFTest
{
    public class TestLibs
    {
        public delegate string DelegateToStr(); //Could Be Any Types (Droid =o, Media, UserControl ...etc), "delegate" (+-)= to function pointers
        public interface ILoadable //Simple object interface
        {
            string getResult();
        }

        static private void test1()
        {
            LibsLoader<DelegateToStr> loader = new LibsLoader<DelegateToStr>(true);
            String[] paths;

            //loader.Clear();
            if ((paths = loader.ShowDialogDLL()) != null)
            {
                foreach (string path in paths)
                    loader.LoadDLL(path);
            }

            IEnumerable<Lazy<DelegateToStr>> loaded = loader.GetLoaded();
            if (loaded != null && loaded.Count() > 0)
            {
                Console.WriteLine("Items:");
                foreach (Lazy<DelegateToStr> str in loaded)
                    Console.WriteLine(str.Value()); //invoke delegate
            }
        }

        static private void test2()
        {
            LibsLoader<ILoadable> loader = new LibsLoader<ILoadable>(true);
            String[] paths;

            if ((paths = loader.ShowDialogDLL()) != null)
            {
                foreach (string path in paths)
                    loader.LoadDLL(path);
            }

            IEnumerable<Lazy<ILoadable>> loaded = loader.GetLoaded();
            if (loaded != null && loaded.Count() > 0)
            {
                Console.WriteLine("Items:");
                foreach (Lazy<ILoadable> load in loaded)
                    Console.WriteLine(load.Value.getResult());
            }
        }

        static public void start()
        {

            Console.WriteLine("Test 1\n------------------------------");
            test1();
            Console.WriteLine();

            Console.WriteLine("Test 2\n------------------------------");
            test2();
        }
    }

    class Program
    {
     
        [STAThread]
        static void Main(string[] args)
        {
            TestLibs.start();
            Console.ReadKey(); //Wait for user =P
        }
    }
}
