using MVCCore.Helpers;
using MVCCore.Repositories;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Helpers
{   
    public class ModuleRepository : IModuleRepository
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public ModuleRepository(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
            this.totalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<Module> GetAllModules()
        {
            return this.totalBikePortalsEntities.Modules.Where(w => w.InActive == 0);
        }

        public Module GetModuleByID(int moduleID)
        {
            var module = this.totalBikePortalsEntities.Modules.SingleOrDefault(x => x.ModuleID == moduleID);
            return module;
        }

  

        public void SaveChanges()
        {
            this.totalBikePortalsEntities.SaveChanges();
        }

        public void Add(Module module)
        {
            this.totalBikePortalsEntities.Modules.Add(module);
        }

        public void Delete(Module module)
        {
            this.totalBikePortalsEntities.Modules.Remove(module);
        }
    }
}
