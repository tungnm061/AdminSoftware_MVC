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
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;
using Entity.Kpi;
using Newtonsoft.Json;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AssignWorkBll _assignWorkBll;
        private readonly CategoryKpiBll _categoryKpiBll;
        private readonly ComplainBll _complainBll;
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly HolidayBll _holidayBll;
        private readonly RightsBll _rightsBll;
        private readonly SuggesWorkBll _suggesWorkBll;
        private readonly TaskBll _taskBll;
        private readonly UserBll _userBll;
        private readonly WorkDetailBll _workDetailBll;
        private readonly WorkPlanBll _workPlanBll;
        private readonly WorkPlanDetailBll _workPlanDetailBll;
        private readonly WorkStreamDetailBll _workStreamDetailBll;
        private readonly FactorConfigBll _factorConfigBll;

        public HomeController()
        {
            _holidayBll = SingletonIpl.GetInstance<HolidayBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _workPlanBll = SingletonIpl.GetInstance<WorkPlanBll>();
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
            _rightsBll = SingletonIpl.GetInstance<RightsBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _suggesWorkBll = SingletonIpl.GetInstance<SuggesWorkBll>();
            _workStreamDetailBll = SingletonIpl.GetInstance<WorkStreamDetailBll>();
            _assignWorkBll = SingletonIpl.GetInstance<AssignWorkBll>();
            _workPlanDetailBll = SingletonIpl.GetInstance<WorkPlanDetailBll>();
            _taskBll = SingletonIpl.GetInstance<TaskBll>();
            _complainBll = SingletonIpl.GetInstance<ComplainBll>();
            _categoryKpiBll = SingletonIpl.GetInstance<CategoryKpiBll>();
            _factorConfigBll = SingletonIpl.GetInstance<FactorConfigBll>();
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

            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var toDate = DateTime.Now;
            var factorWork = _workDetailBll.GetFactorWorkKpi(fromDate, toDate, UserLogin.EmployeeId??0, toDate.Day)??new StatisticalFactorWork();
            var holidayNumbers = _holidayBll.GetHolidayByDates(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                DateTime.Now).Count();
            factorWork.UsefullHourMinReal = ((DateTime.Now.Day - holidayNumbers) * (7.4)).ToString("n1");
            factorWork.NumberEmployee = 1;

            //
            var factorWorks = _workDetailBll.GetFactorWorkKpisNew(fromDate, toDate, null, toDate.Day);
            var employees = new List<StatisticalFactorWork>();
            if (factorWork.DepartmentCompany == 1)
            {
                employees = factorWorks.Where(x => x.DepartmentCompany != 1).ToList();
            }
            if (factorWork.DepartmentCompany == 2)
            {
                employees =
                    factorWorks.Where(
                        x =>
                            x.DepartmentId == factorWork.DepartmentId && x.DepartmentCompany != 2 &&
                            x.DepartmentCompany != 1).ToList();
            }
            if (factorWork.DepartmentCompany == 3)
            {
                employees =
                    factorWorks.Where(
                        x =>
                            x.DepartmentId == factorWork.DepartmentId && x.DepartmentCompany != 2 &&
                            x.DepartmentCompany != 3 && x.DepartmentCompany != 1).ToList();
            }
            if (factorWork.DepartmentCompany == 4)
            {
                employees =
                    factorWorks.Where(x => x.DepartmentId == factorWork.DepartmentId && x.DepartmentCompany == 5)
                        .ToList();
            }
            if (factorWork.DepartmentCompany != 5)
            {
                var numberEmployees = employees.Count();
                factorWork.UsefulHoursTask =
                    employees.Sum(y => y.UsefulHoursTask);
                factorWork.UsefulHoursSuggesWork =
                    employees.Sum(y => y.UsefulHoursSuggesWork);
                factorWork.UsefullHourMin = factorWork.UsefullHourMin * numberEmployees;
                factorWork.NumberEmployee = numberEmployees;
                factorWork.UsefullHourMinReal = factorWork.UsefullHourMin.ToString("n1");
                var factorConfigs = _factorConfigBll.GetFactorConfigs();
                var factorConfig =
                    factorConfigs.Where(
                        x => x.FactorConditionMin <= factorWork.AvgPoint && x.FactorConditionMax > factorWork.AvgPoint)
                        .FirstOrDefault();
                if (factorConfig != null)
                {
                    factorWork.FactorPoint = factorConfig.FactorPointMax;
                    factorWork.FactorType = factorConfig.FactorType;
                }
                if (factorWork.AvgPoint <= 0)
                {
                    factorWork.FactorPoint = 0;
                    factorWork.FactorType = "";
                }
            }
            ViewBag.Factor = factorWork;
            //
            ViewBag.ActionCompany = _userBll.GetUser(UserLogin.UserId).DepartmentCompany;
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

        public JsonResult WorkDetailSchedulers(int month, int year, int viewStyle)
        {
            var fromDate = new DateTime(year, month, 1);
            var toDate = fromDate.AddMonths(1).AddDays(-1);
            var workDetails = _workDetailBll.GetWorkDetails(UserLogin.UserId, 60, fromDate, toDate);
            var homeLists = new List<HomeDashBoard>();
            if (viewStyle == 2)
            {
                var workDetails2 = _workDetailBll.GetWorkDetails(null, 60, fromDate, toDate);
                if (UserLogin.DepartmentCompany == 1)
                {
                    workDetails = workDetails2.Where(x => x.DepartmentCompany != 1).ToList();
                }
                if (UserLogin.DepartmentCompany == 2 || UserLogin.DepartmentCompany == 3)
                {
                    workDetails =
                        workDetails2.Where(
                            x =>
                                x.DepartmentCompany != 1 && x.DepartmentCompany != 2 && x.DepartmentCompany != 3 &&
                                x.DepartmentId == UserLogin.UserId).ToList();
                }
                if (UserLogin.DepartmentCompany == 4)
                {
                    workDetails =
                        workDetails2.Where(x => x.DepartmentCompany == 5 && x.DepartmentId == UserLogin.DepartmentId)
                            .ToList();
                }
            }
            for (var x = 1; x <= toDate.Day; x++)
            {
                var homeDash1 = new HomeDashBoard();
                var homeDash2 = new HomeDashBoard();
                var homeDash3 = new HomeDashBoard();
                homeDash1.DateDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, x);
                homeDash2.DateDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, x);
                homeDash3.DateDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, x);
                var workDetailstoDate =
                    workDetails.Where(y => y.ToDate.ToShortDateString() == homeDash1.DateDay.ToShortDateString())
                        .ToList();
                var count1 =
                    workDetailstoDate.Where(
                        y => (y.Status == 4 || y.Status == 3))
                        .Count();
                var count2 =
                    workDetailstoDate.Where(
                        y =>
                            DateTime.Parse(y.ToDate.ToShortDateString()) >=
                            DateTime.Parse(DateTime.Now.ToShortDateString()) && y.Status != 3 && y.Status != 4 &&
                            y.Status != 5)
                        .Count();
                var count3 =
                    workDetailstoDate.Where(
                        y =>
                            ((DateTime.Parse(y.ToDate.ToShortDateString()) <
                              DateTime.Parse(DateTime.Now.ToShortDateString()) && y.Status != 4) || y.Status == 5))
                        .Count();
                homeDash1.DateId = "HT" + x;
                homeDash1.CssClass = "work-success";
                homeDash1.Check = "1";
                homeDash1.Title = count1.ToString();

                homeDash2.DateId = "CHT" + x;
                homeDash2.Check = "2";
                homeDash2.Title = count2.ToString();
                homeDash2.CssClass = "work-need";
                homeDash3.Title = count3.ToString();
                homeDash3.DateId = "KHT" + x;
                homeDash3.Check = "3";
                homeDash3.CssClass = "work-false";

                if (count1 != 0)
                {
                    homeLists.Add(homeDash1);
                }
                if (count2 != 0)
                {
                    homeLists.Add(homeDash2);
                }
                if (count3 != 0)
                {
                    homeLists.Add(homeDash3);
                }
            }
            return Json(homeLists,
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkScheduler(DateTime dayDate, string check, int viewStyle)
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
            if (viewStyle == 2)
            {
                var workDetails3 = _workDetailBll.GetWorkDetails(null, 63, null, dayDate);

                if (check == "1")
                {
                    workDetails3 = _workDetailBll.GetWorkDetails(null, 61, null, dayDate);
                }
                if (check == "2")
                {
                    workDetails3 = _workDetailBll.GetWorkDetails(null, 62, null, dayDate);
                }
                if (UserLogin.DepartmentCompany == 1)
                {
                    workDetails3 = workDetails3.Where(x => x.DepartmentCompany != 1).ToList();
                }
                if (UserLogin.DepartmentCompany == 2 || UserLogin.DepartmentCompany == 3)
                {
                    workDetails3 =
                        workDetails3.Where(
                            x =>
                                x.DepartmentCompany != 1 && x.DepartmentCompany != 2 && x.DepartmentCompany != 3 &&
                                x.DepartmentId == UserLogin.UserId).ToList();
                }
                if (UserLogin.DepartmentCompany == 4)
                {
                    workDetails3 =
                        workDetails3.Where(x => x.DepartmentCompany == 5 && x.DepartmentId == UserLogin.DepartmentId)
                            .ToList();
                }
                return PartialView(workDetails3);
            }
            if (check == "1")
            {
                var workDetails = _workDetailBll.GetWorkDetails(UserLogin.UserId, 61, null, dayDate);
                return PartialView(workDetails);
            }
            if (check == "2")
            {
                var workDetails = _workDetailBll.GetWorkDetails(UserLogin.UserId, 62, null, dayDate);
                return PartialView(workDetails);
            }
            var workDetails2 = _workDetailBll.GetWorkDetails(UserLogin.UserId, 63, null, dayDate);
            return PartialView(workDetails2);
        }

        public ActionResult EditWork(string id, int workType)
        {
            WorkingNotesInMemory = null;
            var workDetail = _workDetailBll.GetWorkDetail(id, workType);
            if (workDetail.WorkingNote != null)
            {
                WorkingNotesInMemory = JsonConvert.DeserializeObject<List<WorkingNote>>(workDetail.WorkingNote);
            }
            return PartialView(workDetail);
        }

        //Complain
        public JsonResult Complains()
        {
            var complains = _complainBll.GetComplains_AccusedBy(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                DateTime.Now, UserLogin.UserId, 2);
            return
                Json(complains,
                    JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateWork(DateTime fromDate)
        {
            return PartialView(new SuggesWork
            {
                FromDate = fromDate,
                SuggesWorkId = Guid.NewGuid().ToString(),
                ToDate = fromDate.AddDays(+1),
                CreateDate = DateTime.Now,
                CreateBy = UserLogin.UserId,
                Quantity = 1,
                Status = 1
            });
        }


        public JsonResult ApprovedSugges()
        {
            var rights = _rightsBll.GeRightsFunction(UserLogin.UserId, "hrm", "ApproveSugges");
            if (rights.IsView)
            {
                return
                    Json(_workDetailBll.GetWorkDetails(UserLogin.UserId, 7, null, null),
                        JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public JsonResult WorkPlans()
        {
            var rights = _rightsBll.GeRightsFunction(UserLogin.UserId, "hrm", "ManagerAdmin");
            if (rights.IsView || UserLogin.RoleId == 1)
            {
                return Json(_workPlanBll.GetWorkPlansByDepartmentId(6, null, null, null), JsonRequestBehavior.AllowGet);
            }
            rights = _rightsBll.GeRightsFunction(UserLogin.UserId, "hrm", "ManagerDepartment");
            if (rights.IsView)
            {
                var department = _departmentBll.GetDepartment(UserLogin.DepartmentId ?? 0);
                var path = department.Path;
                var elements = Regex.Split(path, "/");
                var newPath = elements[0] + "/" + elements[1] + "/" + elements[2] + "/";
                return Json(_workPlanBll.GetWorkPlansByDepartmentId(5, newPath, null, null),
                    JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        //Follows
        public JsonResult AssignWorkFollowBys()
        {
            if (UserLogin.EmployeeId != null)
                return Json(_assignWorkBll.GetAssignWorkByFollows((int) UserLogin.EmployeeId),
                    JsonRequestBehavior.AllowGet);
            return Json(_assignWorkBll.GetAssignWorkByFollows(0), JsonRequestBehavior.AllowGet);
        }

        // EditWork

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

        // Post

        //Upload File
        public JsonResult DeleteFile(string filePath)
        {
            try
            {
                if (filePath == null)
                {
                    return Json(new {Status = 0, Message = "File không tồn tại!"},
                        JsonRequestBehavior.AllowGet);
                }
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
                var path = HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Kpi/WorkDetail/";
                var file = global::System.Web.HttpContext.Current.Request.Files["File"];
                var shortPath = "/Upload/Kpi/WorkDetail/";
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
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() +
                                              "/Upload/Hrm/WorkDetail");
                    var dInfo =
                        new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() +
                                          "/Upload/Hrm/WorkDetail");
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
                var fileName = DateTime.Now.Ticks + "-" + file.FileName;
                path += fileName;
                shortPath += fileName;
                file.SaveAs(path);
                return Json(new {Status = 1, Message = "Tải file thành công!", Url = shortPath},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        //TaskSearch
        public ActionResult TaskSearch()
        {
            ViewBag.CategoryKpis =
                _categoryKpiBll.GetCategoryKpis()
                    .Select(x => new KendoForeignKeyModel(x.KpiName, x.CategoryKpiId.ToString()));
            ViewBag.CategoryKpiId = UserLogin.CategoryKpiId ?? 0;
            return PartialView();
        }

        public JsonResult TaskResults(long? categoryKpiId)
        {
            if (categoryKpiId == null)
            {
                return Json(new List<Task>(), JsonRequestBehavior.AllowGet);
            }
            var tasks = _taskBll.GetTasks(null, true, categoryKpiId, null);
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        public List<KendoDropDownTreeModel> BuildDropDownTreeModels(int departmentId)
        {
            var kendoDropDownTreeModels = new List<KendoDropDownTreeModel>();
            var departments = _departmentBll.GetDepartments(null).Where(x => x.DepartmentId != departmentId).ToList();
            if (departments.Any())
            {
                var itemRoots = departments.Where(x => x.ParentId == null || x.ParentId == 0).ToList();
                kendoDropDownTreeModels.AddRange(itemRoots.Select(x => new KendoDropDownTreeModel
                {
                    value = x.DepartmentId.ToString(),
                    text = x.DepartmentName,
                    expanded = true,
                    ChildModels = GetChilds(x.DepartmentId, departments)
                }));
            }
            return kendoDropDownTreeModels;
        }

        public List<KendoDropDownTreeModel> GetChilds(long departmentId, List<Department> departments)
        {
            var kendoDropDownTreeModels = new List<KendoDropDownTreeModel>();
            var childGroups = departments.Where(x => x.ParentId == departmentId).ToList();
            if (childGroups.Any())
            {
                kendoDropDownTreeModels.AddRange(childGroups.Select(x => new KendoDropDownTreeModel
                {
                    value = x.DepartmentId.ToString(),
                    text = x.DepartmentName,
                    expanded = true,
                    ChildModels = GetChilds(x.DepartmentId, departments)
                }));
            }
            return kendoDropDownTreeModels;
        }

        public JsonResult AssignWorkCreateBys()
        {
            return Json(_assignWorkBll.GetAssignWorkByUserIds(UserLogin.UserId, null), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AssignWorkAssignBys()
        {
            return Json(_assignWorkBll.GetAssignWorkByUserIds(null, UserLogin.UserId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult WorkOutDates()
        {
            return Json(_workDetailBll.GetWorkDetails(UserLogin.UserId, 22, null, null), JsonRequestBehavior.AllowGet);
        }

        public JsonResult WorkNeedCompletes()
        {
            return Json(_workDetailBll.GetWorkDetails(UserLogin.UserId, 101, null, null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssignWork(string id)
        {
            var user = _userBll.GetUser(UserLogin.UserId);
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new AssignWork
                {
                    AssignWorkId = Guid.Empty.ToString(),
                    Status = (byte) WorkDetailStatusEnum.InPlan,
                    CreateBy = UserLogin.UserId,
                    CreateDate = DateTime.Now,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(+7),
                    ActionCompany = user.DepartmentCompany
                });
            }
            return PartialView(_assignWorkBll.GetAssignWork(id));
        }

        public JsonResult SuggesWorks()
        {
        
            var workdetails = _workDetailBll.GetWorkDetails(UserLogin.UserId, 25, null, null);

            return Json(workdetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveExplanation(string id, string content, int workType)
        {
            try
            {
                var workDetail = _workDetailBll.GetWorkDetail(id, workType);
                if (string.IsNullOrEmpty(content))
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
                }
                var listExplanation = new List<Explanation>();

                if (workDetail.Explanation != null)
                {
                    listExplanation = JsonConvert.DeserializeObject<List<Explanation>>(workDetail.Explanation);
                }
                listExplanation.Add(new Explanation
                {
                    CreateDate = DateTime.Now,
                    ExplanationText = content
                });
                workDetail.Explanation = JsonConvert.SerializeObject(listExplanation);
                workDetail.Status = 5;
                if (workDetail.WorkType == (int) StatusWorkDetail.WorkPlanDetail)
                {
                    var workPlanDetail = _workPlanDetailBll.GetWorkPlanDetail(workDetail.WorkDetailId);
                    workPlanDetail.Status = workDetail.Status;
                    workPlanDetail.Explanation = workDetail.Explanation;
                    if (_workPlanDetailBll.Update(workPlanDetail))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (workDetail.WorkType == (int) StatusWorkDetail.AssignWork)
                {
                    var assignWork = _assignWorkBll.GetAssignWork(workDetail.WorkDetailId);
                    assignWork.Status = (byte) workDetail.Status;
                    assignWork.Explanation = workDetail.Explanation;
                    if (_assignWorkBll.Update(assignWork))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (workDetail.WorkType == (int) StatusWorkDetail.WorkStreamDetail)
                {
                    var workStreamDetail = _workStreamDetailBll.GetWorkStreamDetail(workDetail.WorkDetailId);
                    workStreamDetail.Status = workDetail.Status;
                    workStreamDetail.Explanation = workDetail.Explanation;
                    if (_workStreamDetailBll.Update(workStreamDetail))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (workDetail.WorkType == (int) StatusWorkDetail.SuggesWork)
                {
                    var suggesWork = _suggesWorkBll.GetSuggesWork(workDetail.WorkDetailId);
                    suggesWork.Status = (byte) workDetail.Status;
                    suggesWork.Explanation = workDetail.Explanation;
                    if (_suggesWorkBll.Update(suggesWork))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }

                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Employees(string id)
        {
            var employees = _employeeBll.GetEmployeesByDepartmentAndPosition((byte) DepartmentConpanyEnum.Employee,
                UserLogin.DepartmentId);
            return PartialView(employees);
        }

        public ActionResult WorkDetailExplanation(string id, int workType)
        {
            var workDetail = _workDetailBll.GetWorkDetail(id, workType);
            return PartialView(workDetail);
        }
    }
}