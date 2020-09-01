using System.Collections.Generic;
using Core.Singleton;
using DataAccess.System;
using Entity.System;

namespace BusinessLogic.System
{
    public class UserGroupBll
    {
        private readonly UserGroupDal _userGroupDal;

        public UserGroupBll()
        {
            _userGroupDal = SingletonIpl.GetInstance<UserGroupDal>();
        }

        public List<UserGroup> GetUserGroups()
        {
            return _userGroupDal.GetUserGroups();
        }

        public UserGroup GetUserGroup(int userGroupId)
        {
            return _userGroupDal.GetUserGroup(userGroupId);
        }

        public UserGroup GetUserGroup(string userGroupCode)
        {
            return _userGroupDal.GetUserGroup(userGroupCode);
        }

        public List<UserGroupRights> GetUserGroupRightsAuthority(int userGroupId)
        {
            return _userGroupDal.GetUserGroupRightsAuthority(userGroupId);
        }

        public bool InsertUserGroupRight(int userGroupId, List<UserGroupRights> userGroupRights)
        {
            return _userGroupDal.InsertUserGroupRight(userGroupId, userGroupRights);
        }
        public long Insert(UserGroup obj)
        {
            return _userGroupDal.Insert(obj);
        }

        public bool Update(UserGroup obj)
        {
            return _userGroupDal.Update(obj);
        }

        public bool Delete(int userGroupId)
        {
            return _userGroupDal.Delete(userGroupId);
        }
    }
}