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
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;
using Entity.Kpi;
using Entity.System;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly AddressBll _addressBll;
        private readonly AutoNumberBll _autoNumberBll;
        private readonly CareerBll _careerBll;
        private readonly CountryBll _countryBll;
        private readonly DepartmentBll _departmentBll;
        private readonly EducationLevelBll _educationLevelBll;
        private readonly EmployeeBll _employeeBll;
        private readonly NationBll _nationBll;
        private readonly PositionBll _positionBll;
        private readonly ReligionBll _religionBll;
        private readonly SchoolBll _schoolBll;
        private readonly TrainingLevelBll _trainingLevelBll;
        private readonly UserBll _userBll;
        private readonly ContractBll _contractBll;
        private readonly ContractTypeBll _contractTypeBll;
        private readonly PraiseDisciplineBll _praiseDisciplineBll;
        private readonly SalaryBll _salaryBll;
        private readonly IncurredSalaryBll _incurredSalaryBll;
        private readonly MedicalBll _medicalBll;
        private readonly InsuranceProcessBll _insuranceProcessBll;
        private readonly InsuranceBll _insuranceBll;
        private readonly InsuranceMedicalBll _insuranceMedicalBll;
        private readonly HolidayConfigBll _holidayConfigBll;
        private readonly string _prefix = "KT" + DateTime.Now.Year.ToString().Substring(2, 2);
        private readonly string _prefixKl = "KL" + DateTime.Now.Year.ToString().Substring(2, 2);

        private readonly JobChangeBll _jobChangeBll;
        private readonly string _prefixTcct = "TCCT" + DateTime.Now.Year.ToString().Substring(2, 2);
        private readonly ShiftWorkBll _shiftWorkBll;
        public EmployeeController()
        {
            _holidayConfigBll = SingletonIpl.GetInstance<HolidayConfigBll>();
            _insuranceMedicalBll = SingletonIpl.GetInstance<InsuranceMedicalBll>();
            _medicalBll = SingletonIpl.GetInstance<MedicalBll>();
            _praiseDisciplineBll = SingletonIpl.GetInstance<PraiseDisciplineBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _countryBll = SingletonIpl.GetInstance<CountryBll>();
            _nationBll = SingletonIpl.GetInstance<NationBll>();
            _religionBll = SingletonIpl.GetInstance<ReligionBll>();
            _addressBll = SingletonIpl.GetInstance<AddressBll>();
            _positionBll = SingletonIpl.GetInstance<PositionBll>();
            _trainingLevelBll = SingletonIpl.GetInstance<TrainingLevelBll>();
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _educationLevelBll = SingletonIpl.GetInstance<EducationLevelBll>();
            _careerBll = SingletonIpl.GetInstance<CareerBll>();
            _schoolBll = SingletonIpl.GetInstance<SchoolBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _contractBll = SingletonIpl.GetInstance<ContractBll>();
            _contractTypeBll = SingletonIpl.GetInstance<ContractTypeBll>();
            _jobChangeBll = SingletonIpl.GetInstance<JobChangeBll>();
            _salaryBll = SingletonIpl.GetInstance<SalaryBll>();
            _incurredSalaryBll = SingletonIpl.GetInstance<IncurredSalaryBll>();
            _insuranceProcessBll = SingletonIpl.GetInstance<InsuranceProcessBll>();
            _insuranceBll = SingletonIpl.GetInstance<InsuranceBll>();
            _shiftWorkBll = SingletonIpl.GetInstance<ShiftWorkBll>();

        }

        [HrmFilter]
        public ActionResult Index()
        {
            var shiftWorks = _shiftWorkBll.GetShiftWorks();
            var departments = _departmentBll.GetDepartments(true);
            var cities = _addressBll.GetCities();
            var countries = _countryBll.GetCountries();
            //var nations = _nationBll.GetNations();
            //var religions = _religionBll.GetReligions();
            var users = _userBll.GetUsers(true);
            var positions = _positionBll.GetPositions();
            //var categoryKpis = _categoryKpiBll.GetCategoryKpis();
            //ViewBag.CategoryKpi =
            //    categoryKpis.Select(x => new KendoForeignKeyModel(x.KpiName, x.CategoryKpiId.ToString())
            //        );
            var contractTypes = _contractTypeBll.GetContractTypes(null);
            ViewBag.ContractTypes =
               contractTypes.Select(
                   x => new KendoForeignKeyModel { value = x.ContractTypeId.ToString(), text = x.TypeName });
            var years = new List<KendoForeignKeyModel>();
            for (var i = ((DateTime.Now.Year) - 10); i <= ((DateTime.Now.Year) + 10); i++)
            {
                years.Add(new KendoForeignKeyModel("Năm " + i, i.ToString()));
            }
            ViewBag.ShiftWorks =
               shiftWorks.Select(x => new KendoForeignKeyModel { value = x.ShiftWorkId.ToString(), text = x.ShiftWorkCode });
            ViewBag.Years = years;
            //ViewBag.Medicals =
            //    _medicalBll.GetMedicals().Select(x => new KendoForeignKeyModel(x.MedicalName, x.MedicalId.ToString()));
            ViewBag.Positions = positions.Select(x => new KendoForeignKeyModel(x.PositionName, x.PositionId.ToString()));
            ViewBag.Countries =
               countries.Select(x => new KendoForeignKeyModel { value = x.CountryId.ToString(), text = x.CountryName });
            //ViewBag.Religions =
            //   religions.Select(x => new KendoForeignKeyModel { value = x.ReligionId.ToString(), text = x.ReligionName });
            //ViewBag.Nations =
            //   nations.Select(x => new KendoForeignKeyModel { value = x.NationId.ToString(), text = x.NationName });
           
            ViewBag.Departments =
                departments.Select(
                    x => new KendoForeignKeyModel {value = x.DepartmentId.ToString(), text = x.DepartmentName});
            ViewBag.DepartmentObjects = departments;
            ViewBag.Users =
                users.Select(
                    x => new KendoForeignKeyModel { value = x.UserId.ToString(), text = x.FullName });
            ViewBag.Cities =
                cities.Select(x => new KendoForeignKeyModel {value = x.CityId.ToString(), text = x.CityName});
            ViewBag.Sexs = from SexEnum s in Enum.GetValues(typeof (SexEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            //ViewBag.MaritalStatuses = from MaritalStatus s in Enum.GetValues(typeof (MaritalStatus))
            //    let singleOrDefault =
            //        (DescriptionAttribute)
            //            s.GetType()
            //                .GetField(s.ToString())
            //                .GetCustomAttributes(typeof (DescriptionAttribute), false)
            //                .SingleOrDefault()
            //    where singleOrDefault != null
            //    select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            //ViewBag.DepartmentConpanyEnums = from DepartmentConpanyEnum s in Enum.GetValues(typeof(DepartmentConpanyEnum))
            //                              let singleOrDefault =
            //                                  (DescriptionAttribute)
            //                                      s.GetType()
            //                                          .GetField(s.ToString())
            //                                          .GetCustomAttributes(typeof(DescriptionAttribute), false)
            //                                          .SingleOrDefault()
            //                              where singleOrDefault != null
            //                              select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            ViewBag.EmployeeStatusEnums = from EmployeeStatusEnum s in Enum.GetValues(typeof (EmployeeStatusEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            return View();
        }

        public JsonResult Employees(string path)
        {
            var employees = _employeeBll.GetEmployees(null);
            if (string.IsNullOrEmpty(path))
            {
                return Json(employees, JsonRequestBehavior.AllowGet);
            }
            var employeesBypath = _employeeBll.GetEmployeesByPath(path);
            return Json(employeesBypath, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Employee(string id)
        {         
            ViewBag.DepartmentTrees = BuildDropDownTreeModels(0);
            if (string.IsNullOrEmpty(id))
            {
                return
                    PartialView(new Employee
                    {
                        EmployeeCode = _autoNumberBll.GetAutoNumber(AutoNumberEnum.NV.ToString()),
                        Avatar = "/Content/images/avatar-img.jpg",
                        IsActive = true,
                        CreateDate = DateTime.Now,
                        CreateBy = UserLogin.UserId,
                        Status = 1
                    });
            }
            var employeeId = long.Parse(id);
            var employee = _employeeBll.GetEmployee(employeeId);
            return PartialView(employee);
        }

        public JsonResult EmJsonResult(Employee model)
        {
            foreach (var item in _employeeBll.GetEmployees(null))
            {
                item.EmployeeId = 1;
                item.FullName = "";
                item.Email = "tungnm06@gmail.com";
                item.DepartmentId = 1;
                item.EducationLevelName = "";
                item.Gender = 1;
                item.DepartmentName = "Trưởng phòng HCNS";
            }
            return Json(_employeeBll.GetEmployees(null), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DistrictsByCityId(int cityId)
        {
            var districtsByCityId = _addressBll.GetDistricts(cityId);
            var districts = from District w in districtsByCityId
                select new KendoForeignKeyModel {value = w.DistrictId.ToString(), text = w.DistrictName};
            return Json(districts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAvatar(string imageUrl)
        {
            try
            {
                if (imageUrl.Contains("/Content/images/avatar-img.jpg"))
                {
                    return Json(new {Status = 0, Message = "Bạn không thể xóa ảnh mặc định!"},
                        JsonRequestBehavior.AllowGet);
                }
                global::System.IO.File.Delete(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/" + imageUrl);
                return Json(new {Status = 1, Message = "Xóa ảnh thành công!", Url = "/Content/images/avatar-img.jpg"},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UploadAvatar()
        {
            try
            {
                var path = HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Employee/Avatar/";
                string shortPath = "/Upload/Employee/Avatar/";
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath()+"/Upload"))
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
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Employee"))
                {
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Employee");
                    var dInfo = new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Employee");
                    var dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(
                        new FileSystemAccessRule(
                            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                            FileSystemRights.FullControl,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                }
                if (!Directory.Exists(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Employee/Avatar"))
                {
                    Directory.CreateDirectory(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Employee/Avatar");
                    var dInfo = new DirectoryInfo(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Upload/Employee/Avatar");
                    var dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(
                        new FileSystemAccessRule(
                            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                            FileSystemRights.FullControl,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                }
                var file = global::System.Web.HttpContext.Current.Request.Files["ImageAvatar"];
                if (file == null || file.ContentLength == 0)
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
                }
                string fileName= DateTime.Now.Ticks + "-" + file.FileName;
                path += fileName;
                shortPath += fileName;
                file.SaveAs(path);
                return Json(new {Status = 1, Message = "Tải ảnh thành công!", Url = shortPath }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Save(EmployeeModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
                }
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
                var employeeByCode = _employeeBll.GetEmployee(model.EmployeeCode);
                var employeeByTimeSheetCode = _employeeBll.GetEmployeeByTimeSheetCode(model.TimeSheetCode ?? "TimeSheetCode");
                if (model.EmployeeId <= 0)
                {
                    if(employeeByCode != null)
                        return Json(new { Status = 0, Message = "Mã nhân viên đã tồn tại trong hệ thống!" },
                        JsonRequestBehavior.AllowGet);
                    if(employeeByTimeSheetCode != null)
                        return Json(new { Status = 0, Message = "Mã chấm công đã tồn tại trong hệ thống!" },
                        JsonRequestBehavior.AllowGet);
                    model.CreateDate = DateTime.Now;
                    model.EmployeeCode = "NV";
                    var employeeCode = "";
                    if (_employeeBll.Insert(model.ToObject(), ref employeeCode) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (employeeByCode != null && employeeByCode.EmployeeId != model.EmployeeId)
                    return Json(new { Status = 0, Message = "Mã nhân viên đã tồn tại trong hệ thống!" },
                    JsonRequestBehavior.AllowGet);
                //if (employeeByTimeSheetCode != null && employeeByTimeSheetCode.EmployeeId != model.EmployeeId)
                //    return Json(new { Status = 0, Message = "Mã chấm công đã tồn tại trong hệ thống!" },
                //    JsonRequestBehavior.AllowGet);
                if (_employeeBll.Update(model.ToObject()))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                var employee = _employeeBll.GetEmployee(long.Parse(id));
                if (employee == null)
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                }
                if (_employeeBll.Delete(employee.EmployeeId))
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
        //Country
        public JsonResult Countries()
        {
            var positions = _countryBll.GetCountries();
            return Json(positions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CountrySearch()
        {

            return PartialView();
        }
        public ActionResult Country(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new Country());
            }
            return PartialView(_countryBll.GetCountry(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult SaveCountry(CountryModel model)
        {
            try
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
                var country = _countryBll.GetCountry(model.CountryCode);
                if (model.CountryId <= 0)
                {
                    if (country != null)
                    {
                        return Json(new { Status = 0, Message = "Mã quốc tịch đã tồn tại trong hệ thống!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_countryBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (country != null && country.CountryId != model.CountryId)
                {
                    return Json(new { Status = 0, Message = "Mã quốc tịch đã tồn tại trong hệ thống!" },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_countryBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteCountry(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_countryBll.Delete(int.Parse(id)))
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
        
        //Nation
        public JsonResult Nations()
        {
            var nations = _nationBll.GetNations();
            return Json(nations, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NationSearch()
        {

            return PartialView();
        }

        public ActionResult Nation(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new Nation());
            return PartialView(_nationBll.GetNation(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult SaveNation(NationModel model)
        {
            try
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
                if (model.NationId == 0)
                {
                    if (_nationBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_nationBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteNation(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_nationBll.Delete(int.Parse(id)))
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
        
        // Religion
        public JsonResult Religions()
        {
            var religions = _religionBll.GetReligions();
            return Json(religions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReligionSearch()
        {

            return PartialView();
        }

        public ActionResult Religion(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return
                    PartialView(new Religion());
            }
            return PartialView(_religionBll.GetReligion(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult SaveReligion(ReligionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { Status = 0, Message = MessageAction.ModelStateNotValid },
                        JsonRequestBehavior.AllowGet);
                if (model.ReligionId == 0)
                {
                    if (_religionBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }

                if (!_religionBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteReligion(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_religionBll.Delete(int.Parse(id)))
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
        
        //TrainingLevel
        public JsonResult TrainingLevels()
        {
            var trainingLevels = _trainingLevelBll.GetTrainingLevels();
            return Json(trainingLevels, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TrainingLevelSearch()
        {
            return PartialView();
        }
        public ActionResult TrainingLevel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return
                    PartialView(new TrainingLevel());
            }
            var trainingLevelId = int.Parse(id);
            return PartialView(_trainingLevelBll.GetTrainingLevel(trainingLevelId));
        }

        [HttpPost]
        public JsonResult SaveTrainingLevel(TrainingLevelModel model)
        {
            try
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
                var trainingLevel = _trainingLevelBll.GetTrainingLevel(model.LevelCode);
                if (model.TrainingLevelId == 0)
                {
                    if (trainingLevel != null)
                    {
                        return Json(new { Status = 0, Message = "Mã chức vụ đã tồn tại trong hệ thống!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_trainingLevelBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (trainingLevel != null && trainingLevel.TrainingLevelId != model.TrainingLevelId)
                {
                    return Json(new { Status = 0, Message = "Mã chức vụ đã tồn tại trong hệ thống!" },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_trainingLevelBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteTrainingLevel(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_trainingLevelBll.Delete(int.Parse(id)))
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
      
        //Position
        public JsonResult Positions()
        {
            var positions = _positionBll.GetPositions();
            return Json(positions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PositionSearch()
        {
            return PartialView();
        }

        public ActionResult Position(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new Position());
            return PartialView(_positionBll.GetPosition(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult SavePosition(PositionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { Status = 0, Message = MessageAction.ModelStateNotValid },
                        JsonRequestBehavior.AllowGet);
                var position = _positionBll.GetPosition(model.PositionCode);
                if (model.PositionId == 0)
                {
                    if (position != null)
                    {
                        return Json(new { Status = 0, Message = "Mã chức vụ đã tồn tại trong hệ thống!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_positionBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (position != null && position.PositionId != model.PositionId)
                {
                    return Json(new { Status = 0, Message = "Mã chức vụ đã tồn tại trong hệ thống!" },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_positionBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeletePosition(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_positionBll.Delete(int.Parse(id)))
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
      
        //EducationLevel
        public JsonResult EducationLevels()
        {
            var educationLevels = _educationLevelBll.GetEducationLevels();
            return Json(educationLevels, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EducationLevelSearch()
        {
            return PartialView();
        }

        public ActionResult EducationLevel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return
                    PartialView(new EducationLevel());
            }
            var educationLevelId = int.Parse(id);
            return PartialView(_educationLevelBll.GetEducationLevel(educationLevelId));
        }

        [HttpPost]
        public JsonResult SaveEducationLevel(EducationLevelModel model)
        {
            try
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
                var educationLevel = _educationLevelBll.GetEducationLevel(model.LevelCode);
                if (model.EducationLevelId == 0)
                {
                    if (educationLevel != null)
                    {
                        return Json(new { Status = 0, Message = "Mã trình độ đào tạo đã tồn tại trong hệ thống!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_educationLevelBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (educationLevel != null && educationLevel.EducationLevelId != model.EducationLevelId)
                {
                    return Json(new { Status = 0, Message = "Mã trình độ đào tạo đã tồn tại trong hệ thống!" },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_educationLevelBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteEducationLevel(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_educationLevelBll.Delete(int.Parse(id)))
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
        // School
        public JsonResult Schools()
        {
            var schools = _schoolBll.GetSchools();
            return Json(schools, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SchoolSearch()
        {
            return PartialView();
        }
        public ActionResult School(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new School());
            return PartialView(_schoolBll.GetSchool(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult SaveSchool(SchoolModel model)
        {
            try
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
                if (model.SchoolId == 0)
                {
                    if (_schoolBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_schoolBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteSchool(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_schoolBll.Delete(int.Parse(id)))
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
        // Career
        public JsonResult Careers()
        {
            var careers = _careerBll.GetCareers();
            return Json(careers, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CareerSearch()
        {
            return PartialView();
        }
        public ActionResult Career(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new Career());
            return PartialView(_careerBll.GetCareer(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult SaveCareer(CareerModel model)
        {
            try
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
                if (model.CareerId == 0)
                {
                    if (_careerBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_careerBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteCareer(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_careerBll.Delete(int.Parse(id)))
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

        // Contract
        public ActionResult Contracts(long employeeId)
        {
            var contracts = _contractBll.GetContractsByEmployeeId(employeeId);
            return Json(contracts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contract(string id)
        {
            var contractTypes = _contractTypeBll.GetContractTypes(null);

            ViewBag.ContractTypes =
              contractTypes.Select(
                  x => new KendoForeignKeyModel { value = x.ContractTypeId.ToString(), text = x.TypeName });
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
        public JsonResult SaveContract(ContractModel model)
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
                var contract = _contractBll.GetContractByContractCode(model.ContractCode);
                if (string.IsNullOrEmpty(model.ContractId) || model.ContractId == Guid.Empty.ToString())
                {
                    if (contract != null)
                    {
                        return Json(new { Status = 0, Message = "Số hợp đồng đã tồn tại trong hệ thống!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    model.CreateDate = DateTime.Now;
                    model.CreateBy = UserLogin.UserId;
                    model.ContractId = Guid.NewGuid().ToString();
                    if (_contractBll.Insert(model.ToObject()))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (contract != null && contract.ContractId != model.ContractId)
                {
                    return Json(new { Status = 0, Message = "Số hợp đồng đã tồn tại trong hệ thống!" },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_contractBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteContract(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                var contract = _contractBll.GetContract(id);
                if (contract == null)
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_contractBll.Delete(id))
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


        public JsonResult DeleteFileContract(string filePath)
        {
            try
            {
                global::System.IO.File.Delete(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/" + filePath);
                return Json(new { Status = 1, Message = "Xóa file thành công!" },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UploadFileContract()
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
        // ContractType
        public ActionResult IndexContractType()
        {
            return PartialView();
        }

        public JsonResult ContractTypes()
        {
            var contractTypes = _contractTypeBll.GetContractTypes(null);
            return Json(contractTypes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ContractType(string id)
        {
            var contractTypes = _contractTypeBll.GetContractTypes(null);
            ViewBag.ContractTypes =
               contractTypes.Select(
                   x => new KendoForeignKeyModel { value = x.ContractTypeId.ToString(), text = x.TypeName });
            if (string.IsNullOrEmpty(id))
            {
                return
                    PartialView(new ContractType
                    {
                        IsActive = true
                    });
            }
            return PartialView(_contractTypeBll.GetContractType(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult SaveContractType(ContractTypeModel model)
        {
            try
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
                if (model.ContractTypeId == 0)
                {
                    if (_contractTypeBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_contractTypeBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteContractType(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_contractTypeBll.Delete(int.Parse(id)))
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
        // Khen Thuong
        public JsonResult Praises(long employeeId)
        {
            var praiseDisciplines = _praiseDisciplineBll.GetPraiseDisciplinesByEmployeeId((byte)PraiseDisciplineType.Praise, employeeId);
            return Json(praiseDisciplines, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Praise(string id)
        {
            var departments = _departmentBll.GetDepartments(null);
            ViewBag.Departments =
                departments.Select(
                    x => new KendoForeignKeyModel { value = x.DepartmentId.ToString(), text = x.DepartmentName });
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new PraiseDiscipline
                {
                    PraiseDisciplineDate = DateTime.Now,
                    PraiseDisciplineId = Guid.Empty.ToString(),
                    PraiseDisciplineCode = _autoNumberBll.GetAutoNumber(_prefix)
                });
            }
            var praiseDiscipline = _praiseDisciplineBll.GetPraiseDiscipline(id);
            return PartialView(praiseDiscipline);
        }

        [HttpPost]
        public JsonResult SavePraise(PraiseDisciplineModel model,long employeeId)
        {
            try
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
                var praiseDetails = new List<PraiseDisciplineDetail>();
                if (employeeId != 0)
                {
                    var employee = _employeeBll.GetEmployee(employeeId);
                    var praiseDetail = new PraiseDisciplineDetail
                    {
                        FullName = employee.FullName,
                        DepartmentId = employee.DepartmentId,
                        EmployeeId = employee.EmployeeId,
                        EmployeeCode = employee.EmployeeCode
                    };
                    praiseDetails.Add(praiseDetail);
                }             
                var praiseDiscipline = model.ToObject();
                praiseDiscipline.PraiseDisciplineType = (byte)PraiseDisciplineType.Praise;

                praiseDiscipline.PraiseDisciplineDetails = praiseDetails;
                if (string.IsNullOrEmpty(praiseDiscipline.PraiseDisciplineId) ||
                    praiseDiscipline.PraiseDisciplineId == Guid.Empty.ToString())
                {
                    praiseDiscipline.CreateDate = DateTime.Now;
                    praiseDiscipline.CreateBy = UserLogin.UserId;
                    praiseDiscipline.PraiseDisciplineId = Guid.NewGuid().ToString();
                    praiseDiscipline.PraiseDisciplineCode = _prefix;
                    var praiseDisciplineCode = "";
                    if (_praiseDisciplineBll.Insert(praiseDiscipline, ref praiseDisciplineCode))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_praiseDisciplineBll.Update(praiseDiscipline))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeletePraise(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                var praiseDiscipline = _praiseDisciplineBll.GetPraiseDiscipline(id);
                if (praiseDiscipline == null)
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_praiseDisciplineBll.Delete(id))
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
        //
        public JsonResult Disciplines(long employeeId)
        {
            var praiseDisciplines = _praiseDisciplineBll.GetPraiseDisciplinesByEmployeeId((byte)PraiseDisciplineType.Discipline, employeeId);
            return Json(praiseDisciplines, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Discipline(string id)
        {
            var departments = _departmentBll.GetDepartments(null);
            ViewBag.Departments =
                departments.Select(
                    x => new KendoForeignKeyModel { value = x.DepartmentId.ToString(), text = x.DepartmentName });
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new PraiseDiscipline
                {
                    PraiseDisciplineDate = DateTime.Now,
                    PraiseDisciplineId = Guid.Empty.ToString(),
                    PraiseDisciplineCode = _autoNumberBll.GetAutoNumber(_prefix)
                });
            }
            var praiseDiscipline = _praiseDisciplineBll.GetPraiseDiscipline(id);
            return PartialView(praiseDiscipline);
        }
        [HttpPost]
        public JsonResult SaveDiscipline(PraiseDisciplineModel model,long employeeId)
        {
            try
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
                var praiseDiscipline = model.ToObject();
                praiseDiscipline.PraiseDisciplineType = (byte)PraiseDisciplineType.Discipline;
                var praiseDisciplineDetails = new List<PraiseDisciplineDetail>();
                var employee = _employeeBll.GetEmployee(employeeId);
                praiseDisciplineDetails.Add(new PraiseDisciplineDetail
                {
                    FullName = employee.FullName,
                    DepartmentId = employee.DepartmentId,
                    EmployeeId = employee.EmployeeId,
                    EmployeeCode = employee.EmployeeCode
                });
                praiseDiscipline.PraiseDisciplineDetails = praiseDisciplineDetails;
                if (string.IsNullOrEmpty(praiseDiscipline.PraiseDisciplineId) ||
                    praiseDiscipline.PraiseDisciplineId == Guid.Empty.ToString())
                {
                    praiseDiscipline.CreateDate = DateTime.Now;
                    praiseDiscipline.CreateBy = UserLogin.UserId;
                    praiseDiscipline.PraiseDisciplineId = Guid.NewGuid().ToString();
                    praiseDiscipline.PraiseDisciplineCode = _prefixKl;
                    var praiseDisciplineCode = "";
                    if (_praiseDisciplineBll.Insert(praiseDiscipline, ref praiseDisciplineCode))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_praiseDisciplineBll.Update(praiseDiscipline))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteDiscipline(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                var praiseDiscipline = _praiseDisciplineBll.GetPraiseDiscipline(id);
                if (praiseDiscipline == null)
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_praiseDisciplineBll.Delete(id))
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
        // JobChange
        public JsonResult JobChanges(long employeeId)
        {
            var jobChanges = _jobChangeBll.GetJobChangesByEmployeeId(employeeId);
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
                    return Json(new { Status = 0, Message = "Không có file để xóa!" },
                        JsonRequestBehavior.AllowGet);
                }
                global::System.IO.File.Delete(HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/" + filePath);
                return Json(new { Status = 1, Message = "Xóa file thành công!", Url = "" },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
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
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }
                path += DateTime.Now.Ticks + "-" + file.FileName;
                file.SaveAs(path);
                return Json(new { Status = 1, Message = "Tải ảnh thành công!", Url = path }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveJobChange(JobChangeModel model)
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
                    model.JobChangeCode = _prefixTcct;
                    var jobChangeCode = "";
                    if (_jobChangeBll.Insert(model.ToObject(), ref jobChangeCode))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_jobChangeBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteJobChange(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                var jobChange = _jobChangeBll.GetJobChange(id);
                if (jobChange == null)
                {
                    return Json(new { Status = 0, Message = "Mã thuyên chuyển không tồn tại trong hệ thống!" },
                        JsonRequestBehavior.AllowGet);
                }
                if (_jobChangeBll.Delete(jobChange))
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
        // Salary
        public JsonResult Salaries(long? employeeId)
        {
            if (employeeId == 0 || employeeId == null)
                return Json(new List<Salary>(), JsonRequestBehavior.AllowGet);
            return Json(_salaryBll.GetSalaries((long)employeeId), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult SalariesEmployee(long employeeId)
        {
            ViewBag.EmployeeId = employeeId;
            return PartialView();
        }

        public ActionResult Salary(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new Salary { ApplyDate = DateTime.Now, SalaryId = Guid.Empty.ToString() });
            return PartialView(_salaryBll.GetSalary(id));
        }

        [HttpPost]
        public JsonResult SaveSalary(SalaryModel model)
        {
            try
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
                model.CreateBy = UserLogin.UserId;
                model.CreateDate = DateTime.Now;
                model.PercentProfessional = 100;
                if (string.IsNullOrEmpty(model.SalaryId) || model.SalaryId == Guid.Empty.ToString())
                {
                    model.SalaryId = Guid.NewGuid().ToString();
                    if (_salaryBll.Insert(model.ToObject()))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_salaryBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteSalary(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_salaryBll.Delete(id))
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
        // Lương thưởng IncurredSalary
        public JsonResult IncurredSalaries(long employeeId,DateTime fromDate, DateTime toDate)
        {
            return Json(_incurredSalaryBll.GetIncurredSalaries(employeeId, fromDate, toDate, false),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult IncurredSalary(string id)
        {
            if (string.IsNullOrEmpty(id))
                return
                    PartialView(new IncurredSalary { IncurredSalaryId = Guid.Empty.ToString(), SubmitDate = DateTime.Now });
            return PartialView(_incurredSalaryBll.GetIncurredSalary(id));
        }

        [HttpPost]
        public JsonResult SaveIncurredSalary(IncurredSalaryModel model)
        {
            try
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
                model.CreateBy = UserLogin.UserId;
                model.CreateDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.IncurredSalaryId) || model.IncurredSalaryId == Guid.Empty.ToString())
                {
                    model.IncurredSalaryId = Guid.NewGuid().ToString();
                    if (_incurredSalaryBll.Insert(model.ToObject()))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_incurredSalaryBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteIncurredSalary(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_incurredSalaryBll.Delete(id))
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
        // InsuranceMedical
        public JsonResult InsuranceMedicals(int? year,long employeeId)
        {
            return Json(_insuranceMedicalBll.GetInsuranceMedicals(year ?? DateTime.Now.Year, employeeId),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsuranceMedical(string id)
        {
            ViewBag.Medicals =
                _medicalBll.GetMedicals().Select(x => new KendoForeignKeyModel(x.MedicalName, x.MedicalId.ToString()));
            if (string.IsNullOrEmpty(id))
                return PartialView(new InsuranceMedical
                {
                    InsuranceMedicalId = Guid.Empty.ToString(),
                    StartDate = DateTime.Now,
                    ExpiredDate = DateTime.Now.AddYears(1)
                });
            return PartialView(_insuranceMedicalBll.GetInsuranceMedical(id));
        }
        [HttpPost]
        public JsonResult SaveInsuranceMedical(InsuranceMedicalModel model)
        {
            try
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
                model.CreateBy = UserLogin.UserId;
                model.CreateDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.InsuranceMedicalId) || model.InsuranceMedicalId == Guid.Empty.ToString())
                {
                    model.InsuranceMedicalId = Guid.NewGuid().ToString();
                    if (_insuranceMedicalBll.Insert(model.ToObject()))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_insuranceMedicalBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteInsuranceMedical(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_insuranceMedicalBll.Delete(id))
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
        //Medical
        public ActionResult IndexMedical()
        {
            return PartialView();
        }

        public JsonResult Medicals()
        {
            return Json(_medicalBll.GetMedicals(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Medical(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new Medical());
            return PartialView(_medicalBll.GetMedical(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult SaveMedical(MedicalModel model)
        {
            try
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
                if (model.MedicalId <= 0)
                {
                    if (_medicalBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_medicalBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteMedical(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_medicalBll.Delete(int.Parse(id)))
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
        // InsuranceProcess
        public JsonResult InsuranceProcesses(long employeeId)
        {
            return Json(_insuranceProcessBll.GetInsuranceProcesses(employeeId), JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult IndexProcess()
        {
            return PartialView();
        }
        public ActionResult InsuranceProcess(string id,long employeeId)
        {

            if (string.IsNullOrEmpty(id))
            {
                var insurance = _insuranceBll.GetInsuranceByEmployeeId(employeeId);
                if (insurance != null)
                {
                    return PartialView(new InsuranceProcess
                    {
                        InsuranceProcessId = Guid.Empty.ToString(),
                        Amount = 0,
                        FromDate = DateTime.Now,
                        InsuranceId = insurance.InsuranceId,
                        InsuranceNumber = insurance.InsuranceNumber
                    });
                }
                return PartialView(new InsuranceProcess
                {
                    InsuranceProcessId = Guid.Empty.ToString(),
                    Amount = 0,
                    FromDate = DateTime.Now
                });
            }
           
            return PartialView(_insuranceProcessBll.GetInsuranceProcess(id));
        }

        [HttpPost]
        public JsonResult SaveProcess(InsuranceProcessModel model)
        {
            try
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
                model.CreateBy = UserLogin.UserId;
                model.CreateDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.InsuranceProcessId) || model.InsuranceProcessId == Guid.Empty.ToString())
                {
                    model.InsuranceProcessId = Guid.NewGuid().ToString();
                    if (_insuranceProcessBll.Insert(model.ToObject()))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_insuranceProcessBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteProcess(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_insuranceProcessBll.Delete(id))
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
        // Insurance
        
        public JsonResult Insurances()
        {

            return Json(_insuranceBll.GetInsurances(null), JsonRequestBehavior.AllowGet);
        }
        public ActionResult IndexInsurance()
        {
            return PartialView();
        }
        public ActionResult Insurance(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new Insurance
                {
                    SubscriptionDate = DateTime.Now,
                    IsActive = true,
                    MonthBefore = 0
                });
            }
            var insurance = _insuranceBll.GetInsurance(long.Parse(id));
            return PartialView(insurance);
        }

        [HttpPost]
        public JsonResult SaveInsurance(InsuranceModel model)
        {
            try
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
                model.CreateBy = UserLogin.UserId;
                model.CreateDate = DateTime.Now;
                if (model.InsuranceId <= 0)
                {
                    if (_insuranceBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                if (!_insuranceBll.Update(model.ToObject()))
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
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
        public JsonResult DeleteInsurance(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_insuranceBll.Delete(long.Parse(id)))
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
        // HolidayConfig
        public JsonResult HolidayConfigs(int year,long employeeId)
        {
            if (employeeId == 0)
            {
                return Json(new List<HolidayConfig>(), JsonRequestBehavior.AllowGet);
            }
            var holidayConfig = _holidayConfigBll.GetHolidayConfigs(year, employeeId);
            if (holidayConfig.Count ==0)
            {
                _holidayConfigBll.Insert(new HolidayConfig
                {
                    HolidayNumber = 0,
                    Year = year,
                    EmployeeId = employeeId
                });
            }
            return Json(_holidayConfigBll.GetHolidayConfigs(year, employeeId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveHoliday(HolidayConfig model)
        {
            try
            {
                _holidayConfigBll.Update(model);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public JsonResult ClearSession()
        {
            return null;
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
    }
}
