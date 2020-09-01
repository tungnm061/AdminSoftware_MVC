using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Core.Security.Crypt;
using Dapper;
using Entity.System;

namespace DataAccess.System
{
    public class UserDal : BaseDal<ADOProvider>
    {
        public List<Rights> GetRights(int userId, int roleId, int? moduleGroupId)
        {
            try
            {
                return UnitOfWork.Procedure<Rights>("[dbo].[Get_UserRights]", new
                {
                    UserId = userId,
                    RoleId = roleId,
                    ModuleGroupId = moduleGroupId
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Rights>();
            }
        }

        public List<Rights> GetRightsAuthority(int userId, int moduleGroupId)
        {
            try
            {
                return UnitOfWork.Procedure<Rights>("[dbo].[Get_UserRights_Authority]", new
                {
                    UserId = userId,
                    ModuleGroupId = moduleGroupId
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Rights>();
            }
        }

        public User Login(string userName, string password)
        {
            try
            {
                var a = Md5Util.Md5EnCrypt(password);
                return
                    UnitOfWork.Procedure<User>("[dbo].[User_Login]",
                        new {UserName = userName, Password = a })
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public List<User> GetUsers(bool? isActive)
        {
            try
            {
                return UnitOfWork.Procedure<User>("[dbo].[Get_Users]", new {IsActive = isActive}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<User>();
            }
        }
        public List<User> GetUsersOfEmployee()
        {
            try
            {
                return UnitOfWork.Procedure<User>("[dbo].[Get_Users_Of_Employee]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<User>();
            }
        }

        public List<User> GetUserByDepartmentId(string path)
        {
            try
            {
                return
                    UnitOfWork.Procedure<User>("[dbo].[Get_UserByDepartmentId]", new { Path = path })
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<User>();
            }
        }

        public List<User> GetUserForCheckWorkPlan(DateTime date)
        {
            try
            {
                return UnitOfWork.Procedure<User>("[dbo].[Get_User_For_CheckWorkPlan]", new {Date = date}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<User>();
            }
        }

        public List<User> GetUsers(int userGroupId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<User>("[dbo].[Get_User_ByUserGroupId]", new {UserGroupId = userGroupId})
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<User>();
            }
        }

        public User GetUser(int userId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<User>("[dbo].[Get_User]", new {UserId = userId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public User GetUser(string userName)
        {
            try
            {
                return
                    UnitOfWork.Procedure<User>("[dbo].[Get_User_ByUserName]", new {UserName = userName})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public User GetUserByEmployeeId(long employeeId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<User>("[dbo].[Get_User_ByEmployeeId]", new {EmployeeId = employeeId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Update(User user)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", user.UserId);
                param.Add("@UserName", user.UserName);
                param.Add("@Email", user.Email);
                param.Add("@EmployeeId", user.EmployeeId);
                param.Add("@FullName", user.FullName);
                param.Add("@IsActive", user.IsActive);
                param.Add("@ModuleGroupId", user.ModuleGroupId);
                param.Add("@PhoneNumber", user.PhoneNumber);
                param.Add("@RoleId", user.RoleId);
                param.Add("@UserGroupId", user.UserGroupId);
                return UnitOfWork.ProcedureExecute("[dbo].[Update_User]", param);
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
                var param = new DynamicParameters();
                param.Add("@UserId", user.UserId, DbType.Int32, ParameterDirection.InputOutput);
                param.Add("@UserName", user.UserName);
                param.Add("@CreateDate", user.CreateDate);
                param.Add("@Email", user.Email);
                param.Add("@EmployeeId", user.EmployeeId);
                param.Add("@FullName", user.FullName);
                param.Add("@IsActive", user.IsActive);
                param.Add("@ModuleGroupId", user.ModuleGroupId);
                param.Add("@Password", Md5Util.Md5EnCrypt(user.Password));
                param.Add("@PhoneNumber", user.PhoneNumber);
                param.Add("@RoleId", user.RoleId);
                param.Add("@UserGroupId", user.UserGroupId);
                if (UnitOfWork.ProcedureExecute("[dbo].[Insert_User]", param))
                    return param.Get<int>("@UserId");
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
            try
            {
                return UnitOfWork.ProcedureExecute("[dbo].[Insert_UserRights]", new
                {
                    UserId = userId,
                    XML = XmlHelper.SerializeXml<List<Rights>>(rightses)
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool UpdatePassword(int userId, string password)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[dbo].[Update_Password]",
                    new {UserId = userId, Password = Md5Util.Md5EnCrypt(password)});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}