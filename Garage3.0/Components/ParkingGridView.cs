using Microsoft.AspNetCore.Mvc;
using Garage3._0.Models;

namespace Garage3._0.Components
{
    public class ParkingGridView : ViewComponent
    {
        private readonly IGarageManager _manager;
        public ParkingGridView(IGarageManager garageManager)
        {
            _manager = garageManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View(_manager));
        }
    }
}
