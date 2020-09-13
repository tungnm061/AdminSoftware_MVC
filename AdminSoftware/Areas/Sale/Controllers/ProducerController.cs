using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Sale;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Sale.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Sale;


namespace AdminSoftware.Areas.Sale.Controllers
{
    public class ProducerController : BaseController
    {
        private readonly AutoNumberBll _autoNumberBll;
        private readonly ProducerBll _producerBll;
        private readonly UserBll _userBll;

        public ProducerController()
        {
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _producerBll = SingletonIpl.GetInstance<ProducerBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }

        [SaleFilter]
        public ActionResult Index()
        {
            ViewBag.Users =
                _userBll.GetUsers(null)
                    .Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString()));
            return View();
        }

        public JsonResult Producers()
        {
            return Json(_producerBll.GetProducers(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Producer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new Producer
                {
                    ProducerCode = _autoNumberBll.GetAutoNumber(AutoNumberSale.SX.ToString()),
                    ProducerId = 0,
                    IsActive = true
                });
            }
            return PartialView(_producerBll.GetProducer(long.Parse(id)));
        }

        public JsonResult Save(Producer model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
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

                if (model.ProducerId <= 0)
                {
                    model.CreateDate = DateTime.Now;
                    model.CreateBy = UserLogin.UserId;
                    model.ProducerCode = AutoNumberSale.SP.ToString();
                    if (_producerBll.Insert(model) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                model.UpdateDate = DateTime.Now;
                model.UpdateBy = UserLogin.UserId;

                if (_producerBll.Update(model))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = 0,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
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
                if (_producerBll.Delete(long.Parse(id)))
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

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}