using System.Collections.Generic;
using Core.Singleton;
using DataAccess.System;
using Entity.System;

namespace BusinessLogic.System
{
    public class ModuleGroupBll
    {
        private readonly ModuleGroupDal _moduleGroupDal;

        public ModuleGroupBll()
        {
            _moduleGroupDal = SingletonIpl.GetInstance<ModuleGroupDal>();
        }

        public List<ModuleGroup> GetModuleGroups()
        {
            return _moduleGroupDal.GetModuleGroups();
        }

        public List<ModuleGroup> GetModuleGroups(int userId, int roleId)
        {
            return _moduleGroupDal.GetModuleGroups(userId, roleId);
        }
    }
}