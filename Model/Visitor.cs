using System;

namespace DatabaseAdmin.Model
{
    public class Visitor
    {
       
        public int? VisitorID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public int? VisitorBadge { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool BadgeReturned { get; set; }
        public bool NotCheckedOut { get; set; }


        public override string ToString()
        {
            return $"{VisitorID} {Firstname} {Lastname}";
        }
    }
}
