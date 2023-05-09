using Microsoft.EntityFrameworkCore;
using NewsWeb.Models.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NewsWeb.Models.Repository
{
    public class CRUDGeneric<Tentity> where Tentity : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Tentity> _table;

        public CRUDGeneric(ApplicationDbContext context)
        {
            _context = context;
            _table = context.Set<Tentity>();
        }

        // متد اضافه کردن داده جدید در پایگاه داده
        public virtual void Create(Tentity entity)
        {
            _table.Add(entity);
        }

        // متد بروزرسانی داده ها در پایگاه داده
        public virtual void Update(Tentity entity)
        {
            _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        // متد حذف داده ها در پایگاه داده
        public virtual void Delete(Tentity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _table.Attach(entity);
            }
            _table.Remove(entity);
        }

        // متد حذف داده ها با استفاده از پیدا کردن آی دی موجودیت در پایگاه داده
        public virtual void DeleteById(object id)
        {
            var entity = GetById(id);
            Delete(entity);
        }

        // متد بازگشتی مربوط به نوع موجودیت داده در پایگاه داده
        public virtual Tentity GetById(object id)
        {
            return _table.Find(id);
        }

        // متد ذخیره سازی داده ها در پایگاه داده
        public virtual void Save()
        {
            _context.SaveChanges();
        }
        
        // متد دریافت لیستی از درخواست ها به همراه مرتب سازی و ادغام از پایگاه داده
        public virtual IEnumerable<Tentity> Get(Expression<Func<Tentity, bool>> whereVariable = null, 
            Func<IQueryable<Tentity>, 
            IOrderedQueryable<Tentity>> orderbyVariable = null, string joinString = "")
        {
            //=====================================================
            IQueryable<Tentity> query = _table;
            if (whereVariable != null)
            {
                query = query.Where(whereVariable);
            }
            //=====================================================
            if (orderbyVariable != null)
            {
                query = orderbyVariable(query);
            }
            //=====================================================
            if (joinString != "")
            {
                foreach (string joins in joinString.Split(','))
                {
                    query = query.Include(joins);
                }
            }

            return query.ToList();

        }
        public void DeleteRange(IEnumerable<Tentity> entities) => _table.RemoveRange(entities);

    }
}
