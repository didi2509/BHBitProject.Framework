using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace BBP.MVC.Core
{
  
    public class BBPBaseTransactionController : Controller, IDisposable
    {
        protected TransactionScope transaction { get; set; }
       

        protected bool IsContextInTransaction = true;

        protected new virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (this.IsContextInTransaction)
                transaction = new TransactionScope();

            base.OnActionExecuting(filterContext);
        }

        protected new virtual void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            this.Dispose(true);
        }

        public virtual new void Dispose()
        {
            base.Dispose();
        }

        protected virtual new void Dispose(bool disposing)
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