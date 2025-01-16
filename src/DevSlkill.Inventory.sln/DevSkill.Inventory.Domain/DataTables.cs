using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain
{
    public abstract class DataTables
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public SortColumn[] Order { get; set; }
        public DataTablesSearch Search { get; set; }

        public int PageIndex
        {
            get
            {
                if (Length > 0)
                    return (Start / Length) + 1;
                else
                    return 1;
            }
        }

        public int PageSize
        {
            get
            {
                if (Length == 0)
                    return 10;
                else
                    return Length;
            }
        }

        public static object EmptyResult
        {
            get
            {
                return new
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = (new string[] { }).ToArray()
                };
            }
        }

        public string? FormatSortExpression(params string[] columns)
        {
            StringBuilder columnBuilder = new StringBuilder();

            for (int i = 0; i < Order.Length; i++)
            {
                columnBuilder.Append(columns[Order[i].Column])
                .Append(" ")
                .Append(Order[i].Dir);

                if (i < Order.Length - 1)
                    columnBuilder.Append(", ");
            }
            var orderString = columnBuilder.ToString();
            return orderString == string.Empty ? null : orderString;
        }
    }

    public struct SortColumn
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public struct DataTablesSearch
    {
        public bool Regex { get; set; }
        public string Value { get; set; }
    }
}

