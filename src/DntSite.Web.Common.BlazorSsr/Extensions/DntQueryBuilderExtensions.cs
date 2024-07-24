using System.Text;
using System.Text.RegularExpressions;

namespace DntSite.Web.Common.BlazorSsr.Extensions;

/// <summary>
///     Queryable Extensions
/// </summary>
public static partial class DntQueryBuilderExtensions
{
    // https://alirezanet.github.io/Gridify/guide/filtering.html#conditional-operators
    private static readonly Dictionary<DntQueryBuilderOperation, string> PositiveOperationsMap = new()
    {
        {
            DntQueryBuilderOperation.StartsWith, "^"
        },
        {
            DntQueryBuilderOperation.EndsWith, "$"
        },
        {
            DntQueryBuilderOperation.Contains, "=*"
        },
        {
            DntQueryBuilderOperation.Equal, "="
        },
        {
            DntQueryBuilderOperation.LessThan, "<"
        },
        {
            DntQueryBuilderOperation.LessThanOrEqual, "<="
        },
        {
            DntQueryBuilderOperation.GreaterThan, ">"
        },
        {
            DntQueryBuilderOperation.GreaterThanOrEqual, ">="
        }
    };

    private static readonly Dictionary<DntQueryBuilderOperation, string> NegativeOperationsMap = new()
    {
        {
            DntQueryBuilderOperation.StartsWith, "!^"
        },
        {
            DntQueryBuilderOperation.EndsWith, "!$"
        },
        {
            DntQueryBuilderOperation.Contains, "!*"
        },
        {
            DntQueryBuilderOperation.Equal, "!="
        },
        {
            DntQueryBuilderOperation.LessThan, ">"
        },
        {
            DntQueryBuilderOperation.LessThanOrEqual, ">="
        },
        {
            DntQueryBuilderOperation.GreaterThan, "<"
        },
        {
            DntQueryBuilderOperation.GreaterThanOrEqual, "<="
        }
    };

    /// <summary>
    ///     Create a dynamic `Gridify Where` statement for the provided model
    /// </summary>
    public static string ToGridifyFilter<TRecord>(this IList<DntQueryBuilderSearchRule<TRecord>>? searchRules)
        where TRecord : class
    {
        if (searchRules is null || searchRules.Count == 0)
        {
            return string.Empty;
        }

        var gridifyFilter = new StringBuilder();

        var currentOperationLogic = DntQueryBuilderOperationLogic.And;

        foreach (var rule in searchRules)
        {
            var propertyPath = rule.QueryBuilderProperty?.PropertyPath;

            if (string.IsNullOrWhiteSpace(propertyPath) || rule.Operation is null)
            {
                continue;
            }

            var ruleValue = rule.Value.EscapeValue();

            switch (rule.OperationKind)
            {
                case DntQueryBuilderOperationKind.Is:
                    switch (currentOperationLogic)
                    {
                        case DntQueryBuilderOperationLogic.And:
                            gridifyFilter.Append(CultureInfo.InvariantCulture,
                                $", ({propertyPath} {PositiveOperationsMap[rule.Operation.Value]} {ruleValue}) ");

                            break;
                        case DntQueryBuilderOperationLogic.Or:
                            gridifyFilter.Append(CultureInfo.InvariantCulture,
                                $"| ({propertyPath} {PositiveOperationsMap[rule.Operation.Value]} {ruleValue}) ");

                            break;
                    }

                    break;

                case DntQueryBuilderOperationKind.Not:
                    switch (currentOperationLogic)
                    {
                        case DntQueryBuilderOperationLogic.And:
                            gridifyFilter.Append(CultureInfo.InvariantCulture,
                                $", ({propertyPath} {NegativeOperationsMap[rule.Operation.Value]} {ruleValue}) ");

                            break;
                        case DntQueryBuilderOperationLogic.Or:
                            gridifyFilter.Append(CultureInfo.InvariantCulture,
                                $"| ({propertyPath} {NegativeOperationsMap[rule.Operation.Value]} {ruleValue}) ");

                            break;
                    }

                    break;
            }

            currentOperationLogic = rule.NextOperationLogic;
        }

        return gridifyFilter.ToString().TrimStart(',', '|').Trim();
    }

    private static string EscapeValue(this string? value)
        => string.IsNullOrWhiteSpace(value) ? "" : EscapeRegex().Replace(value, "\\$1");

    [GeneratedRegex(@"([(),|\\]|\/i)", RegexOptions.Compiled, 3000)]
    private static partial Regex EscapeRegex();
}