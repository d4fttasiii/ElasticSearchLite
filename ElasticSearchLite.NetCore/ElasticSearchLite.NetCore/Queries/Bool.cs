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
            if (string.IsNullOrWhiteSpace(indexName)) { throw new ArgumentNullException(nameof(indexName)); }

            _indexName = indexName;
        }

        public static Bool In(string indexName)
        {
            return new Bool(indexName);
        }

        public IExecutableBoolQuery<TPoco> Returns<TPoco>() where TPoco : IElasticPoco
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
            IExecutableBoolQuery<TPoco>,
            IShouldAddQuery<TPoco>,
            IMustAddQuery<TPoco>,
            IMustNotAddQuery<TPoco>,
            IFilterAddQuery<TPoco>
            where TPoco : IElasticPoco
        {
            private string tempFieldName;
            private ElasticBoolQueryOccurrences tempOccurrence;

            internal BoolQuery(string indexName) : base(indexName) { }

            public IMustAddQuery<TPoco> Must(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.Must;

                return this;
            }
            public IFilterAddQuery<TPoco> Filter(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.Filter;

                return this;
            }

            public IMustNotAddQuery<TPoco> MustNot(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.MustNot;

                return this;
            }

            public IShouldAddQuery<TPoco> Should(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.Should;

                return this;
            }
            public IExecutableBoolQuery<TPoco> Match(object value)
            {
                var condition = new ElasticMatchCodition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Value = value
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }

            public IExecutableBoolQuery<TPoco> MatchPhrase(object value)
            {
                var condition = new ElasticMatchPhraseCondition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Value = value
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }

            public IExecutableBoolQuery<TPoco> MatchPhrasePrefix(object value)
            {
                var condition = new ElasticMatchPhrasePrefixCondition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Value = value
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }

            public IExecutableBoolQuery<TPoco> Range(ElasticRangeOperations op, object value)
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
        }
    }
}
