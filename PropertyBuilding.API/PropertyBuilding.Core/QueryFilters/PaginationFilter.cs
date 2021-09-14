namespace PropertyBuilding.Core.QueryFilters
{
    public abstract class PaginationFilter
    {
        public int page { get; set; }
        public int rows { get; set; }
    }
}
