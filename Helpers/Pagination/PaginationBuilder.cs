namespace Api.Helpers.Pagination
{
    public static class PaginationBuilder<T>
    {
        public static object ToPagination(IList<T> data, int page = 1, int itemsPerPage = 20)
        {
            var items  = data.Skip((page - 1) * itemsPerPage)
                             .Take(itemsPerPage)
                             .ToList();

            var totalPages = Math.Ceiling(data.Count() / (float)itemsPerPage);

            return new
            {
                items,
                currentPage = page,
                totalItems = data.Count(),
                totalPages = (int)totalPages
            };
        }
    }
}
