namespace DatabaseAdmin.Model
{
    public class Admin : Employee
    {
        public int? AdminID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
