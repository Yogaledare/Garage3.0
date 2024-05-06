using Garage3._0.Models;
using Garage3._0.Models.ViewModels;

namespace Garage3._0.Services;

public interface IMemberService {
    Task<List<MemberViewModel>> GetAllMemberViewModelsAsync();
    VehicleViewModel CreateVehicleViewModel(Vehicle vehicle, Member member);
}