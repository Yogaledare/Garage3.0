namespace Garage3._0.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public int SocialSecurityNr { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public ICollection<Vehicle> VehicleList { get; set; }
    }
}
