using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MVCModel.Models;

namespace MVCCore.Helpers
{
    public interface IModuleRepository
    {
        IQueryable<Module> GetAllModules();
        Module GetModuleByID(int moduleID);                

        void SaveChanges();
        void Add(Module module);
        void Delete(Module module);
    }
}
