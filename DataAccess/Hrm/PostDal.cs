using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;
using Entity.System;

namespace DataAccess.Hrm
{
    public class PostDal : BaseDal<ADOProvider>
    {
        public List<Post> GetPosts()
        {
            try
            {
                return UnitOfWork.Procedure<Post>("[dbo].[Get_Posts]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Post>();
            }
        }

        public Post GetPost(int postId)
        {
            try
            {
                return UnitOfWork.Procedure<Post>("[dbo].[Get_Post]", new
                {
                    PostId = postId
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new Post();
            }
        }

        public bool Insert(Post post)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PostId", post.PostId);
                param.Add("@Title", post.Title);
                param.Add("@PostContent", post.PostContent);
                param.Add("@CreateDate", post.CreateDate);
                param.Add("@CreateBy", post.CreateBy);
                param.Add("@PublishDate", post.PublishDate);
                param.Add("@IsFeature", post.IsFeature);
                return UnitOfWork.ProcedureExecute("[dbo].[Insert_Post]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(Post post)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PostId", post.PostId);
                param.Add("@Title", post.Title);
                param.Add("@PostContent", post.PostContent);
                param.Add("@CreateDate", post.CreateDate);
                param.Add("@CreateBy", post.CreateBy);
                param.Add("@PublishDate", post.PublishDate);
                param.Add("@IsFeature", post.IsFeature);
                return UnitOfWork.ProcedureExecute("[dbo].[Update_Post]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int postId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[dbo].[Delete_Post]", new
                {
                    PostId = postId
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}