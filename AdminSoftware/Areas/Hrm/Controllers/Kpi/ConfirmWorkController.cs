using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.System.Models.Kpi;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;
using Entity.Kpi;
using Newtonsoft.Json;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class ConfirmWorkController : BaseController
    {
        private readonly AssignWorkBll _assignWorkBll;
        private readonly DepartmentBll _departmentBll;
        private readonly SuggesWorkBll _suggesWorkBll;
        private readonly WorkDetailBll _workDetailBll;
        private readonly WorkPlanDetailBll _workPlanDetailBll;
        private readonly WorkStreamDetailBll _workStreamDetailBll;

        public ConfirmWorkController()
        {
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _workStreamDetailBll = SingletonIpl.GetInstance<WorkStreamDetailBll>();
            _assignWorkBll = SingletonIpl.GetInstance<AssignWorkBll>();
            _suggesWorkBll = SingletonIpl.GetInstance<SuggesWorkBll>();
            _workPlanDetailBll = SingletonIpl.GetInstance<WorkPlanDetailBll>();
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
        }

        private List<WorkingNote> WorkingNotesInMemory
        {
            get
            {
                if (Session["WorkingNotesInMemory"] == null)
                    return new List<WorkingNote>();
                return (List<WorkingNote>) Session["WorkingNotesInMemory"];
            }
            set { Session["WorkingNotesInMemory"] = value; }
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.WorkDetailStatusEnum = from WorkDetailStatusEnum s in Enum.GetValues(typeof (WorkDetailStatusEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            ViewBag.StatusWorkDetail = from StatusWorkDetail s in Enum.GetValues(typeof (StatusWorkDetail))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            return View();
        }

        public JsonResult WorkDetails(int action, DateTime fromDate, DateTime toDate)
        {
            if (action == 3)
            {
                return Json(_workDetailBll.GetWorkDetails(UserLogin.UserId, 7, fromDate, toDate), JsonRequestBehavior.AllowGet);
            }
            if (action == 2)
            {
                return Json(_workDetailBll.GetWorkDetails(UserLogin.UserId, 2, fromDate, toDate), JsonRequestBehavior.AllowGet);
            }
            return Json(_workDetailBll.GetWorkDetails(UserLogin.UserId, 0, null, null), JsonRequestBehavior.AllowGet);
        }
        public ActionResult WorkDetail(string id, int workType)
        {
            WorkingNotesInMemory = null;
            var workDetail = _workDetailBll.GetWorkDetail(id, workType);
            if (workDetail.WorkingNote != null)
            {
                WorkingNotesInMemory = JsonConvert.DeserializeObject<List<WorkingNote>>(workDetail.WorkingNote);
            }
            return PartialView(workDetail);
        }
        public ActionResult Renewal(string id, int workType)
        {
            WorkingNotesInMemory = null;
               var workDetail = _workDetailBll.GetWorkDetail(id, workType);
            if (workDetail.WorkingNote != null)
            {
                WorkingNotesInMemory = JsonConvert.DeserializeObject<List<WorkingNote>>(workDetail.WorkingNote);
            }
            return PartialView(workDetail);
        }
        public JsonResult SaveRenewal(DateTime toDate, string id, int workType)
        {
            try
            {
                var workDetail = _workDetailBll.GetWorkDetail(id, workType);

                if (workDetail == null)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }

                var suggesWork = new SuggesWork();
                suggesWork.CreateBy = workDetail.CreateBy;
                suggesWork.SuggesWorkId = Guid.NewGuid().ToString();
                suggesWork.TaskId = workDetail.TaskId;
                suggesWork.FromDate = DateTime.Now;
                suggesWork.CreateDate = DateTime.Now;
                suggesWork.ToDate = toDate;
                suggesWork.CalcType = workDetail.CalcType;
                suggesWork.VerifiedBy = UserLogin.UserId;
                suggesWork.VerifiedDate = DateTime.Now;
                suggesWork.Quantity = workDetail.Quantity;
                suggesWork.WorkPointType = workDetail.WorkPointType;
                suggesWork.Status = 1;
                suggesWork.Description = "Công việc gia hạn  ngày kết thúc từ  : " +
                                         workDetail.ToDate.ToString("dd/MM/yyyy") + " Đến ngày " +
                                         toDate.ToString("dd/MM/yyyy")
                                          + workDetail.Description + ".";
                if (_suggesWorkBll.InsertTransection(suggesWork, workDetail))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult WorkingNotes()
        {
            return Json(WorkingNotesInMemory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkingNote(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new WorkingNote
                {
                    WorkingNoteId = Guid.Empty.ToString(),
                    CreateDate = DateTime.Now
                });
            }
            var workingNote = WorkingNotesInMemory.Find(
                x =>
                    x.WorkingNoteId ==
                    id);
            return PartialView(workingNote);
        }
        public JsonResult Save(WorkDetailModel model)
        {
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
            if (model == null)
            {
                return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
            }
            if (DateTime.Parse(model.ToDate.AddDays(+5).ToShortDateString()) < DateTime.Parse(DateTime.Now.ToShortDateString()))
            {
                return Json(new { Status = 0, Message = "Công việc đã quá hạn không thể cập nhật!   " }, JsonRequestBehavior.AllowGet);

            }var workDetail = _workDetailBll.GetWorkDetail(model.WorkDetailId, model.WorkType);
            if (workDetail == null)
            {
                return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
            }
            if (model.Status == 0)
            {
                model.Status = 1;
            }
            if (model.Status == 1  || model.Status == 2 || model.Status == 5)
            {
                workDetail.UsefulHours = null;
                workDetail.FisnishDate = null;
                workDetail.FileConfirm = "";
            }
            if (model.Status == 3)
            {
                workDetail.UsefulHours = model.UsefulHours;
                workDetail.FisnishDate = model.FisnishDate ?? DateTime.Now;
                workDetail.FileConfirm = model.FileConfirm;
            }
            if (model.Status == 5)
            {
                if (string.IsNullOrEmpty(model.Explanation))
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }
                var listExplanation = new List<Explanation>();

                if (workDetail.Explanation != null)
                {
                    listExplanation = JsonConvert.DeserializeObject<List<Explanation>>(workDetail.Explanation);
                }
                listExplanation.Add(new Explanation
                {
                    CreateDate = DateTime.Now,
                    ExplanationText = model.Explanation
                });
                workDetail.Explanation = JsonConvert.SerializeObject(listExplanation);
            }
            if (WorkingNotesInMemory != null)
            {
                workDetail.WorkingNote = JsonConvert.SerializeObject(WorkingNotesInMemory);
            }
            workDetail.Status = model.Status;
            workDetail.WorkType = model.WorkType;
            if (workDetail.WorkType == (int)StatusWorkDetail.WorkPlanDetail)
            {
                var workPlanDetail = _workPlanDetailBll.GetWorkPlanDetail(workDetail.WorkDetailId);
                workPlanDetail.UsefulHours = workDetail.UsefulHours;
                workPlanDetail.FisnishDate = workDetail.FisnishDate;
                workPlanDetail.Status = workDetail.Status;
                workPlanDetail.Explanation = workDetail.Explanation;
                workPlanDetail.WorkingNote = workDetail.WorkingNote;
                workPlanDetail.FileConfirm = workDetail.FileConfirm;
                if (_workPlanDetailBll.Update(workPlanDetail))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.AssignWork)
            {
                var assignWork = _assignWorkBll.GetAssignWork(workDetail.WorkDetailId);
                assignWork.UsefulHours = workDetail.UsefulHours;
                assignWork.FisnishDate = workDetail.FisnishDate;
                assignWork.Status = (byte)workDetail.Status;
                assignWork.Explanation = workDetail.Explanation;
                assignWork.WorkingNote = workDetail.WorkingNote;
                assignWork.FileConfirm = workDetail.FileConfirm;

                if (_assignWorkBll.Update(assignWork))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.WorkStreamDetail)
            {
                var workStreamDetail = _workStreamDetailBll.GetWorkStreamDetail(workDetail.WorkDetailId);
                workStreamDetail.UsefulHours = workDetail.UsefulHours;
                workStreamDetail.FisnishDate = workDetail.FisnishDate;
                workStreamDetail.Status = workDetail.Status;
                workStreamDetail.Explanation = workDetail.Explanation;
                workStreamDetail.WorkingNote = workDetail.WorkingNote;
                workStreamDetail.FileConfirm = workDetail.FileConfirm;

                if (_workStreamDetailBll.Update(workStreamDetail))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.SuggesWork)
            {
                var suggesWork = _suggesWorkBll.GetSuggesWork(workDetail.WorkDetailId);
                suggesWork.UsefulHours = workDetail.UsefulHours;
                suggesWork.FisnishDate = workDetail.FisnishDate;
                suggesWork.Status = (byte)workDetail.Status;
                suggesWork.Explanation = workDetail.Explanation;
                suggesWork.WorkingNote = workDetail.WorkingNote;
                suggesWork.FileConfirm = workDetail.FileConfirm;
                if (_suggesWorkBll.Update(suggesWork))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveWorkingNoteMemory(WorkingNote model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
                }
                var noteWorkings = WorkingNotesInMemory;
                var noteWorking =
                    noteWorkings.Find(
                        x =>
                            x.WorkingNoteId ==
                            model.WorkingNoteId);
                if (noteWorking == null)
                {
                    noteWorkings.Add(new WorkingNote
                    {
                        WorkingNoteId = Guid.NewGuid().ToString(),
                        CreateDate = model.CreateDate,
                        TextNote = model.TextNote
                    });
                }
                else
                {
                    noteWorking.TextNote = model.TextNote;
                }
                WorkingNotesInMemory = noteWorkings;
                return
                    Json(
                        new
                        {
                            Status = 1,
                            Message = "Cập nhật thực hiện công việc thành công!"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteWorkingNote(string id)
        {
            try
            {
                var noteWorkings = WorkingNotesInMemory;
                noteWorkings.Remove(
                    noteWorkings.Find(
                        x =>
                            x.WorkingNoteId ==
                            id));
                WorkingNotesInMemory = noteWorkings;
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
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Explanation()
        {
            return PartialView();
        }

        public JsonResult ClearSession()
        {
            WorkingNotesInMemory = null;
            return null;
        }

        //Upload File
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

        public JsonResult UploadFile()
        {
            try
            {
                var path = HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Kpi/WorkDetail/";
                var file = global::System.Web.HttpContext.Current.Request.Files["File"];
                string shortPath = "/Upload/Kpi/WorkDetail/";
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
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Kpi"))
                {
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Kpi");
                    var dInfo = new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Kpi");
                    var dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(
                        new FileSystemAccessRule(
                            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                            FileSystemRights.FullControl,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                }
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Hrm/WorkDetail"))
                {
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Hrm/WorkDetail");
                    var dInfo = new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Hrm/WorkDetail");
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
       
    }
}