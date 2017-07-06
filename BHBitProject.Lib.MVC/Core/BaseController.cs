using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace BBP.MVC.Core
{
  
    public class BaseController : Controller, IDisposable
    {

        #region [Properties]
#if DEBUG
        private const string conectionStringName = "strConexaoDesenvolvimento";
#else
        private const string conectionStringName = "strConexaoProducao";
#endif

        private TransactionScope transaction { get; set; }

        #endregion

        protected bool IsContextInTransaction = true;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (this.IsContextInTransaction)
                transaction = new TransactionScope();

            base.OnActionExecuting(filterContext);
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            this.Dispose(true);
        }

        public new void Dispose()
        {
            base.Dispose();
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.IsContextInTransaction)
                {
                    transaction.Complete();
                    transaction.Dispose();
                }
            }

            base.Dispose(disposing);
        }

    }
}