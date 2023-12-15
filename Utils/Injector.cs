using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autoschool.Utils.NinjectModules;

namespace Autoschool.Utils
{
    public static class Injector
    {
        public static IKernel Kernel { get; private set; }
        static Injector()
        {
            Kernel = new StandardKernel(new ServicesModule(), new VMModule(), new ReposModule());
        }
    }
}
