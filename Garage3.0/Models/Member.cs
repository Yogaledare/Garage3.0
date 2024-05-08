namespace Garage3._0.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string SocialSecurityNr { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public ICollection<Vehicle> VehicleList { get; set; } = [];

        public DateTime BirthDate {
            get {
                var year = int.Parse(SocialSecurityNr[..4]);
                var month = int.Parse(SocialSecurityNr[4..6]);
                var day = int.Parse(SocialSecurityNr[6..8]);

                return new DateTime(year, month, day); 
            }
        }
    }
}
