using DAL.Repository;
using Interfaces.Repository;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoschool.Utils.NinjectModules
{
    public class ReposModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbRepos>().To<DbReposSQL>();    
        }
    }
}
