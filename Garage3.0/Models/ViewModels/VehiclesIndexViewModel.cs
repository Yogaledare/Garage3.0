namespace Garage3._0.Models.ViewModels;

public class VehiclesIndexViewModel {
    public string SearchQuery { get; set; } = string.Empty;

    public IEnumerable<VehicleViewModel> Vehicles { get; set; } = [];
}