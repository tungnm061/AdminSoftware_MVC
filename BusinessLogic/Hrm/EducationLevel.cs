using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class EducationLevelBll
    {
        private readonly EducationLevelDal _trainingLevelDal;

        public EducationLevelBll()
        {
            _trainingLevelDal = SingletonIpl.GetInstance<EducationLevelDal>();
        }

        public List<EducationLevel> GetEducationLevels()
        {
            return _trainingLevelDal.GetEducationLevels();
        }

        public EducationLevel GetEducationLevel(int trainingLevelId)
        {
            return _trainingLevelDal.GetEducationLevel(trainingLevelId);
        }

        public EducationLevel GetEducationLevel(string trainingLevelCode)
        {
            return _trainingLevelDal.GetEducationLevel(trainingLevelCode);
        }

        public int Insert(EducationLevel trainingLevel)
        {
            return _trainingLevelDal.Insert(trainingLevel);
        }

        public bool Update(EducationLevel trainingLevel)
        {
            return _trainingLevelDal.Update(trainingLevel);
        }

        public bool Delete(int trainingLevelId)
        {
            return _trainingLevelDal.Delete(trainingLevelId);
        }
    }
}