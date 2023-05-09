using NewsWeb.Models.DatabaseContext;
using NewsWeb.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Models.UnitOfWork
{
    public interface IUnitOfWork
    {
        CRUDGeneric<TBL_Category> CategoryRepositoryUW { get; }
        CRUDGeneric<TBL_News> NewsRepositoryUW { get; }
        CRUDGeneric<ApplicationUsers> userManagerUW { get; }
        void Save();
    }
}
