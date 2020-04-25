using System;
using System.Linq.Expressions;
using Xunit;

namespace NetCore.Assumptions
{
    public class AboutExpressions
    {
        [Fact]
        public void CompilingTwiceGeneratesDifferentDelegates()
        {
            Expression<Func<int, bool>> fn = x => x > 10;

            var f1 = fn.Compile();
            var f2 = fn.Compile();
            Assert.False(ReferenceEquals(f1, f2));
        }

        [Fact]
        public void TwoExpressionsBuildTheSameWayAreNotEqual()
        {
            Expression<Func<int, bool>> divisibleBy3 = x => (x % 3) == 0;
            Expression<Func<int, bool>> divisibleBy5 = x => (x % 5) == 0;

            var expr1 = MakeAnd(divisibleBy3, divisibleBy5);
            var expr2 = MakeAnd(divisibleBy3, divisibleBy5);

            Assert.NotEqual(expr1, expr2);
        }

        [Fact]
        public void StrignRepresentationOfTwoExpressionsBuildTheSameWayAreEqual()
        {
            Expression<Func<int, bool>> divisibleBy3 = x => (x % 3) == 0;
            Expression<Func<int, bool>> divisibleBy5 = x => (x % 5) == 0;

            var expr1 = MakeAnd(divisibleBy3, divisibleBy5);
            var expr2 = MakeAnd(divisibleBy3, divisibleBy5);

            Assert.Equal(expr1.ToString(), expr2.ToString());
        }

        static Expression<Func<T, bool>> MakeAnd<T>(Expression<Func<T, bool>> x, Expression<Func<T, bool>> y)
        {
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.AndAlso(x.Body, y.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return finalExpr;
        }

        #region Visitors

        class ParameterReplacer : ExpressionVisitor
        {

            private readonly ParameterExpression _parameter;

            protected override Expression VisitParameter(ParameterExpression node)
                => base.VisitParameter(_parameter);

            internal ParameterReplacer(ParameterExpression parameter)
            {
                _parameter = parameter;
            }
        }

        #endregion
    }
}
