using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAdmin.DatabaseConnections;
using Npgsql;
using System.Configuration;
using DatabaseAdmin.Enums;

namespace DatabaseAdmin.Model
{
    static public class VisitorHelperClass
    {

        //static public string ReturnVisitorName(string vFirstname, string vLastname)
        //{
        //    var sql = new StringBuilder();
        //    sql.AppendLine("WHERE 1=1");

        //    if (vFirstname != null /*|| vLastname == null*/)
        //    {
        //        sql.AppendFormat($"visitor.firstname LIKE UPPER ({vFirstname})");
        //        sql.AppendLine();

        //    }
        //    if (vLastname != null)
        //    {
        //        sql.AppendFormat($"visitor.lastname LIKE UPPER({vLastname})");
        //        sql.AppendLine();
        //    }
        //    var query = sql.ToString();
        //    return query;
        //    //else
        //    //{
        //    //    //var test = $" WHERE visitor.firstname LIKE UPPER( {vFirstname}) AND UPPER visitor.lastname LIKE UPPER( {vLastname})";
        //    //    return $" WHERE visitor.firstname LIKE UPPER( {vFirstname}) AND UPPER visitor.lastname LIKE UPPER( {vLastname})";
        //    //}
        //}

    }
}
