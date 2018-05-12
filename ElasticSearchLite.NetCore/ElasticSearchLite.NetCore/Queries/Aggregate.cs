using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Aggregate;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Models.Conditions;
using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class Aggregate : AbstractConditionalQuery
    {
        internal ElasticField AggregateField { get; set; }
        internal ElasticMetricsAggregations ElasticMetricsAggregations { get; set; }

        protected Aggregate(IElasticPoco poco) : base(poco) { }

        protected Aggregate(string indexName) : base(indexName) { }

        /// <summary>
        /// Creates a new aggregate query
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <returns></returns>
        public static Interfaces.Aggregate.IFilterableAggregatedQuery<TPoco> In<TPoco>()
            where TPoco : IElasticPoco
                => new Aggregate<TPoco>($"{typeof(TPoco).Name.ToLower()}index");
    }

    public sealed class Aggregate<TPoco> :
        Aggregate,
        Interfaces.Aggregate.IFilterableAggregatedQuery<TPoco>,
        Interfaces.Aggregate.IFilteredAggregatedQuery<TPoco>,
        Interfaces.Aggregate.IExecutableAggregatedQuery<TPoco>
        where TPoco : IElasticPoco
    {
        internal Aggregate(string indexName) : base(indexName)
        {
        }
        /// <summary>
        /// he most simple query, which matches all documents, giving them all a _score of 1.0.
        /// </summary>
        /// <returns></returns>
        public IFilteredAggregatedQuery<TPoco> MatchAll()
        {
            return this;
        }
        /// <summary>
        /// Term Query finds documents that contain the exact term specified in the inverted index. Doesn't affect the score.
        /// </summary>
        /// <param name="propertyExpression">Field name</param>
        /// <param name="value">Value which should equal the field content</param>
        /// <returns></returns>
        public Interfaces.Aggregate.IFilteredAggregatedQuery<TPoco> Term(Expression<Func<TPoco, object>> propertyExpression, object value)
        {
            TermCondition = new ElasticTermCodition()
            {
                Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                Values = new List<object> { value ?? throw new ArgumentNullException(nameof(value)) }
            };

            return this;
        }
        /// <summary>
        /// Match Query for full text search.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        public Interfaces.Aggregate.IFilteredAggregatedQuery<TPoco> Match(Expression<Func<TPoco, object>> propertyExpression, object value)
        {
            MatchCondition = new ElasticMatchCodition
            {
                Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                Value = value ?? throw new ArgumentNullException(nameof(value)),
                Fuzziness = 0
            };

            return this;
        }
        /// <summary>
        /// match_phrase Query for search whole phrases.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        public Interfaces.Aggregate.IFilteredAggregatedQuery<TPoco> MatchPhrase(Expression<Func<TPoco, object>> propertyExpression, string value)
        {
            MatchPhraseCondition = new ElasticMatchPhraseCondition
            {
                Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                Value = value ?? throw new ArgumentNullException(nameof(value)),
                Slop = 0
            };

            return this;
        }
        /// <summary>
        /// match_phrase_prefix Query for auto_complete like functionality.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        public Interfaces.Aggregate.IFilteredAggregatedQuery<TPoco> MatchPhrasePrefix(Expression<Func<TPoco, object>> propertyExpression, string value)
        {
            MatchPhrasePrefixCondition = new ElasticMatchPhrasePrefixCondition
            {
                Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                Value = value ?? throw new ArgumentNullException(nameof(value))
            };

            return this;
        }
        /// <summary>
        /// Returns document for a numeric range or timeinterval. It doesn't affect the score (filter context).
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="op">Range operator</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Interfaces.Aggregate.IFilteredAggregatedQuery<TPoco> Range(Expression<Func<TPoco, object>> propertyExpression, ElasticRangeOperations rangeOperation, object value)
        {
            var condition = new ElasticRangeCondition
            {
                Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                Operation = rangeOperation ?? throw new ArgumentNullException(nameof(rangeOperation)),
                Value = value ?? throw new ArgumentNullException(nameof(value))
            };
            RangeConditions.Add(condition);

            return this;
        }
        /// <summary>
        /// A single-value metrics aggregation that computes the average of numeric values that are extracted from the aggregated documents. 
        /// These values can be extracted either from specific numeric fields in the documents, or be generated by a provided script.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public Interfaces.Aggregate.IExecutableAggregatedQuery<TPoco> Average(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregateField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregations = ElasticMetricsAggregations.Avg;

            return this;
        }
        /// <summary>
        /// A single-value metrics aggregation that calculates an approximate count of distinct values. 
        /// Values can be extracted either from specific fields in the document or generated by a script.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public Interfaces.Aggregate.IExecutableAggregatedQuery<TPoco> Cardinality(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregateField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregations = ElasticMetricsAggregations.Cardinality;

            return this;
        }
        /// <summary>
        /// A single-value metrics aggregation that keeps track and returns the maximum value among the numeric values extracted from the aggregated documents. 
        /// These values can be extracted either from specific numeric fields in the documents, or be generated by a provided script.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public Interfaces.Aggregate.IExecutableAggregatedQuery<TPoco> Max(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregateField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregations = ElasticMetricsAggregations.Max;

            return this;
        }
        /// <summary>
        /// A single-value metrics aggregation that keeps track and returns the minimum value among numeric values extracted from the aggregated documents. 
        /// These values can be extracted either from specific numeric fields in the documents, or be generated by a provided script.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public Interfaces.Aggregate.IExecutableAggregatedQuery<TPoco> Min(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregateField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregations = ElasticMetricsAggregations.Min;

            return this;
        }
        /// <summary>
        /// A single-value metrics aggregation that sums up numeric values that are extracted from the aggregated documents. 
        /// These values can be extracted either from specific numeric fields in the documents, or be generated by a provided script.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public Interfaces.Aggregate.IExecutableAggregatedQuery<TPoco> Sum(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregateField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregations = ElasticMetricsAggregations.Sum;

            return this;
        }
        /// <summary>
        /// A single-value metrics aggregation that counts the number of values that are extracted from the aggregated documents. 
        /// These values can be extracted either from specific fields in the documents, or be generated by a provided script. 
        /// Typically, this aggregator will be used in conjunction with other single-value aggregations. 
        /// For example, when computing the avg one might be interested in the number of values the average is computed over.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public Interfaces.Aggregate.IExecutableAggregatedQuery<TPoco> ValueCount(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregateField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregations = ElasticMetricsAggregations.ValueCount;

            return this;
        }
    }
}
