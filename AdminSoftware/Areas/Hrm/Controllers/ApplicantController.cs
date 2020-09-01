using System;
using System.ComponentModel;
using System.Linq;
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

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class ApplicantController : BaseController
    {
        private readonly AddressBll _addressBll;
        private readonly ApplicantBll _applicantBll;
        private readonly CountryBll _countryBll;
        private readonly NationBll _nationBll;
        private readonly RecruitChanelBll _recruitChanelBll;
        private readonly RecruitPlanBll _recruitPlanBll;
        private readonly ReligionBll _religionBll;
        private readonly TrainingLevelBll _trainingLevelBll;
        private readonly UserBll _userBll;

        public ApplicantController()
        {
            _applicantBll = SingletonIpl.GetInstance<ApplicantBll>();
            _countryBll = SingletonIpl.GetInstance<CountryBll>();
            _nationBll = SingletonIpl.GetInstance<NationBll>();
            _religionBll = SingletonIpl.GetInstance<ReligionBll>();
            _addressBll = SingletonIpl.GetInstance<AddressBll>();
            _recruitChanelBll = SingletonIpl.GetInstance<RecruitChanelBll>();
            _trainingLevelBll = SingletonIpl.GetInstance<TrainingLevelBll>();
            _recruitPlanBll = SingletonIpl.GetInstance<RecruitPlanBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Countries =
                _countryBll.GetCountries()
                    .Select(x => new KendoForeignKeyModel(x.CountryName, x.CountryId.ToString()))
                    .ToList();
            ViewBag.Nations =
                _nationBll.GetNations()
                    .Select(x => new KendoForeignKeyModel(x.NationName, x.NationId.ToString()))
                    .ToList();
            ViewBag.Religions =
                _religionBll.GetReligions()
                    .Select(x => new KendoForeignKeyModel(x.ReligionName, x.ReligionId.ToString()))
                    .ToList();
            ViewBag.Cities =
                _addressBll.GetCities().Select(x => new KendoForeignKeyModel(x.CityName, x.CityId.ToString())).ToList();
            ViewBag.Chanels =
                _recruitChanelBll.GetRecruitChanels(null)
                    .Select(x => new KendoForeignKeyModel(x.ChanelName, x.RecruitChanelId.ToString()))
                    .ToList();
            ViewBag.TrainingLevels =
                _trainingLevelBll.GetTrainingLevels()
                    .Select(x => new KendoForeignKeyModel(x.LevelName, x.TrainingLevelId.ToString()))
                    .ToList();
            ViewBag.RecruitPlans =
                _recruitPlanBll.GetRecruitPlans(null, DateTime.Now.AddYears(-10), DateTime.Now)
                    .Select(x => new KendoForeignKeyModel(x.RecruitPlanCode, x.RecruitPlanId.ToString()))
                    .ToList();
            ViewBag.Sexs = from SexEnum s in Enum.GetValues(typeof (SexEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString())).ToList();
            return View();
        }

        public JsonResult Applicants(long? recruitPlanId)
        {
            return Json(_applicantBll.GetApplicants(recruitPlanId ?? 0), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Applicant(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new Applicant
                {
                    ApplicantId = Guid.Empty.ToString(),
                    CvDate = DateTime.Now
                });
            }
            return PartialView(_applicantBll.GetApplicant(id) ?? new Applicant
            {
                ApplicantId = Guid.Empty.ToString(),
                CvDate = DateTime.Now
            });
        }

        [HttpPost]
        public JsonResult Save(ApplicantModel model)
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
                model.CreateDate = DateTime.Now;
                model.CreateBy = UserLogin.UserId;
                if (string.IsNullOrEmpty(model.ApplicantId) || model.ApplicantId == Guid.Empty.ToString())
                {
                    model.ApplicantId = Guid.NewGuid().ToString();
                    if (_applicantBll.Insert(model.ToObject()))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }

                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_applicantBll.Update(model.ToObject()))
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
                if (_applicantBll.Delete(id))
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