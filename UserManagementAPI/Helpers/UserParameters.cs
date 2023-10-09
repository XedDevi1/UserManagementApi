namespace UserManagementAPI.Helpers
{
    public class UserParameters
    {
        const int maxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

        public string SortBy { get; set; } = "Id";

        public bool SortDescending { get; set; } = false;

        public string FilterName { get; set; }

        public int? FilterAge { get; set; }

        public string FilterEmail { get; set; }
    }
}
