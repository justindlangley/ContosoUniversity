using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Web;
using ContosoUniversity.Logging;

namespace ContosoUniversity.DAL
{
    public class SchoolInterceptorLogging
    {
        private ILogger _logger = new Logger();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public override void ScalarExecuting(DbCommand command,
     DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void ScalarExecuted (DbCommand command, 
            DbCommandInterceptionContect<object> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception !=null)
            {
                _logger.Error(interceptionContext.Exception, 
                    "Error executing command: {0}", command.CommandText);
            }
            else
            {
                _logger.TraceApi("SQQL Database", 
                    "SchoolInterceptor.scalarExecuted", _stopwatch.Elapsed, "Command: {0}:", command.CommandText);
            }
            base.ScalarExecuted(command, interceptionContext);
        }
        public override void NonQueryExecuting(DbCommand command,
            DbCommandInterceptionContext<int> interceptionContext)
            {
                base.NonQueryExecuting(command, interceptionContext);
                _stopwatch.Restart();
            }
        public override void NonQueryExecuting(DbCommand command,
            DbCommandInterceptionContext<int> interceptionContext)
            {
                _stopwatch.Stop();
                if (interceptionContext.Exception != null)
                {
                    _logger.Error(interceptionContext.Exception, "Error executing command: {0}",
                        command.CommandText);
                }
                else
                {
                    _logger.TraceApi("SQL Database", "SchoolInterceptor.NonQueryExecuted",
                        _stopwatch.Elapsed, "Command: {0} ", command.CommandText);
                }
                base.NonQueryExecuted(command, interceptionContext);
                
            }
        
        }
}