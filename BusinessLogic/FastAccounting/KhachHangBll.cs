using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class KhachHangBll
    {
        private readonly KhachHangDal _khachHangDal;

        public KhachHangBll()
        {
            _khachHangDal = SingletonIpl.GetInstance<KhachHangDal>();
        }

        public List<KhachHang> GetKhachHangs()
        {
            return _khachHangDal.GetKhachHangs();
        }
    }
}