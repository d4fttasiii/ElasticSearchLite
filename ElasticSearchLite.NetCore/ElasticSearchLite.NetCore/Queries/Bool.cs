using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Bool;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Models.Conditions;
using ElasticSearchLite.NetCore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Bool
    {
        private readonly string _indexName;

        private Bool(string indexName)
        {
            if (string.IsNullOrWhiteSpace(indexName)) { throw new ArgumentException(nameof(indexName)); }

            _indexName = indexName;
        }

        public static Bool QueryIn(string indexName)
        {
            return new Bool(indexName);
        }

        public IBoolQueryExecutable<TPoco> Returns<TPoco>() where TPoco : IElasticPoco
        {
            return new BoolQuery<TPoco>(_indexName);
        }

        public abstract class BoolQuery : AbstractBaseQuery
        {
            protected internal bool HighlightingEnabled { get; set; } = false;
            protected internal ElasticHighlight Highlight = new ElasticHighlight();
            protected internal int Size = 25;
            protected internal int From = 0;
            protected internal int MinimumNumberShouldMatch = 1;
            protected internal List<ElasticField> SourceFields = new List<ElasticField>();
            protected internal Dictionary<ElasticBoolQueryOccurrences, List<IElasticCondition>> Conditions { get; } = new Dictionary<ElasticBoolQueryOccurrences, List<IElasticCondition>>
            {
                { ElasticBoolQueryOccurrences.Should, new List<IElasticCondition>() },
                { ElasticBoolQueryOccurrences.Must, new List<IElasticCondition>() },
                { ElasticBoolQueryOccurrences.Filter, new List<IElasticCondition>() },
                { ElasticBoolQueryOccurrences.MustNot, new List<IElasticCondition>() }
            };
            protected internal List<ElasticSort> SortingFields { get; } = new List<ElasticSort>();
            protected BoolQuery(string indexName) : base(indexName)
            {
                SortingFields.Add(new ElasticSort
                {
                    Field = new ElasticField { Name = ElasticFields.Score.Name },
                    Order = ElasticSortOrders.Descending
                });
            }
        }

        public class BoolQuery<TPoco> :
            BoolQuery,
            IBoolQueryExecutable<TPoco>,
            IBoolQueryShouldAdded<TPoco>,
            IBoolQueryMustAdded<TPoco>,
            IBoolQueryMustNotAdded<TPoco>,
            IBoolQueryFilterAdded<TPoco>,
            IBoolQuerySortAdded<TPoco>,
            IBoolQuerySortOrderDefined<TPoco>,
            IBoolQueryHighlightingEnabled<TPoco>,
            IBoolQueryPreAdded<TPoco>,
            IBoolQueryPostAdded<TPoco>,
            IBoolQueryFragmentsLimited<TPoco>
            where TPoco : IElasticPoco
        {
            private string tempFieldName;
            private ElasticBoolQueryOccurrences tempOccurrence;

            internal BoolQuery(string indexName) : base(indexName) { }

            public IBoolQueryExecutable<TPoco> Sources(params Expression<Func<TPoco, object>>[] propertyExpressions)
            {
                foreach (var p in propertyExpressions)
                {
                    SourceFields.Add(new ElasticField
                    {
                        Name = GetCorrectPropertyName(p),
                        UseKeywordField = false
                    });
                }

                return this;
            }

            public IBoolQueryMustAdded<TPoco> Must(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.Must;

                return this;
            }
            public IBoolQueryFilterAdded<TPoco> Filter(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.Filter;

                return this;
            }

            public IBoolQueryMustNotAdded<TPoco> MustNot(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.MustNot;

                return this;
            }

            public IBoolQueryShouldAdded<TPoco> Should(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.Should;

                return this;
            }
            public IBoolQueryExecutable<TPoco> Match(object value)
            {
                var condition = new ElasticMatchCodition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Value = value,
                    Operation = ElasticOperators.And
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }

            public IBoolQueryExecutable<TPoco> MatchPhrase(object value)
            {
                var condition = new ElasticMatchPhraseCondition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Value = value
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }

            public IBoolQueryExecutable<TPoco> MatchPhrasePrefix(object value)
            {
                var condition = new ElasticMatchPhrasePrefixCondition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Value = value
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }

            public IBoolQueryExecutable<TPoco> Range(ElasticRangeOperations op, object value)
            {
                var condition = new ElasticRangeCondition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Operation = op,
                    Value = value
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }
            public IBoolQueryExecutable<TPoco> Term(params object[] values)
            {
                var condition = new ElasticTermCodition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Values = values?.ToList()
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }

            public IBoolQueryExecutable<TPoco> Take(int take)
            {
                if (take < 0 || take > 10000)
                {
                    throw new ArgumentException(nameof(take));
                }

                Size = take;

                return this;
            }

            public IBoolQueryExecutable<TPoco> Skip(int skip)
            {
                if (skip < 0 || skip > 10000)
                {
                    throw new ArgumentException(nameof(skip));
                }

                From = skip;

                return this;
            }

            public IBoolQuerySortAdded<TPoco> Sort(Expression<Func<TPoco, object>> propertyExpression)
            {
                SortingFields.Clear();
                SortingFields.Add(new ElasticSort
                {
                    Field = new ElasticField
                    {
                        Name = GetCorrectPropertyName(propertyExpression)
                    }
                });

                return this;
            }
            public IBoolQuerySortOrderDefined<TPoco> Ascending()
            {
                var sortField = SortingFields.FirstOrDefault();
                sortField.Order = ElasticSortOrders.Ascending;

                return this;
            }

            public IBoolQuerySortOrderDefined<TPoco> Descending()
            {
                var sortField = SortingFields.FirstOrDefault();
                sortField.Order = ElasticSortOrders.Descending;

                return this; ;
            }

            public IBoolQueryExecutable<TPoco> WithKeyword()
            {
                var sortField = SortingFields.FirstOrDefault();
                sortField.Field.UseKeywordField = true;

                return this;
            }

            public IBoolQueryExecutable<TPoco> WithoutKeyword()
            {
                var sortField = SortingFields.FirstOrDefault();
                sortField.Field.UseKeywordField = false;

                return this;
            }

            public IBoolQueryExecutable<TPoco> ShouldMatchAtLeast(int minimumNumber)
            {
                if (minimumNumber < 0)
                {
                    throw new ArgumentException(nameof(minimumNumber));
                }

                MinimumNumberShouldMatch = minimumNumber;

                return this;
            }

            public IBoolQueryHighlightingEnabled<TPoco> EnableHighlighting()
            {
                HighlightingEnabled = true;

                return this;
            }

            public IBoolQueryHighlightingEnabled<TPoco> AddField(Expression<Func<TPoco, object>> propertyExpression)
            {
                CheckParameter(propertyExpression);
                Highlight.HighlightedFields.Add(new ElasticField
                {
                    Name = GetCorrectPropertyName(propertyExpression)
                });

                return this;
            }

            public IBoolQueryHighlightingEnabled<TPoco> AddFields(IEnumerable<Expression<Func<TPoco, object>>> propertyExpressions)
            {
                CheckParameters(propertyExpressions);
                Highlight.HighlightedFields.AddRange(propertyExpressions
                    .Select(pe => new ElasticField
                    {
                        Name = GetCorrectPropertyName(pe)
                    }));

                return this;
            }

            public IBoolQueryPreAdded<TPoco> SetPreTagTo(string preTag)
            {
                Highlight.PreTag = preTag;

                return this;
            }

            public IBoolQueryPostAdded<TPoco> SetPostTagTo(string postTag)
            {
                Highlight.PostTag = postTag;

                return this;
            }

            public IBoolQueryFragmentsLimited<TPoco> LimitTheNumberOfFragmentsTo(int numberOfFragments)
            {
                CheckParameterBiggerThanNull(numberOfFragments);
                Highlight.NumberOfFragments = numberOfFragments;

                return this;
            }

            public IBoolQueryExecutable<TPoco> LimitFragmentSizeTo(int fragmentSize)
            {
                CheckParameterBiggerThanNull(fragmentSize);
                Highlight.FragmentSize = fragmentSize;

                return this;
            }
        }
    }
}
