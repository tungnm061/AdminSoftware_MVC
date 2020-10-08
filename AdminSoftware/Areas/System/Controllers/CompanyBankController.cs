using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web.Hosting;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using DuyAmazone.Areas.Printify.Models;
using Entity.System;
using Newtonsoft.Json;

namespace AdminSoftware.Areas.System.Controllers
{
    public class CompanyBankController : BaseController
    {

        private readonly CompanyBankBll _companyBankBll;
        private readonly UserBll _userBll;
        private readonly ExpenseTypeBll _expenseTypeBll;
        //private readonly DepartmentBll _departmentBll;

        public CompanyBankController()
        {
            _expenseTypeBll = SingletonIpl.GetInstance<ExpenseTypeBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _companyBankBll = SingletonIpl.GetInstance<CompanyBankBll>();
            //_departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }

        private List<TextNote> TextNotesInMemory
        {
            get
            {
                if (Session["TextNotesInMemory"] == null)
                    return new List<TextNote>();
                return (List<TextNote>)Session["TextNotesInMemory"];
            }
            set { Session["TextNotesInMemory"] = value; }
        }

        // GET: Printify/CompanyBank
        [SystemFilter]
        public ActionResult Index()
        {
            var users = _userBll.GetUsers(null);
            var expenseTypes = _expenseTypeBll.GetExpenseTypes(true);
            var expenseTypesAll = _expenseTypeBll.GetExpenseTypes(null);
            ViewBag.ExpenseTypesAll = expenseTypesAll.Select(x => new KendoForeignKeyModel { value = x.ExpenseId.ToString(), text = x.ExpenseName });

            //var departments = _departmentBll.GetDepartments(0);
            ViewBag.Users = users.Select(x => new KendoForeignKeyModel { value = x.UserId.ToString(), text = x.FullName });
            ViewBag.ExpenseTypes = expenseTypes.Select(x => new KendoForeignKeyModel { value = x.ExpenseId.ToString(), text = x.ExpenseName });
            //ViewBag.Departments = departments.Select(x => new KendoForeignKeyModel { value = x.Path.ToString(), text = x.DepartmentName }); ;
            ViewBag.TypeMoneys = from TypeMoneyEnum s in Enum.GetValues(typeof(TypeMoneyEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                    s.GetType()
                        .GetField(s.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            ViewBag.StatusSearch = from StatusCompanyBankEnum s in Enum.GetValues(typeof(StatusCompanyBankEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                    s.GetType()
                        .GetField(s.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            return View();
        }

        public ActionResult CompanyBanks(DateTime? fromDate,DateTime? toDate,int? expenseId,int? statusSearch)
        {
            var listObj = _companyBankBll.GetCompanyBanks(true, fromDate,toDate,expenseId, statusSearch);
            return Json(listObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompanyBank(int id)
        {
            TextNotesInMemory = null;
            if (id == 0)
            {
                return PartialView(new CompanyBank
                {
                    CompanyBankId = 0,
                    IsActive = true,
                    TypeMonney = 1,
                    TradingBy = UserLogin.UserId,
                    TradingDate = DateTime.Now,
                    Status =  1
                });
            }

            var obj = _companyBankBll.GetCompanyBank(id);
            if (!string.IsNullOrEmpty(obj.TextNote))
            {
                TextNotesInMemory = JsonConvert.DeserializeObject<List<TextNote>>(obj.TextNote);
            }
            return PartialView(_companyBankBll.GetCompanyBank(id));
        }

        [HttpPost]
        public JsonResult Save(CompanyBank model)
        {
            try
            {
                if (model == null)
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
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

                if (model.Status == 3)
                {
                    return Json(new { Status = -1, Message = MessageAction.DuLieuKhongTheUpdate },
                        JsonRequestBehavior.AllowGet);
                }

               
                if (TextNotesInMemory != null)
                {
                    model.TextNote = JsonConvert.SerializeObject(TextNotesInMemory);
                }
                if (model.CompanyBankId == 0)
                {
                    //model.Path = UserLogin.Path;
                    //if (string.IsNullOrEmpty(model.Path))
                    //{
                    //    return Json(new { Status = -1, Message = MessageAction.TaiKhoanKhongCoQuyenTao },
                    //        JsonRequestBehavior.AllowGet);
                    //}
                    model.CreateBy = UserLogin.UserId;
                    model.CreateDate = DateTime.Now;
                    model.IsActive = true;
                    model.Status = 1;
                    if (_companyBankBll.Insert(model) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    model.UpdateBy = UserLogin.UserId;
                    model.UpdateDate = DateTime.Now;
                    model.IsActive = true;
                    if (_companyBankBll.Update(model))
                        return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                            JsonRequestBehavior.AllowGet);
                }

                return Json(new { Status = -1, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_companyBankBll.Delete(int.Parse(id)))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageDeleteSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult TextNotes()
        {
            return Json(TextNotesInMemory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TextNote(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new TextNote
                {
                    TextNoteId = Guid.Empty.ToString(),
                    CreateDate = DateTime.Now
                });
            }
            var obj = TextNotesInMemory.Find(
                x =>
                    x.TextNoteId ==
                    id);
            return PartialView(obj);
        }

        public JsonResult SaveTextNoteMemory(TextNote model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }
                var noteWorkings = TextNotesInMemory;
                var noteWorking =
                    noteWorkings.Find(
                        x =>
                            x.TextNoteId ==
                            model.TextNoteId);
                if (noteWorking == null)
                {
                    noteWorkings.Add(new TextNote
                    {
                        TextNoteId = Guid.NewGuid().ToString(),
                        CreateDate = model.CreateDate,
                        CreateBy = UserLogin.UserId,
                        Text = model.Text
                    });
                }
                else
                {
                    noteWorking.Text = model.Text;
                }
                TextNotesInMemory = noteWorkings;
                return
                    Json(
                        new
                        {
                            Status = 1,
                            Message = "Cập nhật ghi chú thành công!"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteTextNote(string id)
        {
            try
            {
                var noteWorkings = TextNotesInMemory;
                noteWorkings.Remove(
                    noteWorkings.Find(
                        x =>
                            x.TextNoteId ==
                            id));
                TextNotesInMemory = noteWorkings;
                return
                    Json(
                        new
                        {
                            Status = 1,
                            Message = "Xóa chi tiết công việc thành công!"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult UploadFile()
        {
            try
            {
                var path = HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/CompanyBank/File/";
                var file = global::System.Web.HttpContext.Current.Request.Files["File"];
                string shortPath = "/Upload/CompanyBank/File/";
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
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/CompanyBank"))
                {
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/CompanyBank");
                    var dInfo = new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/CompanyBank");
                    var dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(
                        new FileSystemAccessRule(
                            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                            FileSystemRights.FullControl,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                }
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/CompanyBank/File"))
                {
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/CompanyBank/File");
                    var dInfo = new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/CompanyBank/File");
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
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = DateTime.Now.Ticks + "-" + file.FileName;
                path += fileName;
                shortPath += fileName;
                file.SaveAs(path);
                return Json(new { Status = 1, Message = "Tải file thành công!", Url = shortPath }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteFile(string filePath)
        {
            try
            {
                if (filePath == null)
                {
                    return Json(new { Status = 0, Message = "File không tồn tại!" },
                        JsonRequestBehavior.AllowGet);
                }

                global::System.IO.File.Delete(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/" + filePath);
                return Json(new { Status = 1, Message = "Xóa file thành công!" },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ClearSession()
        {
            return null;
        }


    }
}