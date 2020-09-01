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
    public class ContractController : BaseController
    {
        private readonly ContractBll _contractBll;
        private readonly ContractTypeBll _contractTypeBll;
        private readonly EmployeeBll _employeeBll;
        private readonly UserBll _userBll;

        public ContractController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _contractTypeBll = SingletonIpl.GetInstance<ContractTypeBll>();
            _contractBll = SingletonIpl.GetInstance<ContractBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            var users = _userBll.GetUsers(null);
            var employees = _employeeBll.GetEmployees(null);
            var contractTypes = _contractTypeBll.GetContractTypes(null);
            ViewBag.Users = users.Select(x => new KendoForeignKeyModel {value = x.UserId.ToString(), text = x.FullName});
            ViewBag.Employees =
                employees.Select(
                    x =>
                        new KendoForeignKeyModel
                        {
                            value = x.EmployeeId.ToString(),
                            text = (x.EmployeeCode + "-" + x.FullName)
                        });
            ViewBag.ContractTypes =
                contractTypes.Select(
                    x => new KendoForeignKeyModel {value = x.ContractTypeId.ToString(), text = x.TypeName});
            return View();
        }

        public ActionResult Contracts()
        {
            var contracts = _contractBll.GetContracts();
            return Json(contracts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contract(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new Contract
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddYears(1),
                    ContractId = Guid.Empty.ToString()
                });
            }
            return PartialView(_contractBll.GetContract(id));
        }

        [HttpPost]
        public JsonResult Save(ContractModel model)
        {
            try
            {
                if (model == null)
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                if (!ModelState.IsValid)
                {
                    var message = ModelState.Where(modelState => modelState.Value.Errors.Count > 0)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                    return Json(new
                    {
                        Status = 0,
                        Message = message == null ? MessageAction.ModelStateNotValid : message.Errors[0].ErrorMessage
                    }, JsonRequestBehavior.AllowGet);
                }
                var contract = _contractBll.GetContractByContractCode(model.ContractCode);
                if (string.IsNullOrEmpty(model.ContractId) || model.ContractId == Guid.Empty.ToString())
                {
                    if (contract != null)
                    {
                        return Json(new {Status = 0, Message = "Số hợp đồng đã tồn tại trong hệ thống!"},
                            JsonRequestBehavior.AllowGet);
                    }
                    model.CreateDate = DateTime.Now;
                    model.CreateBy = UserLogin.UserId;
                    model.ContractId = Guid.NewGuid().ToString();
                    if (_contractBll.Insert(model.ToObject()))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (contract != null && contract.ContractId != model.ContractId)
                {
                    return Json(new {Status = 0, Message = "Số hợp đồng đã tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_contractBll.Update(model.ToObject()))
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
                var contract = _contractBll.GetContract(id);
                if (contract == null)
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                if (_contractBll.Delete(id))
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


        public JsonResult DeleteFile(string filePath)
        {
            try
            {
                global::System.IO.File.Delete(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/" + filePath);
                return Json(new {Status = 1, Message = "Xóa file thành công!"},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UploadFile()
        {
            try
            {
                var path = HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Contract/" +
                           DateTime.Now.Year;
                var file = global::System.Web.HttpContext.Current.Request.Files["File"];
                string shortPath = "/Upload/Contract/";
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
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Contract"))
                {
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Contract");
                    var dInfo = new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Contract");
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
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = DateTime.Now.Ticks + "-" + file.FileName;
                path += fileName;
                shortPath += fileName;
                file.SaveAs(path);
                return Json(new {Status = 1, Message = "Tải file thành công!", Url = shortPath }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ClearSession()
        {
            return null;
        }

    }
}