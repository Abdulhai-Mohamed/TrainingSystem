namespace Entities.RequestFeatures
{
    public class RequestParameters
    {
        const int maxPageSize = 50;
        public int pageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = 10;
        public int pageSize
        {

            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }



        public string orderBy { get; set; }
    }

    public class EmployeeParameters : RequestParameters
    {
        public EmployeeParameters()
        {
            orderBy = "name";//set the name as default orderBY for employees
        }
        public uint MinAge { get; set; } = 1;
        public uint MaxAge { get; set; } = int.MaxValue;
        public bool ValidRange => MinAge < MaxAge;


        public string SearchTerm { get; set; }
    }


}
