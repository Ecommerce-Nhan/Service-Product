using Microsoft.EntityFrameworkCore;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;
using System.Linq.Dynamic.Core;

namespace ProductService.Infrastructure.Helpers;

public static class EntityFrameworkHelpers
{
    public static async Task<PagedResponse<List<T>>> ToPagedResponseListAsync<T>(this IQueryable<T> source, PageRequest request)
    {
        string? sort = request.SortBy;
        string? filters = request.Filters;

        if (!string.IsNullOrEmpty(filters))
        {
            filters = FilterConvert(filters);
            source = source.Where<T>(filters);
        }

        if (!string.IsNullOrEmpty(sort))
        {
            sort = sort.Replace("-", " ");
            source = source.OrderBy<T>(sort);
        }

        var pagedData = await source.Skip((request.PageNumber - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToListAsync();
        
        return new PagedResponse<List<T>>(pagedData, request.PageNumber, request.PageSize)
        {
            TotalRecords = await source.CountAsync(),
            TotalPages = (int)Math.Ceiling((double)(await source.CountAsync()) / request.PageSize)
        };
    }

    public static string FilterConvert(string filter)
    {

        // Replace logic operators with C# syntax
        filter = filter.Replace("-and-", " && ").Replace("-or-", " || ");

        // Tách các biểu thức con ra khỏi dấu ngoặc để xử lý từng phần
        var pattern = @"([\w\d_.]+~[a-z]+~(?:int|decimal|datetime|Date)?\([^)]+\)|[\w\d_.]+~[a-z]+~null|[\w\d_.]+~[a-z]+~'[^|&]*'|[\w\d_.]+~[a-z]+~(\[.*\]|[^()\s]+))";
        var matches = System.Text.RegularExpressions.Regex.Matches(filter, pattern);

        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            var original = match.Value;
            var transformed = TransformFilter(original);
            filter = filter.Replace(original, transformed);
        }

        return filter;

    }

    public static string FilterConvertExpression(string filter)
    {
        bool isEndWithBracket = false;
        int countBracket = 0;
        //Convert DateTime
        if (filter.Contains("Date("))
        {
            string strDate = filter.Substring(filter.IndexOf("Date("), filter.IndexOf(")") - filter.IndexOf("Date(") + 1);
            DateTime filterDate = DateTime.FromOADate(double.Parse(strDate.Replace("Date(", "").Replace(")", "")));
            filter = filter.Replace(strDate, "\"" + filterDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"");
        }

        // if (filter.EndsWith(")"))
        // {
        //     isEndWithBracket = true;
        //     int countStartBracket = filter.Count(x => x == '('); 
        //     int countEndBracket = filter.Count(x => x == ')');
        //     countBracket = countEndBracket - countStartBracket;
        //     filter = filter.Substring(0, filter.Length - countBracket);
        // }

        filter = TransformFilter(filter);
        if (filter.Contains("~in~"))
        {
            var idxBracket = filter.LastIndexOf("(");
            var idxBracketVal = filter.IndexOf("[");
            var colName = filter.Substring(idxBracket + 1, filter.IndexOf("~in~") - idxBracket - 1);
            var value = filter.Substring(idxBracketVal + 1, filter.IndexOf("]") - idxBracketVal - 1);
            string newFilter = string.Empty;
            while (idxBracket + 1 > newFilter.Length)
                newFilter += "(";
            // newFilter += $"StringList(\"{value}\").Contains({colName})";
            newFilter += string.Join("||", value.Split(',').Select(s => $"{colName} == \"{s}\""));
            filter = $"({newFilter})";

        }

        if (isEndWithBracket)
        {
            while (countBracket > 0)
            {
                filter += ")";
                countBracket--;
            }
        }
        return filter;
    }

    static string TransformFilter(string filter)
    {
        bool hasParentheses = filter.StartsWith("(") && filter.EndsWith(")");
        if (hasParentheses)
            filter = filter.Substring(1, filter.Length - 2).Trim();

        string[] ops = new[] {
                "~eq~", "~neq~", "~>=~", "~<=~", "~>~", "~<~",
                "~gte~", "~lte~", "~gt~", "~lt~",
                "~contains~", "~startwith~", "~endwith~",
                "~in~"
                };

        string op = ops.FirstOrDefault(o => filter.Contains(o))!;
        if (op == null) return filter;

        var parts = filter.Split(op);
        if (parts.Length != 2) return filter;

        string field = parts[0].Trim();
        string value = parts[1].Trim();
        string parsedValue;
        bool isString = true;

        if (value == "null")
        {
            parsedValue = "null";
            isString = false;
        }
        // Detect typed values
        else if (value.StartsWith("int(") && value.EndsWith(")"))
        {
            parsedValue = value.Substring(4, value.Length - 5);
            isString = false;
        }
        else if (value.StartsWith("decimal(") && value.EndsWith(")"))
        {
            parsedValue = value.Substring(8, value.Length - 9);
            isString = false;
        }
        else if (value.StartsWith("datetime(") && value.EndsWith(")"))
        {
            parsedValue = $"DateTime.Parse(\"{value.Substring(9, value.Length - 10)}\")";
            isString = false;
        }
        else if (value.StartsWith("Date(") && value.EndsWith(")"))
        {
            string strDate = value.Substring(value.IndexOf("Date("), value.IndexOf(")") - value.IndexOf("Date(") + 1);
            DateTime filterDate = DateTime.FromOADate(double.Parse(strDate.Replace("Date(", "").Replace(")", "")));
            parsedValue = value.Replace(strDate, "\"" + filterDate.ToString("yyyy-MM-dd HH:mm:ss") + "\"");
        }
        else if (value.StartsWith("[") && value.EndsWith("]"))
        {
            var idxBracket = value.LastIndexOf("(");
            var idxBracketVal = value.IndexOf("[");
            parsedValue = value.Substring(idxBracketVal + 1, value.IndexOf("]") - idxBracketVal - 1);
            string newFilter = string.Empty;
            while (idxBracket + 1 > newFilter.Length)
                newFilter += "(";
            // newFilter += $"StringList(\"{value}\").Contains({colName})";
            newFilter += string.Join("||", parsedValue.Split(',').Select(s => {
                string value = s;
                if (value.StartsWith("'") && value.EndsWith("'"))
                    value = value.Substring(1, value.Length - 2).Replace("''", "'");
                return $"{field} == \"{value}\"";
            }));
            parsedValue = $"({newFilter})";
        }
        else
        {
            // Handle 'O''Connor' or '''' (=> single quote)
            if (value.StartsWith("'") && value.EndsWith("'"))
                value = value.Substring(1, value.Length - 2).Replace("''", "'");

            parsedValue = $"\"{value.Trim().Replace("\"", "\\\"")}\"";
        }

        string expr = op switch
        {
            "~eq~" => $"{field} == {parsedValue}",
            "~neq~" => $"{field} != {parsedValue}",
            "~>~" or "~gt~" => $"{field} > {parsedValue}",
            "~<~" or "~lt~" => $"{field} < {parsedValue}",
            "~>=~" or "~gte~" => $"{field} >= {parsedValue}",
            "~<=~" or "~lte~" => $"{field} <= {parsedValue}",
            "~contains~" => $"{field}.ToLower().Contains({parsedValue}.ToLower())",
            "~startwith~" => $"{field}.ToLower().StartsWith({parsedValue}.ToLower())",
            "~endwith~" => $"{field}.ToLower().EndsWith({parsedValue}.ToLower())",
            "~in~" => parsedValue,
            _ => filter
        };

        return hasParentheses ? $"({expr})" : expr;
    }

    public static string FilterGetByField(string filter, string field)
    {
        string result = string.Empty;
        if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(field) && filter.Contains(field))
        {
            List<string> val = new List<string>();
            //Get filter expression
            var expFields = filter.Split(new string[] { "-and-", "-or-" }, StringSplitOptions.None).Where(x => x.Contains(field)).ToList();
            foreach (var expField in expFields)
            {
                string filterVal = expField.Split("~")[2];
                while (filterVal.EndsWith(")"))
                {
                    if (filterVal.Count(x => x == '(') == filterVal.Count(x => x == ')'))
                        break;

                    filterVal = filterVal.Remove(filterVal.LastIndexOf(")"));
                }
                val.Add(filterVal.Replace("'", ""));
            }
            result = string.Join(",", val);
        }
        return result;
    }

    public static string FilterRemoveField(string filter, string field)
    {
        string result = filter;
        if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(field) && filter.Contains(field))
        {
            var expField = filter.Split("-and-").ToList();
            result = string.Join("-and-", expField.Where(x => !x.StartsWith(field)));
        }
        return result;
    }
}