using ElasticSearchLite.NetCore.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class AbstractBaseQuery : IQuery
    {
        internal string IndexName { get; }
        internal string TypeName { get; }
        internal IElasticPoco Poco { get; set; }

        protected AbstractBaseQuery(IElasticPoco poco)
        {
            Poco = poco ?? throw new ArgumentNullException(nameof(poco));
            IndexName = poco.Index ?? throw new ArgumentNullException(nameof(poco.Index));
            TypeName = poco.Type ?? throw new ArgumentNullException(nameof(poco.Type));
        }

        protected AbstractBaseQuery(string indexName)
        {
            if (string.IsNullOrEmpty(indexName)) { throw new ArgumentNullException(nameof(indexName)); }

            IndexName = indexName;
        }

        protected AbstractBaseQuery(string indexName, string typeName)
        {
            if (string.IsNullOrEmpty(indexName)) { throw new ArgumentNullException(nameof(indexName)); }
            if (string.IsNullOrEmpty(typeName)) { throw new ArgumentNullException(nameof(typeName)); }

            IndexName = indexName;
            TypeName = typeName;
        }

        protected PT CheckParameter<PT>(PT parameter)
        {
            if (parameter == null) { throw new ArgumentNullException(nameof(parameter)); }
            return parameter;
        }

        protected void CheckParameters<PT>(PT[] parameters)
        {
            if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }
            if (!parameters.Any()) { throw new ArgumentException(nameof(parameters)); }
        }

        protected string GetCorrectPropertyName<T>(Expression<Func<T, Object>> expression) => GetPropertyPath(expression);

        private static MemberExpression GetMemberExpression(Expression expression)
        {
            if (expression is MemberExpression)
            {
                return (MemberExpression)expression;
            }
            else if (expression is LambdaExpression)
            {
                var lambdaExpression = expression as LambdaExpression;
                if (lambdaExpression.Body is MemberExpression)
                {
                    return (MemberExpression)lambdaExpression.Body;
                }
                else if (lambdaExpression.Body is UnaryExpression)
                {
                    return ((MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
                }
            }
            return null;
        }

        private static string GetPropertyPath(Expression expr)
        {
            var path = new System.Text.StringBuilder();
            MemberExpression memberExpression = GetMemberExpression(expr);
            do
            {
                if (path.Length > 0)
                {
                    path.Insert(0, ".");
                }
                path.Insert(0, memberExpression.Member.Name);
                memberExpression = GetMemberExpression(memberExpression.Expression);
            }
            while (memberExpression != null);
            return path.ToString();
        }
    }
}
