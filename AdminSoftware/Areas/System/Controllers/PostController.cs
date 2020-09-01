using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.System;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Areas.System.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;
using Entity.System;

namespace AdminSoftware.Areas.System.Controllers
{
    public class PostController : BaseController
    {
        private readonly PostBll _postBll;
        private readonly UserBll _userBll;
        public PostController()
        {
            _postBll = SingletonIpl.GetInstance<PostBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }
        public ActionResult Index()
        {
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.FullName, x.UserId.ToString()));
            return View();
        }

        public JsonResult Posts()
        {
            return Json(_postBll.GetPosts(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Post(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new Post
                {
                    PostId = 0,                  
                    IsFeature = false,
                    PublishDate = DateTime.Now

                });
            }
            return PartialView(_postBll.GetPost(Int32.Parse(id)));
        }
        public JsonResult Save(PostModel model)
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
                model.CreateDate = DateTime.Now;
                model.CreateBy = UserLogin.UserId;
                if (model.PostId <= 0)
                {                   
                    if (_postBll.Insert(model.ToObject()))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
     
                if (!_postBll.Update(model.ToObject()))
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

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_postBll.Delete(int.Parse(id)))
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