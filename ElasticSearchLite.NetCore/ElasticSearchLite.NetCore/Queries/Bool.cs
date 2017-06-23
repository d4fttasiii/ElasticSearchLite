using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Bool;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Queries
{
    class Bool
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

        // public Returns<TPoco>

        public abstract class BoolQuery : AbstractBaseQuery
        {
            protected internal int Size { get; set; } = 25;
            protected internal int From { get; set; } = 0;
            protected internal List<IElasticMatch> ShouldConditions { get; } = new List<IElasticMatch>();
            protected internal List<IElasticMatch> MustConditions { get; } = new List<IElasticMatch>();
            protected internal List<IElasticMatch> MustNotConditions { get; } = new List<IElasticMatch>();
            protected internal List<IElasticMatch> FilterConditions { get; } = new List<IElasticMatch>();
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
            private IElasticMatch tempCondition;

            internal BoolQuery(string indexName) : base(indexName) { }

            public IMustAddQuery<TPoco> Must(Expression<Func<TPoco, object>> propertyExpression)
            {
                return this;
            }
            public IFilterAddQuery<TPoco> Filter(Expression<Func<TPoco, object>> propertyExpression)
            {
                return this;
            }

            public IMustNotAddQuery<TPoco> MustNot(Expression<Func<TPoco, object>> propertyExpression)
            {
                return this;
            }

            public IShouldAddQuery<TPoco> Should(Expression<Func<TPoco, object>> propertyExpression)
            {
                return this;
            }
            public IExecutableBoolQuery<TPoco> Match(object value)
            {
                return this;
            }

            public IExecutableBoolQuery<TPoco> MatchPhrase(object value)
            {
                return this;
            }

            public IExecutableBoolQuery<TPoco> MatchPhrasePrefix(object value)
            {
                return this;
            }
        }
    }
}
