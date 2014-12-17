using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //for openFileDialog
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace MEFTest
{
    /**
     * Tool managing plugin dependencies at runtime and dynamicly for any type, no metadata
     * Use MEF (System.ComponentModel.Composition)
     * @typeparam name="T" type of the dependencies to load (Lazy), the exported onces
     * @author Sebastien Bequignon (bequignon.sebastien@gmail.com)
     */
    public class LibsLoader<T> : IDisposable
    {
        [ImportMany(AllowRecomposition = true)] //Loaded enumerable of any exported T type in DLLs and updatable (recomposable)
        private IEnumerable<Lazy<T>> _loaded = null;
        public IEnumerable<Lazy<T>> Loaded
        {
            get { return (this._loaded); }
            //readonly set
        }

        private CompositionContainer _container;

        /* 
         * Constructor
         * @param name="preload" bool default = false, load all DLLs in Defaults directory "./Extensions"
         */
        public LibsLoader(bool preload = false)
        {
            AggregateCatalog catalog;

            //An aggregate catalog that combines multiple catalogs
            catalog = new AggregateCatalog();
            if (preload == true)
                catalog.Catalogs.Add(new DirectoryCatalog("Extensions")); //import all dll in directory /Extensions

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        public bool LoadDLL(string path)
        {
            if (this.LoadDLL(new AssemblyCatalog(path)))
            {
                Console.WriteLine("New DLL Loaded :" + path);
                return (true);
            }
            return (false);
        }

        public bool LoadDLL(ComposablePartCatalog part)
        {
            //Recompose the imports of this object
            try
            {
                ((AggregateCatalog)(this._container.Catalog)).Catalogs.Add(part); //Need AllowRecomposition = true in Import
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
                return (false);
            }
            return (true);
        }

        /**
         * Open a FileDialog to select one or plural DLL and return fullpath of each selected libs
         */
        public string[] ShowDialogDLL()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "DLL files (*.dll)|*.dll";
            //fd.FilterIndex = 2; //default combobox index
            fd.Multiselect = true;
            fd.RestoreDirectory = true; //http://stackoverflow.com/questions/4353487/what-does-the-filedialog-restoredirectory-property-actually-do


            if (fd.ShowDialog() == DialogResult.OK) //NEED STAThread in main() for debug @see http://stackoverflow.com/questions/1361033/what-does-stathread-do
            {
                return fd.FileNames;
            }
            return null;
        }

        public IEnumerable<Lazy<T>> GetLoaded()
        {
            return (this._loaded);
        }

        public void Clear()
        {
            ((AggregateCatalog)(this._container.Catalog)).Catalogs.Clear();
        }

        public void Dispose()
        {
            _loaded = null;
            _container.Dispose();
        }
    }
}
