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
        internal ElasticField AggregatedField { get; set; }
        internal ElasticField DateHistogramField { get; set; }
        internal ElasticMetricsAggregations ElasticMetricsAggregation { get; set; }
        internal ElasticPipelineAggregations ElasticPipelineAggregation { get; set; }
        internal int Window { get; set; }
        internal float Alpha { get; set; }
        internal string Interval { get; set; }

        protected Aggregate(IElasticPoco poco) : base(poco) { }

        protected Aggregate(string indexName) : base(indexName) { }

        /// <summary>
        /// Creates a new aggregate query
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <returns></returns>
        public static IFilterableAggregatedQuery<TPoco> In<TPoco>(string index = null)
            where TPoco : IElasticPoco
                => new Aggregate<TPoco>(index ?? $"{typeof(TPoco).Name.ToLower()}index");
    }

    public sealed class Aggregate<TPoco> :
        Aggregate,
        IFilterableAggregatedQuery<TPoco>,
        IFilteredAggregatedQuery<TPoco>,
        IMAAggregatedQuery<TPoco>,
        IMADateHistogramAddedAggregatedQuery<TPoco>,
        IMAIntervalSetAggregatedQuery<TPoco>,
        IExecutableAggregatedQuery<TPoco>
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
        public IFilteredAggregatedQuery<TPoco> Term(Expression<Func<TPoco, object>> propertyExpression, object value)
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
        public IFilteredAggregatedQuery<TPoco> Match(Expression<Func<TPoco, object>> propertyExpression, object value)
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
        public IFilteredAggregatedQuery<TPoco> MatchPhrase(Expression<Func<TPoco, object>> propertyExpression, string value)
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
        public IFilteredAggregatedQuery<TPoco> MatchPhrasePrefix(Expression<Func<TPoco, object>> propertyExpression, string value)
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
        public IFilteredAggregatedQuery<TPoco> Range(Expression<Func<TPoco, object>> propertyExpression, ElasticRangeOperations rangeOperation, object value)
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
        public IExecutableAggregatedQuery<TPoco> Average(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregatedField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregation = ElasticMetricsAggregations.Avg;

            return this;
        }
        /// <summary>
        /// A single-value metrics aggregation that calculates an approximate count of distinct values. 
        /// Values can be extracted either from specific fields in the document or generated by a script.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public IExecutableAggregatedQuery<TPoco> Cardinality(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregatedField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregation = ElasticMetricsAggregations.Cardinality;

            return this;
        }
        /// <summary>
        /// A single-value metrics aggregation that keeps track and returns the maximum value among the numeric values extracted from the aggregated documents. 
        /// These values can be extracted either from specific numeric fields in the documents, or be generated by a provided script.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public IExecutableAggregatedQuery<TPoco> Max(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregatedField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregation = ElasticMetricsAggregations.Max;

            return this;
        }
        /// <summary>
        /// A single-value metrics aggregation that keeps track and returns the minimum value among numeric values extracted from the aggregated documents. 
        /// These values can be extracted either from specific numeric fields in the documents, or be generated by a provided script.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public IExecutableAggregatedQuery<TPoco> Min(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregatedField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregation = ElasticMetricsAggregations.Min;

            return this;
        }
        /// <summary>
        /// A single-value metrics aggregation that sums up numeric values that are extracted from the aggregated documents. 
        /// These values can be extracted either from specific numeric fields in the documents, or be generated by a provided script.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public IExecutableAggregatedQuery<TPoco> Sum(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregatedField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregation = ElasticMetricsAggregations.Sum;

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
        public IExecutableAggregatedQuery<TPoco> ValueCount(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregatedField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticMetricsAggregation = ElasticMetricsAggregations.ValueCount;

            return this;
        }
        /// <summary>
        /// Given an ordered series of data, the Moving Average aggregation will slide a window across the data and emit the average value of that window.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public IMAAggregatedQuery<TPoco> SimpleMovingAverage(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregatedField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticPipelineAggregation = ElasticPipelineAggregations.SimpleMovingAverage;

            return this;
        }
        /// <summary>
        /// The linear model assigns a linear weighting to points in the series, such that "older" datapoints (e.g. those at the beginning of the window) contribute a linearly less amount to the total average.
        /// The linear weighting helps reduce the "lag" behind the data’s mean, since older points have less influence.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public IMAAggregatedQuery<TPoco> LinearMovingAverage(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregatedField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticPipelineAggregation = ElasticPipelineAggregations.LinearMovingAverage;

            return this;
        }
        /// <summary>
        /// The ewma model (aka "single-exponential") is similar to the linear model, except older data-points become exponentially less important, rather than linearly less important. 
        /// The speed at which the importance decays can be controlled with an alpha setting. 
        /// Small values make the weight decay slowly, which provides greater smoothing and takes into account a larger portion of the window. 
        /// Larger valuers make the weight decay quickly, which reduces the impact of older values on the moving average. 
        /// This tends to make the moving average track the data more closely but with less smoothing.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public IMAAggregatedQuery<TPoco> EWMA(Expression<Func<TPoco, object>> propertyExpression)
        {
            AggregatedField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };
            ElasticPipelineAggregation = ElasticPipelineAggregations.ExponentiallyWeightedMovingAverage;

            return this;
        }
        /// <summary>
        /// The size of window to "slide" across the histogram.
        /// </summary>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public IExecutableAggregatedQuery<TPoco> SetWindow(int windowSize)
        {
            Window = windowSize;

            return this;
        }
        /// <summary>
        /// Setting the date histogram for the moving average
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public IMADateHistogramAddedAggregatedQuery<TPoco> SetDateHistogramField(Expression<Func<TPoco, object>> propertyExpression)
        {
            DateHistogramField = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) };

            return this;
        }
        /// <summary>
        /// The default value of alpha is 0.3, and the setting accepts any float from 0-1 inclusive.
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public IExecutableAggregatedQuery<TPoco> SetAlpha(float alpha)
        {
            if (alpha > 1 || alpha < 0)
            {
                throw new ArgumentException(nameof(alpha));
            }
            Alpha = alpha;

            return this;
        }
        /// <summary>
        /// The size of window to "slide" across the histogram.
        /// </summary>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public IMAIntervalSetAggregatedQuery<TPoco> SetDateHistogramInterval(string interval)
        {
            if (string.IsNullOrWhiteSpace(interval))
            {
                throw new ArgumentException(nameof(interval));
            }
            Interval = interval;

            return this;
        }
    }
}
