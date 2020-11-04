using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ucl.PontoNet.Domain.Base.Interfaces;

namespace Ucl.PontoNet.Infra.Data.Base
{
    public static class Extensions
    {
        public static IQueryable<TType> ApplyFilter<TType>(
            this IQueryable<TType> source,
            string filter,
            string[] includeFilterFields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }


            if (string.IsNullOrWhiteSpace(filter))
            {
                return source;
            }

            var filterAfterSplit = filter.Split(',');
            foreach (var filterClause in filterAfterSplit.Reverse())
            {
                if (!string.IsNullOrWhiteSpace(filterClause))
                {
                    var trimmedFilterClause = filterClause.Trim();
                    var keyValue = trimmedFilterClause.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                    if (keyValue.Length != 3)
                    {
                        throw new ValidationException($"Invalid filter expression '{keyValue[0]}'");
                    }

                    if (includeFilterFields.All(o => o.ToLower() != keyValue[0].ToLower()))
                    {
                        throw new ValidationException($"Key mapping for {keyValue[0]} is missing");
                    }


                    if (string.Equals(keyValue[1], "like", StringComparison.CurrentCultureIgnoreCase))
                    {
                        source = source.Where($"it.{keyValue[0]}.Contains(@0)", keyValue[2]);
                    }
                    else
                    {
                        if (int.TryParse(keyValue[2], out var intValue))
                        {
                            source = source.Where($"{keyValue[0]} {keyValue[1]} @0", intValue);
                        }
                        else if ((bool.TryParse(keyValue[2], out var boolValue)))
                        {
                            source = source.Where($"{keyValue[0]} {keyValue[1]} @0", boolValue);
                        }
                        else
                        {
                            source = source.Where($"{keyValue[0]} {keyValue[1]} @0", keyValue[2]);
                        }
                    }

                }
            }

            return source;
        }



        public static IQueryable<TType> ApplyPagination<TType>(
            this IQueryable<TType> source, IFilterParameters parameters, out int total)
        {
            total = source.Count();

            if (total > parameters.per_page)
            {
                var skip = parameters.per_page * (parameters.page - 1);
                return source.Skip(skip).Take(parameters.per_page);
            }

            return source;
        }
    }
}
