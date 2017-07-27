using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Highlight;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ElasticSearchLite.NetCore.Models.Conditions;
using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Highlight
    {
        private readonly string _indexName;

        private Highlight(string indexName)
        {
            if (string.IsNullOrWhiteSpace(indexName)) { throw new ArgumentException(nameof(indexName)); }

            _indexName = indexName;
        }

        public static Highlight QueryIn(string indexName)
        {
            return new Highlight(indexName);
        }

        public IHighlightQueryUnconfigured<TPoco> Returns<TPoco>() where TPoco : IElasticPoco
        {
            return new HighlightQuery<TPoco>(_indexName);
        }

        public abstract class HighlightQuery : AbstractBaseQuery
        {
            protected internal ElasticHighlight Highlight { get; } = new ElasticHighlight();
            protected internal int Size { get; set; } = 25;
            protected internal int From { get; set; } = 0;
            protected internal int MinimumNumberShouldMatch { get; set; } = 0;
            protected internal int FragmentSize { get; set; } = 150;
            protected internal int NumberOfFragements { get; set; } = 3;
            protected internal Dictionary<ElasticBoolQueryOccurrences, List<IElasticCondition>> Conditions { get; } = new Dictionary<ElasticBoolQueryOccurrences, List<IElasticCondition>>
            {
                { ElasticBoolQueryOccurrences.Should, new List<IElasticCondition>() },
                { ElasticBoolQueryOccurrences.Must, new List<IElasticCondition>() },
                { ElasticBoolQueryOccurrences.Filter, new List<IElasticCondition>() },
                { ElasticBoolQueryOccurrences.MustNot, new List<IElasticCondition>() }
            };
            protected internal List<ElasticSort> SortingFields { get; } = new List<ElasticSort>();

            public HighlightQuery(string indexName) : base(indexName) { }
        }

        public sealed class HighlightQuery<TPoco> :
            HighlightQuery,
            IHighlightQueryUnconfigured<TPoco>,
            IHighlightQueryExecutable<TPoco>,
            IHighlightQueryShouldAdded<TPoco>,
            IHighlightQueryMustAdded<TPoco>,
            IHighlightQueryMustNotAdded<TPoco>,
            IHighlightQueryFilterAdded<TPoco>,
            IHighlightQueryPreAdded<TPoco>,
            IHighlightQueryPostAdded<TPoco>
            where TPoco : IElasticPoco
        {
            private string tempFieldName;
            private ElasticBoolQueryOccurrences tempOccurrence;

            internal HighlightQuery(string indexName) : base(indexName) { }

            public IHighlightQueryPreAdded<TPoco> WithPre(string preTag)
            {
                if (string.IsNullOrWhiteSpace(preTag)) { throw new ArgumentException(nameof(preTag)); }
                Highlight.PreTag = preTag;

                return this;
            }

            public IHighlightQueryPostAdded<TPoco> WithPost(string postTag)
            {

                if (string.IsNullOrWhiteSpace(postTag)) { throw new ArgumentException(nameof(postTag)); }
                Highlight.PostTag = postTag;

                return this;
            }

            public IHighlightQueryExecutable<TPoco> AddFields(params Expression<Func<TPoco, object>>[] propertyExpressions)
            {
                var fields = propertyExpressions.Select(p => new ElasticField { Name = GetCorrectPropertyName(p) });
                Highlight.HighlightedFields.AddRange(fields);

                return this;
            }

            public IHighlightQueryShouldAdded<TPoco> Should(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.Should;

                return this;
            }

            public IHighlightQueryMustAdded<TPoco> Must(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.Must;

                return this;
            }

            public IHighlightQueryMustNotAdded<TPoco> MustNot(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.MustNot;

                return this;
            }

            public IHighlightQueryFilterAdded<TPoco> Filter(Expression<Func<TPoco, object>> propertyExpression)
            {
                tempFieldName = GetCorrectPropertyName(propertyExpression);
                tempOccurrence = ElasticBoolQueryOccurrences.Filter;

                return this;
            }

            public IHighlightQueryExecutable<TPoco> Match(object value)
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

            public IHighlightQueryExecutable<TPoco> MatchPhrase(object value)
            {
                var condition = new ElasticMatchPhraseCondition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Value = value
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }

            public IHighlightQueryExecutable<TPoco> MatchPhrasePrefix(object value)
            {
                var condition = new ElasticMatchPhrasePrefixCondition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Value = value
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }

            public IHighlightQueryExecutable<TPoco> Range(ElasticRangeOperations op, object value)
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

            public IHighlightQueryExecutable<TPoco> Term(params object[] values)
            {
                var condition = new ElasticTermCodition
                {
                    Field = new ElasticField { Name = tempFieldName },
                    Values = values?.ToList()
                };

                Conditions[tempOccurrence].Add(condition);

                return this;
            }

            public IHighlightQueryExecutable<TPoco> Take(int take)
            {
                if (take < 0 || take > 10000)
                {
                    throw new ArgumentException(nameof(take));
                }

                Size = take;

                return this;
            }

            public IHighlightQueryExecutable<TPoco> Skip(int skip)
            {
                if (skip < 0 || skip > 10000)
                {
                    throw new ArgumentException(nameof(skip));
                }

                From = skip;

                return this;
            }

            public IHighlightQueryExecutable<TPoco> ShouldMatchAtLeast(int minimumNumber)
            {
                CheckParameterBiggerThanNull(minimumNumber);

                MinimumNumberShouldMatch = minimumNumber;

                return this;
            }

            public IHighlightQueryExecutable<TPoco> LimitFragmentSizeTo(int fragmentSize)
            {
                CheckParameterBiggerThanNull(fragmentSize);

                FragmentSize = fragmentSize;

                return this;
            }

            public IHighlightQueryExecutable<TPoco> LimitFragmentsTo(int numberOfFragments)
            {
                CheckParameterBiggerThanNull(numberOfFragments);

                NumberOfFragements = numberOfFragments;

                return this;
            }

            private void CheckParameterBiggerThanNull(int parameter)
            {
                if (parameter < 0)
                {
                    throw new ArgumentException($"{nameof(parameter)} should be bigger than 0");
                }
            }
        }
    }
}
