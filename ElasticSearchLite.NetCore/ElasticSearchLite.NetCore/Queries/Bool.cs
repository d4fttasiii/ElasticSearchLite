using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Bool;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;
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
            protected internal int Size { get; set; } = 25;
            protected internal int From { get; set; } = 0;
            protected internal Dictionary<ElasticBoolQueryOccurrences, List<IElasticCondition>> Conditions { get; } = new Dictionary<ElasticBoolQueryOccurrences, List<IElasticCondition>>
            {
                { ElasticBoolQueryOccurrences.Should, new List<IElasticCondition>() },
                { ElasticBoolQueryOccurrences.Must, new List<IElasticCondition>() },
                { ElasticBoolQueryOccurrences.Filter, new List<IElasticCondition>() },
                { ElasticBoolQueryOccurrences.MustNot, new List<IElasticCondition>() }
            };
            protected internal List<ElasticSort> SortingFields { get; } = new List<ElasticSort>();
            protected BoolQuery(string indexName) : base(indexName) { }
        }

        public sealed class BoolQuery<TPoco> :
            BoolQuery,
            IBoolQueryExecutable<TPoco>,
            IBoolQueryShouldAdded<TPoco>,
            IBoolQueryMustAdded<TPoco>,
            IBoolQueryMustNotAdded<TPoco>,
            IBoolQueryFilterAdded<TPoco>
            where TPoco : IElasticPoco
        {
            private string tempFieldName;
            private ElasticBoolQueryOccurrences tempOccurrence;

            internal BoolQuery(string indexName) : base(indexName) { }

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

            public IBoolQueryExecutable<TPoco> Take(int take)
            {
                if(take < 0 || take > 10000)
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
        }
    }
}
