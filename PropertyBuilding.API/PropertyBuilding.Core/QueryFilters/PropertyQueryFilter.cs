namespace PropertyBuilding.Core.QueryFilters
{
    public class PropertyQueryFilter : PaginationFilter
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public double? Price { get; set; }
        public double? PriceInitial { get; set; }
        public double? PriceFinal { get; set; }
        public string? CodeInternal { get; set; }
        public int? Year { get; set; }
        public int? YearInitial { get; set; }
        public int? YearFinal { get; set; }
        public int? IdOwner { get; set; }
    }
}
