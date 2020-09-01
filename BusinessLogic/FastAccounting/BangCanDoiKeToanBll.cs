using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class BangCanDoiKeToanBll
    {
        private readonly BangCanDoiKeToanDal _bangCanDoiKeToanDal;

        public BangCanDoiKeToanBll()
        {
            _bangCanDoiKeToanDal = SingletonIpl.GetInstance<BangCanDoiKeToanDal>();
        }

        public List<BangCanDoiKeToan> GetBangCanDoiKeToans(DateTime? dateTime, int buTru, string maDvcs)
        {
            return _bangCanDoiKeToanDal.GetBangCanDoiKeToans(dateTime, buTru, maDvcs);
        }
    }
}
