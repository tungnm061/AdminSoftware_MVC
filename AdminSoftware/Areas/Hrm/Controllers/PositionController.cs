using System;
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
    public class PositionController : BaseController
    {
        private readonly PositionBll _positionBll;

        public PositionController()
        {
            _positionBll = SingletonIpl.GetInstance<PositionBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Positions()
        {
            var positions = _positionBll.GetPositions();
            return Json(positions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Position(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new Position());
            return PartialView(_positionBll.GetPosition(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult Save(PositionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new {Status = 0, Message = MessageAction.ModelStateNotValid},
                        JsonRequestBehavior.AllowGet);
                var position = _positionBll.GetPosition(model.PositionCode);
                if (model.PositionId == 0)
                {
                    if (position != null)
                    {
                        return Json(new {Status = 0, Message = "Mã chức vụ đã tồn tại trong hệ thống!"},
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_positionBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }

                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (position != null && position.PositionId != model.PositionId)
                {
                    return Json(new {Status = 0, Message = "Mã chức vụ đã tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_positionBll.Update(model.ToObject()))
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
                if (_positionBll.Delete(int.Parse(id)))
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