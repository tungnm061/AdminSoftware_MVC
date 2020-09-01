using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class TrainingLevelBll
    {
        private readonly TrainingLevelDal _trainingLevelDal;

        public TrainingLevelBll()
        {
            _trainingLevelDal = SingletonIpl.GetInstance<TrainingLevelDal>();
        }

        public List<TrainingLevel> GetTrainingLevels()
        {
            return _trainingLevelDal.GetTrainingLevels();
        }

        public TrainingLevel GetTrainingLevel(int trainingLevelId)
        {
            return _trainingLevelDal.GetTrainingLevel(trainingLevelId);
        }

        public TrainingLevel GetTrainingLevel(string trainingLevelCode)
        {
            return _trainingLevelDal.GetTrainingLevel(trainingLevelCode);
        }

        public int Insert(TrainingLevel trainingLevel)
        {
            return _trainingLevelDal.Insert(trainingLevel);
        }

        public bool Update(TrainingLevel trainingLevel)
        {
            return _trainingLevelDal.Update(trainingLevel);
        }

        public bool Delete(int trainingLevelId)
        {
            return _trainingLevelDal.Delete(trainingLevelId);
        }
    }
}