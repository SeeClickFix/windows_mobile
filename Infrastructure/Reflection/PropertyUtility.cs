using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.Infrastructure.Reflection
{
    public static class PropertyUtility
    {
        public static Func<TProperty> CreateGetter<TProperty>(this PropertyInfo propertyInfo, object owner)
        {
            ArgumentValidator.AssertNotNull(propertyInfo, "propertyInfo");
            ArgumentValidator.AssertNotNull(owner, "owner");

            Type getterType = Expression.GetFuncType(new[] { propertyInfo.PropertyType });
            //Type getterType = typeof(Func<>).MakeGenericType(propertyInfo.PropertyType);

            object getter = Delegate.CreateDelegate(getterType, owner, propertyInfo.GetGetMethod());

            return (Func<TProperty>)getter;
        }

        public static TDelegate CreateDelegate<TDelegate>(this PropertyInfo propertyInfo, object owner)
        {
            ArgumentValidator.AssertNotNull(propertyInfo, "propertyInfo");
            ArgumentValidator.AssertNotNull(owner, "owner");

            object getter = Delegate.CreateDelegate(
                                typeof(TDelegate), owner, propertyInfo.GetGetMethod());

            return (TDelegate)getter;
        }

        public static Action<TProperty> CreateSetter<TProperty>(this PropertyInfo propertyInfo, object owner)
        {
            ArgumentValidator.AssertNotNull(propertyInfo, "propertyInfo");
            ArgumentValidator.AssertNotNull(owner, "owner");

            var propertyType = propertyInfo.PropertyType;
            var setterType = Expression.GetActionType(new[] { propertyType });

            Delegate setter = Delegate.CreateDelegate(
                        setterType, owner, propertyInfo.GetSetMethod());

            return (Action<TProperty>)setter;
        }

        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(
                    "MemberExpression expected.", "expression");
            }

            if (memberExpression.Member == null)
            {
                throw new ArgumentException("Member should not be null.");
            }

            if (memberExpression.Member.MemberType != MemberTypes.Property)
            {
                throw new ArgumentException("Property expected.", "expression");
            }

            PropertyInfo propertyInfo = (PropertyInfo)memberExpression.Member;
            return propertyInfo;
        }
    }
}
