using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.System;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.System.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.System;

namespace AdminSoftware.Areas.System.Controllers
{
    public class UserGroupController : BaseController
    {
        //private readonly UserBll _userBll;
        private readonly UserGroupBll _userGroupBll;

        public UserGroupController()
        {
            _userGroupBll = SingletonIpl.GetInstance<UserGroupBll>();
            //_userBll = SingletonIpl.GetInstance<UserBll>();
        }

        [SystemFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserGroups()
        {
            var userGroups = _userGroupBll.GetUserGroups();
            return Json(userGroups, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserGroup(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new UserGroup());
            }
            return PartialView(_userGroupBll.GetUserGroup(int.Parse(id)));
        }

        public ActionResult UserGroupRight(int id)
        {
            var userGroup = _userGroupBll.GetUserGroup(id);
            ViewBag.UserGroupId = id;
            return PartialView(_userGroupBll.GetUserGroupRightsAuthority(userGroup.UserGroupId));
        }

        [HttpPost]
        public JsonResult Save(UserGroupModel model)
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
                var userGroupInMemory = _userGroupBll.GetUserGroup(model.GroupCode);
                if (model.UserGroupId <= 0)
                {
                    if (userGroupInMemory != null)
                    {
                        return Json(new {Status = 0, Message = "Mã nhóm đã tồn tại trong hệ thống!"},
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_userGroupBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (userGroupInMemory != null && userGroupInMemory.UserGroupId != model.UserGroupId)
                {
                    return Json(new {Status = 0, Message = "Mã nhóm đã tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_userGroupBll.Update(model.ToObject()))
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

        public JsonResult SaveUserGroupFunction(List<UserGroupRights> model, int id)
        {
            try
            {
                if (model == null || !model.Any())
                    return Json(new {Status = 0, Message = "Bạn chưa phân quyền cho nhóm tài khoản"},
                        JsonRequestBehavior.AllowGet);
                if (_userGroupBll.InsertUserGroupRight(id, model))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
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
                //if (_userBll.GetUserByGroup(int.Parse(id)).Any())
                //    return Json(new {Status = 0, Message = MessageAction.DataRelated},
                //        JsonRequestBehavior.AllowGet);
                if (_userGroupBll.Delete(int.Parse(id)))
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