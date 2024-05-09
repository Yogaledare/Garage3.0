namespace Garage3._0.Models.ViewModels
{
    public class SearchFormViewModel
    {
        public string SearchQuery { get; set; } = string.Empty;
        public string FormAction { get; set; } = string.Empty;
        public string FormController { get; set; } = string.Empty;
        public string PlaceholderText { get; set; } = "Search...";
    }
}