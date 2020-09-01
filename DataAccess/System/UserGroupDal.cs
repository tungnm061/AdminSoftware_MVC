using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Dapper;
using Entity.System;

namespace DataAccess.System
{
    public class UserGroupDal : BaseDal<ADOProvider>
    {
        public List<UserGroup> GetUserGroups()
        {
            try
            {
                return UnitOfWork.Procedure<UserGroup>("[dbo].[Get_UserGroups]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<UserGroup>();
            }
        }

        public UserGroup GetUserGroup(int userGroupId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<UserGroup>("dbo.Get_UserGroup", new {UserGroupId = userGroupId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public UserGroup GetUserGroup(string groupCode)
        {
            try
            {
                return
                    UnitOfWork.Procedure<UserGroup>("[dbo].[Get_UserGroup_ByGroupCode]", new {GroupCode = groupCode})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(UserGroup userGroup)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserGroupId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@GroupName", userGroup.GroupName);
                param.Add("@Description", userGroup.Description);
                param.Add("@GroupCode", userGroup.GroupCode);
                if (UnitOfWork.ProcedureExecute("[dbo].[Insert_UserGroup]", param))
                    return param.Get<int>("@UserGroupId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(UserGroup userGroup)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserGroupId", userGroup.UserGroupId);
                param.Add("@GroupName", userGroup.GroupName);
                param.Add("@Description", userGroup.Description);
                param.Add("@GroupCode", userGroup.GroupCode);
                return UnitOfWork.ProcedureExecute("[dbo].[Update_UserGroup]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int userGroupId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[dbo].[Delete_UserGroup]", new {UserGroupId = userGroupId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public List<UserGroupRights> GetUserGroupRightsAuthority(int userGroupId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<UserGroupRights>("[dbo].[Get_UserGroup_Rights_Authority]",
                        new {UserGroupId = userGroupId}).ToList();
            }
            catch (Exception)
            {
                return new List<UserGroupRights>();
            }
        }

        public List<UserGroupRights> GetUserGroupRights(int userGroupId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<UserGroupRights>("[dbo].[Get_UserGroup_Rights]",
                        new {UserGroupId = userGroupId}).ToList();
            }
            catch (Exception)
            {
                return new List<UserGroupRights>();
            }
        }

        public bool InsertUserGroupRight(int userGroupId, List<UserGroupRights> userGroupRights)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[dbo].[Insert_UserGroupRights]", new
                {
                    UserGroupId = userGroupId,
                    XML = XmlHelper.SerializeXml<List<UserGroupRights>>(userGroupRights)
                });
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}