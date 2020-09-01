using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class RecruitChanelBll
    {
        private readonly RecruitChanelDal _recruitChanelDal;

        public RecruitChanelBll()
        {
            _recruitChanelDal = SingletonIpl.GetInstance<RecruitChanelDal>();
        }

        public List<RecruitChanel> GetRecruitChanels(bool? isActive)
        {
            return _recruitChanelDal.GetRecruitChanels(isActive);
        }

        public RecruitChanel GetRecruitChanel(int recruitChanelId)
        {
            return _recruitChanelDal.GetRecruitChanel(recruitChanelId);
        }

        public int Insert(RecruitChanel recruitChanel)
        {
            return _recruitChanelDal.Insert(recruitChanel);
        }

        public bool Update(RecruitChanel recruitChanel)
        {
            return _recruitChanelDal.Update(recruitChanel);
        }

        public bool Delete(int recruitChanelId)
        {
            return _recruitChanelDal.Delete(recruitChanelId);
        }
    }
}