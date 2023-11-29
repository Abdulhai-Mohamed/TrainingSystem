namespace Entities.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public PaginationMetaData PaginationMetaData { get; set; }
        public PagedList(List<T> listItems, int totalItems, int pageNumber, int pageSize)
        {
            PaginationMetaData = new PaginationMetaData
            {
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)


            };
            AddRange(listItems);
        }



        public static PagedList<T> ToPagedList(List<T> source, int pageNumber, int pageSize)
        {
            int count = source.Count();
            List<T> CurrentListItems = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            return new PagedList<T>(CurrentListItems, count, pageNumber, pageSize);

        }
    }
}
