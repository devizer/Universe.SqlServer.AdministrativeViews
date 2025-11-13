using System.Linq.Expressions;

namespace Universe.SqlServer.SystemViews.External
{
    public static class ExpressionExtensions
    {
        public static string GetName(LambdaExpression exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }
    }
}
