using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.System;

namespace DataAccess.System
{
    public class ModuleGroupDal : BaseDal<ADOProvider>
    {
        public List<ModuleGroup> GetModuleGroups()
        {
            try
            {
                return UnitOfWork.Procedure<ModuleGroup>("[dbo].[Get_ModuleGroups]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<ModuleGroup>();
            }
        }

        public List<ModuleGroup> GetModuleGroups(int userId, int roleId)
        {
            try
            {
                return UnitOfWork.Procedure<ModuleGroup>("[dbo].[Get_ModuleGroup_ByUserId]", new
                {
                    UserId = userId,
                    RoleId = roleId
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<ModuleGroup>();
            }
        }
    }
}