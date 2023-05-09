using NewsWeb.Models.DatabaseContext;
using NewsWeb.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Models.UnitOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        #region متد مربوط به کنترلر جدول دسته بندی ها
        private CRUDGeneric<TBL_Category> _CategoryRepositoryUW;
        public CRUDGeneric<TBL_Category> CategoryRepositoryUW
        {
            get
            {
                if (_CategoryRepositoryUW == null)
                {
                    _CategoryRepositoryUW = new CRUDGeneric<TBL_Category>(_context);
                }
                return _CategoryRepositoryUW;
            }
        }
        #endregion

        #region متد مربوط به کنترلر جدول اخبار
        private CRUDGeneric<TBL_News> _NewsRepositoryUW;
        public CRUDGeneric<TBL_News> NewsRepositoryUW
        {
            get
            {
                if (_NewsRepositoryUW == null)
                {
                    _NewsRepositoryUW = new CRUDGeneric<TBL_News>(_context);
                }
                return _NewsRepositoryUW;
            }
        }
        #endregion

        #region متد مربوط به کنترلر جدول کاربران
        private CRUDGeneric<ApplicationUsers> _userManager;
        public CRUDGeneric<ApplicationUsers> userManagerUW
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = new CRUDGeneric<ApplicationUsers>(_context);
                }
                return _userManager;
            }
        }
        #endregion

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
