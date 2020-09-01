using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using DataAccess.System;
using Entity.System;

namespace BusinessLogic.System
{
    public class UserBll
    {
        private readonly UserDal _userDal;
        private readonly UserGroupDal _userGroupDal;

        public UserBll()
        {
            _userDal = SingletonIpl.GetInstance<UserDal>();
            _userGroupDal = SingletonIpl.GetInstance<UserGroupDal>();
        }

        public List<User> GetUserByDepartmentId(string path)
        {
            return _userDal.GetUserByDepartmentId(path);
        }

        public List<User> GetUserForCheckWorkPlan(DateTime date)
        {
            return _userDal.GetUserForCheckWorkPlan(date);
        }

        public List<Rights> GetRights(int userId, int roleId, int? moduleGroupId)
        {
            return _userDal.GetRights(userId, roleId, null);
        }

        public List<Rights> GetRightsAuthority(int userId, int moduleGroupId)
        {
            return _userDal.GetRightsAuthority(userId, moduleGroupId);
        }

        public User Login(string userName, string password)
        {
            return _userDal.Login(userName, password);
        }

        public List<User> GetUsers(bool? isActive)
        {
            return _userDal.GetUsers(isActive);
        }

        public List<User> GetUsersOfEmployee()
        {
            return _userDal.GetUsersOfEmployee();
        }

        public List<User> GetUsers(int userGroupId)
        {
            return _userDal.GetUsers(userGroupId);
        }

        public User GetUser(int userId)
        {
            return _userDal.GetUser(userId);
        }

        public User GetUser(string userName)
        {
            return _userDal.GetUser(userName);
        }

        public User GetUserByEmployeeId(long employeeId)
        {
            return _userDal.GetUserByEmployeeId(employeeId);
        }

        public bool Update(User user)
        {
            try
            {
                if (user.RoleId == (int) Role.Admin)
                {
                    if (_userDal.Update(user))
                    {
                        return true;
                    }
                }
                var userGroupRights = _userGroupDal.GetUserGroupRightsAuthority(user.UserGroupId ?? 0);
                if (userGroupRights != null)
                {
                    var righs = userGroupRights.Select(x => new Rights
                    {
                        FunctionId = x.FunctionId,
                        UserId = user.UserId,
                        IsView = x.IsView,
                        IsCreate = x.IsCreate,
                        IsDelete = x.IsDelete,
                        IsEdit = x.IsEdit
                    })
                        .ToList();
                    using (var scope = new TransactionScope())
                    {
                        if (_userDal.UpdateRights(user.UserId, righs))
                        {
                            if (_userDal.Update(user))
                            {
                                scope.Complete();
                                return true;
                            }
                            scope.Dispose();
                            return false;
                        }
                        scope.Dispose();
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public int Insert(User user)
        {
            try
            {
                if (user.RoleId == (int) Role.Admin)
                {
                    return _userDal.Insert(user);
                }
                var userGroupRights = _userGroupDal.GetUserGroupRightsAuthority(user.UserGroupId ?? 0);
                if (userGroupRights != null)
                {
                    var righs = userGroupRights.Select(x => new Rights
                    {
                        FunctionId = x.FunctionId,
                        UserId = user.UserId,
                        IsView = x.IsView,
                        IsCreate = x.IsCreate,
                        IsDelete = x.IsDelete,
                        IsEdit = x.IsEdit
                    })
                        .ToList();
                    using (var scope = new TransactionScope())
                    {
                        var id = _userDal.Insert(user);
                        if (id > 0)
                        {
                            if (_userDal.UpdateRights(id, righs))
                            {
                                scope.Complete();
                                return id;
                            }
                            scope.Dispose();
                            return 0;
                        }
                        scope.Dispose();
                        return 0;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool UpdateRights(int userId, List<Rights> rightses)
        {
            return _userDal.UpdateRights(userId, rightses);
        }

        public bool UpdatePassword(int userId, string password)
        {
            return _userDal.UpdatePassword(userId, password);
        }
    }
}