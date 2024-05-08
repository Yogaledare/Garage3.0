namespace Garage3._0.Models.ViewModels;

public class MembersIndexViewModel {
    public string? SearchQuery { get; set; }
    public List<MemberViewModel> Members { get; set; } = [];
}