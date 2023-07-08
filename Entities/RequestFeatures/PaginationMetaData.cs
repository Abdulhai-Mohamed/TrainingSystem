using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class PaginationMetaData
    {
        //number of  total items in the list
        public int TotalItems { get; set; }

        //number of Current Page Number
        public int CurrentPageNumber { get; set; }

        //number of items per page
        public int PageSize { get; set; }

        // number ot Total Pages which ==  ( total items / number of items per page ) =>
        // i have 100 item and want display 2 item on each page
        //1 page => 2 items
        //?? page => 100 items
        // ??== (100*1)/2 == 50 total pages
        public int TotalPages { get; set; }


        public bool HasPrevious => CurrentPageNumber > 1;
        public bool HasNext => CurrentPageNumber < TotalPages ;   
    }
}
