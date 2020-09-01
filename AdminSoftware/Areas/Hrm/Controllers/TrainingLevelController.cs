using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class TrainingLevelController : BaseController
    {
        private readonly TrainingLevelBll _trainingLevelBll;

        public TrainingLevelController()
        {
            _trainingLevelBll = SingletonIpl.GetInstance<TrainingLevelBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult TrainingLevels()
        {
            var trainingLevels = _trainingLevelBll.GetTrainingLevels();
            return Json(trainingLevels, JsonRequestBehavior.AllowGet);
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
        public JsonResult Save(TrainingLevelModel model)
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
                        return Json(new {Status = 0, Message = "Mã chức vụ đã tồn tại trong hệ thống!"},
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_trainingLevelBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }

                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (trainingLevel != null && trainingLevel.TrainingLevelId != model.TrainingLevelId)
                {
                    return Json(new {Status = 0, Message = "Mã chức vụ đã tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_trainingLevelBll.Update(model.ToObject()))
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
                if (_trainingLevelBll.Delete(int.Parse(id)))
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