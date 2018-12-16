using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Common
{
    public static class IntegerConstants
    {
        public const int ItemsPerPage = 5;
        public const int NumberOfPagesPossibletoNavigateTo = 5; /*This means that in the pagination rendering only five buttons with numbers will be rendered - 
                                                                if the current page is 5, the visible buttons will be for 2 pages before, meaning pages 3 and 4,
                                                                and 2 pages after, meaning pages 6 and 7.*/
        public const int NumberOfPagesOffset = 2; /*This helps the above mentioned functionality. This number is added and subtracted from 
                                                   the current page that the user is on. So, if the user is on the third page, the first visible
                                                   will be number 1 and the last visible will be number 5.*/
    }
}
