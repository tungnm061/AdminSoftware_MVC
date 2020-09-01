using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;
using Entity.System;

namespace BusinessLogic.Hrm
{
    public class PostBll
    {
        private readonly PostDal _postDal;

        public PostBll()
        {
            _postDal = SingletonIpl.GetInstance<PostDal>();
        }

        public List<Post> GetPosts()
        {
            return _postDal.GetPosts();
        }

        public Post GetPost(int postId)
        {
            return _postDal.GetPost(postId);
        }

        public bool Insert(Post post)
        {
            return _postDal.Insert(post);
        }
        public bool Update(Post post)
        {
            return _postDal.Update(post);
        }
        public bool Delete(int postId)
        {
            return _postDal.Delete(postId);
        }
    }
}
