using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace langdiWeb.Models
{
    public class PagingModel
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public static int[] PageSizeValue = new int[] { 20, 4 };
    }
    public class PageArgs
    {
        public int Index { get; set; }
        public int From { get; set; }
        public int Count { get; set; }
        public int To { get; set; }
        public PageArgs(PagingModel pager)
        {
            if (pager == null)
            {
                Index = 1;
                From = 0;
                To = 20;
                Count = 20;
            }
            else
            {
                if (pager.PageSize > PagingModel.PageSizeValue.Length)
                {
                    pager.PageSize = PagingModel.PageSizeValue.Length;
                }
                if (pager.PageSize < 0||pager.PageSize>1)
                {
                    pager.PageSize = 1;
                }
                if (pager.PageIndex < 1)
                {
                    pager.PageIndex = 1;
                }
                int pageSize = PagingModel.PageSizeValue[pager.PageSize];
                this.From = (pager.PageIndex - 1) * pageSize;
                this.To = this.From + pageSize;
                this.Count = pageSize;
                this.Index = pager.PageIndex;
            }
        }
    }
}