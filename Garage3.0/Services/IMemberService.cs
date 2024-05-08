using Garage3._0.Models;
using Garage3._0.Models.ViewModels;

namespace Garage3._0.Services;

public interface IMemberService {
    Task<List<MemberViewModel>> GetMemberViewModelsAsync(string? searchQuery);
    // VehicleViewModel CreateVehicleViewModel(Vehicle vehicle, Member member, Membership membership);
    Task<MembersIndexViewModel> GetMembersIndexViewModelAsync(string? searchQuery);

    void RegisterMember(Member member);
}