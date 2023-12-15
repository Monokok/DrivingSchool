using Autoschool.ViewModel;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoschool.Utils.NinjectModules
{
    public class VMModule : NinjectModule
    {
        public override void Load()
        {
            Bind<AppMainViewModel>().ToSelf();
        }
    }
}
