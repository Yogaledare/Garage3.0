namespace Garage3._0.Models.ViewModels;

public class MemberViewModel {
    public int MemberId { get; set; }
    public string FirstName { get; set;  }
    public string Surname { get; set; }
    public List<VehicleViewModel> Vehicles { get; set; }
    public string SocialSecurityNr { get; set; }
    public int VehicleCount => Vehicles.Count;
    public string Name => $"{FirstName} {Surname}"; 
    public Membership.MembershipType MembershipType { get; set; }  
}