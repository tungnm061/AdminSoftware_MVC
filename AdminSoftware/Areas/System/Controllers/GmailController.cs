using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using BusinessLogic.System;
using Core.Helper.Logging;
using Core.Singleton;
using DuyAmazone.Areas.Printify.Models;
using Entity.System;

namespace AdminSoftware.Areas.System.Controllers
{
    public class GmailController : BaseController
    {

        private readonly GmailBll _gmailBll;
        private readonly UserBll _userBll;
        private readonly SkuBll _skuBll;

        public GmailController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _gmailBll = SingletonIpl.GetInstance<GmailBll>();
            _skuBll = SingletonIpl.GetInstance<SkuBll>();
        }

        [SystemFilter]
        public ActionResult Index()
        {
            var users = _userBll.GetUsers(null);
            ViewBag.Users = users.Select(x => new KendoForeignKeyModel { value = x.UserId.ToString(), text = x.FullName });

            return View();
        }

        public ActionResult Gmails()
        {
            var gmails = _gmailBll.GetGmails();
            return Json(gmails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Gmail(int id)
        {
            if (id == 0)
            {
                return PartialView(new Gmail
                {
                    Id = 0,
                    IsActive = true
                });
            }
            return PartialView(_gmailBll.GetGmail(id));
        }


        [HttpPost]
        public JsonResult Save(Gmail model)
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

                var gmail = _gmailBll.GetGmailByName(model.FullName);
                if (model.Id == 0)
                {
                    if (gmail != null)
                    {
                        return Json(new { Status = 0, Message = "Tài khoản này đã tồn tại trong hệ thống!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    model.CreateBy = UserLogin.UserId;
                    model.CreateDate = DateTime.Now;
                    model.IsActive = true;
                    if (_gmailBll.Insert(model) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (gmail != null && gmail.Id != model.Id)
                    {
                        return Json(new { Status = 0, Message = "Tên tài khoản đã tồn tại trong hệ thống!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    model.UpdateBy = UserLogin.UserId;
                    model.UpdateDate = DateTime.Now;
                    model.IsActive = true;
                    if (_gmailBll.Update(model))
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
                if (_gmailBll.Delete(int.Parse(id)))
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

        #region SKU

        public ActionResult SkuView(int gmailId)
        {
            return PartialView(_gmailBll.GetGmail(gmailId));
        }

        public ActionResult Sku(int id)
        {
            if (id == 0)
            {
                return PartialView(new Sku
                {
                    Id = 0
                });
            }
            return PartialView(_skuBll.GetSku(id));
        }

        public JsonResult Skus(int gmailId)
        {
            return Json(_skuBll.GetSkus(gmailId),
                   JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSku(SkuModel model)
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

                if (model.Id == 0)
                {
                    model.CreateBy = UserLogin.UserId;
                    model.CreateDate = DateTime.Now;
                    if (_skuBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    model.UpdateBy = UserLogin.UserId;
                    model.UpdateDate = DateTime.Now;
                    if (_skuBll.Update(model.ToObject()))
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
        public JsonResult DeleteSku(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_skuBll.Delete(int.Parse(id)))
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

        #endregion

        public JsonResult ClearSession()
        {
            return null;
        }


    }
}