using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Singleton;
using DataAccess.Sale;
using Entity.Sale;

namespace BusinessLogic.Sale
{
   public class ProjectBll
   {
       private readonly ProjectDal _projectDal;

       public ProjectBll()
       {
           _projectDal = SingletonIpl.GetInstance<ProjectDal>();
       }

       public List<Project> GetProjects()
       {
           return _projectDal.GetProjects();
       }
        public Project GetProject(string id)
        {
            return _projectDal.GetProject(id);
        }

       public bool Insert(Project project)
       {
           return _projectDal.Insert(project);
       }
        public bool Update(Project project)
        {
            return _projectDal.Update(project);
        }

       public bool Delete(string id)
       {
            return _projectDal.Delete(id);
        }
    }
}
