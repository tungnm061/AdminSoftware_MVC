using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.System.Models.Kpi;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Controllers.Kpi
{
    public class KpiConfigController : BaseController
    {
        private readonly AcceptConfigBll _acceptConfigBll;
        private readonly FactorConfigBll _factorConfigBll;
        private readonly FinishJobConfigBll _finishConfigBll;
        private readonly SuggestJobConfigBll _jobConfigBll;
        private readonly KpiConfigBll _kpiConfigBll;

        public KpiConfigController()
        {
            _kpiConfigBll = SingletonIpl.GetInstance<KpiConfigBll>();
            _jobConfigBll = SingletonIpl.GetInstance<SuggestJobConfigBll>();
            _acceptConfigBll = SingletonIpl.GetInstance<AcceptConfigBll>();
            _finishConfigBll = SingletonIpl.GetInstance<FinishJobConfigBll>();
            _factorConfigBll = SingletonIpl.GetInstance<FactorConfigBll>();
        }
        [SystemFilter]
        public ActionResult Index()
        {
            ViewBag.DayNum = from DayNum s in Enum.GetValues(typeof (DayNum))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            return View(_kpiConfigBll.GetKpiConfig());
        }

        public JsonResult Save(KpiConfigModel model)
        {
            try
            {
                return Json(_kpiConfigBll.Update(model.ToObject())
                    ? new {Status = 1, Message = MessageAction.MessageUpdateSuccess}
                    : new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        // Controller ConfigPoint
        public JsonResult JobConfigs()
        {
            return Json(_jobConfigBll.GetJobConfigs(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FinishConfigs()
        {
            return Json(_finishConfigBll.GetFinishConfigs(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AcceptConfigs()
        {
            return Json(_acceptConfigBll.GetAcceptConfigs(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FactorConfigs()
        {
            return Json(_factorConfigBll.GetFactorConfigs(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveJob(SuggetsJobConfig model)
        {
            try
            {
                if (
                    _jobConfigBll.GetSuggestJobCheckPoint(model.JobConfigId,
                        model.JobConditionMin, model.JobConditionMax) != null)
                {
                    return Json(new {Status = 0, Message = " Cấu hình điểm và điều kiện không đúng định dạng! "},
                        JsonRequestBehavior.AllowGet);
                }
                _jobConfigBll.Update(model);
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

        public JsonResult SaveFinish(FinishJobConfig model)
        {
            try
            {
                if (
                    _finishConfigBll.GetFinishJobConfigCheck(model.FinishConfigId,
                        model.FinishConditionMin, model.FinishConditionMax) != null)
                {
                    return Json(new {Status = 0, Message = " Cấu hình điểm và điều kiện không đúng định dạng! "},
                        JsonRequestBehavior.AllowGet);
                }
                _finishConfigBll.Update(model);
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

        public JsonResult SaveAccept(AcceptConfig model)
        {
            try
            {
                if (
                    _acceptConfigBll.GetAcceptConfigCheck(
                        model.AcceptConfigId,
                        model.AcceptConditionMin, model.AcceptConditionMax) != null)
                {
                    return Json(new {Status = 0, Message = " Cấu hình điểm và điều kiện không đúng định dạng! "},
                        JsonRequestBehavior.AllowGet);
                }
                _acceptConfigBll.Update(model);
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

        public JsonResult SaveFactor(FactorConfig model)
        {
            try
            {
                if (
                    _factorConfigBll.GetFactorConfigCheck(
                        model.FactorConfigId,
                        model.FactorConditionMin, model.FactorConditionMax) != null)
                {
                    return Json(new {Status = 0, Message = " Cấu hình điểm và điều kiện không đúng định dạng! "},
                        JsonRequestBehavior.AllowGet);
                }
                _factorConfigBll.Update(model);
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
    }
}