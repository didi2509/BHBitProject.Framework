using BBP.DAL.Entity;
using BBP.DAL.Entity.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBP.BLL.Entity.Base
{
    public class BaseBLL<BaseDALType, EntityType> 
        where BaseDALType : BaseDAL<EntityType> , new()
        where EntityType:class, IObjectWithKey
    {
        public BaseBLL(DbContext _context)
        {
            this.Context = _context;
            DAL = new BaseDALType();
            DAL.SetContext(_context);
        }

        protected DbContext Context { get; set; }
        protected BaseDALType DAL { get; set; }


    }
}
