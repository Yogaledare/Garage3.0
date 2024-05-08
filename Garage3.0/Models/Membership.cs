namespace Garage3._0.Models
{
    public class Membership
    {
        public enum MembershipType
        {
            Standard,
            Premium
        }
        public int MembershipId { get; set; }
        public MembershipType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MemberID { get; set; }  //foreign key
        public Member Member { get; set; } //reference navigation to member

        public void SetDuration(int days = 30)
        {
            EndDate = StartDate.AddDays(days);
        }
        public void SetYearsDuration(int years = 2)
        {
            EndDate = StartDate.AddYears(years);
        }
    }
}
