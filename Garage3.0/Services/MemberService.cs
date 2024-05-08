using Garage3._0.Data;
using Garage3._0.Models;
using Garage3._0.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Garage3._0.Services;

public class MemberService : IMemberService {
    private readonly GarageDbContext _context;
    private readonly IVehicleService _vehicleService;

    public MemberService(GarageDbContext context, IVehicleService vehicleService) {
        _context = context;
        _vehicleService = vehicleService;
    }


    public async Task<MembersIndexViewModel> GetMembersIndexViewModelAsync(string? searchQuery) {
        var memberViewModels = await GetMemberViewModelsAsync(searchQuery);
        var model = new MembersIndexViewModel {
            Members = memberViewModels,
            SearchQuery = searchQuery ?? string.Empty,
        };
        return model;
    }

    public async Task<List<MemberViewModel>> GetMemberViewModelsAsync(string? searchQuery) {
        // Retrieve members and associated vehicles with their parking events and vehicle types
        var membersQuery = _context.Members
            .Include(m => m.VehicleList)
            .ThenInclude(v => v.VehicleType)
            .Include(m => m.VehicleList)
            .ThenInclude(v => v.ParkingEvent)
            .Include(m => m.Membership)
            .AsQueryable();


        if (!string.IsNullOrEmpty(searchQuery)) {
            membersQuery = membersQuery.Where(m => (m.Firstname + " " + m.Surname).Contains(searchQuery));
        }

        var members = await membersQuery.ToListAsync();
        // Map members to view models
        var memberViewModels = members.Select(m => new MemberViewModel {
                MemberId = m.MemberId,
                FirstName = m.Firstname,
                Surname = m.Surname,
                SocialSecurityNr = m.SocialSecurityNr.ToString(),
                MembershipType = GetAndUpdateMembershipType(m.MemberId),
                StartDate = m.Membership.StartDate,
                Vehicles = m.VehicleList.Select(v => _vehicleService.CreateVehicleViewModel(v, m, m.Membership)).ToList()
            })
            .ToList();

        memberViewModels = memberViewModels
            .OrderBy(m => m.FirstName.Length >= 2 ? m.FirstName[..2] : m.FirstName)
            .ToList();

        return memberViewModels;
    }



    public void RegisterMember(Member member) {
        //handle member data creating and saving, including membership generation
        var membership = new Membership {
            Type = Membership.MembershipType.Premium,
            StartDate = DateTime.Now,
            MemberID = member.MemberId,
            Member = member
        };
        int age = DateTime.Today.Year - member.BirthDate.Year;
        //30 days for all member, 2 years for 65+
        if (age >= 65) {
            membership.SetYearsDuration(2);
        }
        else {
            membership.SetDuration(30);
        }

        member.Membership = membership;
        _context.Members.Add(member);
        _context.Memberships.Add(membership);
        _context.SaveChanges();
    }

    private Membership.MembershipType GetAndUpdateMembershipType(int memberId) {
        var member = _context.Members
            .Include(m => m.Membership)
            .FirstOrDefault(m => m.MemberId == memberId);
        if (member != null) {
            if (member.Membership.Type == Membership.MembershipType.Premium) {
                if (DateTime.Now > member.Membership.EndDate) {
                    member.Membership.Type = Membership.MembershipType.Standard;
                    _context.Memberships.Update(member.Membership);
                    _context.SaveChanges();
                }
                else {
                    return Membership.MembershipType.Premium;
                }
            }

            return Membership.MembershipType.Standard;
        }
        else {
            return Membership.MembershipType.Standard;
        }
    }
}