using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web.Hosting;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.System;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class JobChangeController : BaseController
    {
        private readonly AutoNumberBll _autoNumberBll;
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly JobChangeBll _jobChangeBll;
        private readonly PositionBll _positionBll;
        private readonly string _prefix = "TCCT" + DateTime.Now.Year.ToString().Substring(2, 2);
        private readonly UserBll _userBll;

        public JobChangeController()
        {
            _jobChangeBll = SingletonIpl.GetInstance<JobChangeBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _positionBll = SingletonIpl.GetInstance<PositionBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            var users = _userBll.GetUsers(null);
            var employees = _employeeBll.GetEmployees(null);
            var departments = _departmentBll.GetDepartments(null);
            var positions = _positionBll.GetPositions();
            ViewBag.Users = users.Select(x => new KendoForeignKeyModel(x.FullName, x.UserId.ToString()));
            ViewBag.Positions = positions.Select(x => new KendoForeignKeyModel(x.PositionName, x.PositionId.ToString()));
            ViewBag.Departments =
                departments.Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            ViewBag.Employees =
                employees.Select(
                    x => new KendoForeignKeyModel((x.EmployeeCode + "-" + x.FullName), x.EmployeeId.ToString()));
            return View();
        }

        public JsonResult JobChanges()
        {
            var jobChanges = _jobChangeBll.GetJobChanges();
            return Json(jobChanges, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JobChange(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new JobChange
                {
                    JobChangeId = Guid.Empty.ToString(),
                    JobChangeCode = _autoNumberBll.GetAutoNumber(_prefix)
                });
            }
            var jobChange = _jobChangeBll.GetJobChange(id);
            return PartialView(jobChange);
        }

        public JsonResult DeleteJobChangeFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return Json(new {Status = 0, Message = "Không có file để xóa!"},
                        JsonRequestBehavior.AllowGet);
                }
                global::System.IO.File.Delete(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/" + filePath);
                return Json(new {Status = 1, Message = "Xóa file thành công!", Url = ""},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UploadJobChangeFile()
        {
            try
            {
                var path = HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Hrm/JobChangeFile/";
                var file = global::System.Web.HttpContext.Current.Request.Files["JobChangeFile"];
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload"))
                {
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload");
                    var dInfo = new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload");
                    var dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(
                        new FileSystemAccessRule(
                            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                            FileSystemRights.FullControl,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                }
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Hrm"))
                {
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Hrm");
                    var dInfo = new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Hrm");
                    var dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(
                        new FileSystemAccessRule(
                            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                            FileSystemRights.FullControl,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                }
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Hrm/JobChangeFile"))
                {
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Hrm/JobChangeFile");
                    var dInfo = new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Hrm/JobChangeFile");
                    var dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(
                        new FileSystemAccessRule(
                            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                            FileSystemRights.FullControl,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                }
                if (file == null || file.ContentLength == 0)
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
                }
                path += DateTime.Now.Ticks + "-" + file.FileName;
                file.SaveAs(path);
                return Json(new {Status = 1, Message = "Tải ảnh thành công!", Url = path}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Employee(long employeeId)
        {
            var employee = _employeeBll.GetEmployee(employeeId);
            if (employee == null)
                return Json(new Employee(), JsonRequestBehavior.AllowGet);
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(JobChangeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message =
                        ModelState.Where(modelState => modelState.Value.Errors.Count > 0)
                            .Select(x => x.Value)
                            .FirstOrDefault();
                    return
                        Json(
                            new
                            {
                                Status = 0,
                                Message =
                                    message == null ? MessageAction.ModelStateNotValid : message.Errors[0].ErrorMessage
                            },
                            JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.JobChangeId) || model.JobChangeId == Guid.Empty.ToString())
                {
                    model.CreateDate = DateTime.Now;
                    model.CreateBy = UserLogin.UserId;
                    model.JobChangeId = Guid.NewGuid().ToString();
                    model.JobChangeCode = _prefix;
                    var jobChangeCode = "";
                    if (_jobChangeBll.Insert(model.ToObject(), ref jobChangeCode))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_jobChangeBll.Update(model.ToObject()))
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                var jobChange = _jobChangeBll.GetJobChange(id);
                if (jobChange == null)
                {
                    return Json(new {Status = 0, Message = "Mã thuyên chuyển không tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (_jobChangeBll.Delete(jobChange))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageDeleteSuccess},
                        JsonRequestBehavior.AllowGet);
                }

                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}