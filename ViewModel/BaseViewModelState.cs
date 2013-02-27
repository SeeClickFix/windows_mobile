using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeClickFix.WP8.Infrastructure.Serialization;
using System.Linq.Expressions;
using System.Reflection;
using SeeClickFix.WP8.Infrastructure.Reflection;
using System.Diagnostics;
using SeeClickFix.WP8.Infrastructure;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace SeeClickFix.WP8.ViewModel
{
    public abstract class BaseViewModelState : BaseViewModel, IStatePreservation
    {
        protected bool IsLoaded { get; set; }
        
        protected BaseViewModelState()
		{
			this.ReadStateAttributes();
		}

        #region State Management

        readonly ViewState viewState = new ViewState();

        protected void LoadState()
        {
            this.LoadState(IsolatedStorageSettings.ApplicationSettings, PhoneApplicationService.Current.State, StateManager.ShouldLoadTransientState);
        }

        public virtual void LoadState(IDictionary<string, object> persistentStateDictionary, IDictionary<string, object> transientStateDictionary, bool shouldLoadTransientState)
        {
            if (this.IsLoaded)
            {
                return;
            }

            viewState.LoadPersistentState(persistentStateDictionary);
            if (shouldLoadTransientState)
            {
                viewState.LoadTransientState(transientStateDictionary);
            }

            this.IsLoaded = true;
        }

        public virtual void SaveState(IDictionary<string, object> persistentStateDictionary, IDictionary<string, object> transientStateDictionary)
        {
            viewState.SavePersistentState(persistentStateDictionary);
            viewState.SaveTransientState(transientStateDictionary);
        }

        public virtual void ClearState(IDictionary<string, object> persistentStateDictionary, IDictionary<string, object> transientStateDictionary)
        {
            viewState.DeregisterAndRemoveAllPersistentStates(persistentStateDictionary);
            viewState.DeregisterAndRemoveAllTransientStates(transientStateDictionary);
        }

        //protected byte[] Serialize(object value)
        //{
        //    byte[] state = SilverlightSerializer.Serialize(value);
        //    return state;
        //}

        //protected T Deserialize<T>(byte[] data) where T : class
        //{
        //    T result = SilverlightSerializer.Deserialize<T>(data);
        //    return result;
        //}

        protected void RegisterStatefulProperty<TProperty>(ApplicationStateType applicationStateType, Expression<Func<TProperty>> expression, Action<TProperty> setAction = null)
        {
            RegisterStatefulProperty((name, getter, setter)
                => viewState.RegisterState(name, getter, setter, applicationStateType), expression, setAction);
        }

        protected void DeregisterStatefulProperty(ApplicationStateType? applicationStateType, Expression<Func<object>> expression)
        {
            PropertyInfo propertyInfo = PropertyUtility.GetPropertyInfo(expression);
            string name = propertyInfo.Name;
            viewState.DeregisterState(name, applicationStateType);
        }

        protected void RegisterState<T>(
            string stateKey,
            Func<T> getFunc,
            Action<T> setAction,
            ApplicationStateType applicationStateType)
        {
            viewState.RegisterState(stateKey, getFunc, setAction, applicationStateType);
        }

        protected void DeregisterState<T>(string stateKey, ApplicationStateType? applicationStateType = null)
        {
            viewState.DeregisterState(stateKey, applicationStateType);
        }

        //		protected void RegisterTransientProperty<TProperty>(
        //			Expression<Func<TProperty>> expression, Action<TProperty> setAction = null)
        //		{
        //			RegisterStatefulProperty((name, getter, setter) 
        //				=> viewState.RegisterState(
        //							name, getter, setter, ApplicationStateType.Transient), 
        //						expression, setAction);
        //		}
        //
        //		protected void DeregisterTransientProperty(Expression<Func<object>> expression)
        //		{
        //			PropertyInfo propertyInfo
        //				= PropertyUtility.GetPropertyInfo(expression);
        //			string name = propertyInfo.Name;
        //			viewState.DeregisterTransientState(name);
        //		}
        //
        //		protected void RegisterTransientState<T>(
        //			string stateKey, Func<T> getFunc, Action<T> setAction)
        //		{
        //			viewState.RegisterState(
        //				stateKey, getFunc, setAction, ApplicationStateType.Transient);
        //		}

        void RegisterStatefulProperty<T>(Action<string, Func<T>, Action<T>> registerAction, Expression<Func<T>> expression, Action<T> setAction = null)
        {
            ArgumentValidator.AssertNotNull(registerAction, "registerAction");
            ArgumentValidator.AssertNotNull(expression, "expression");

            PropertyInfo propertyInfo = PropertyUtility.GetPropertyInfo(expression);
            string name = propertyInfo.Name;
            var propertyGetterFunc = propertyInfo.CreateGetter<T>(this);

            if (setAction == null)
            {
                try
                {
                    setAction = propertyInfo.CreateSetter<T>(this);
                }
                catch (Exception ex)
                {
                    string message = string.Format(
                        "Unable to get setter for property '{0}' {1} ", name, ex);
                    Console.WriteLine(message);
                    Debug.Assert(false, message);
                    return;
                }
            }
            registerAction(name, propertyGetterFunc, setAction);
        }

        //		protected void DeregisterTransientState<T>(string stateKey)
        //		{
        //			viewState.DeregisterTransientState(stateKey);
        //		}
        //
        //		protected void RegisterPersistentProperty<T>(
        //			Expression<Func<T>> expression, Action<T> setAction = null)
        //		{
        //			RegisterStatefulProperty((name, getter, setter)
        //				=> viewState.RegisterState(
        //							name, getter, setter, ApplicationStateType.Persistent),
        //						expression, setAction);
        //		}
        //
        //		protected void DeregisterPersistentProperty(Expression<Func<object>> expression)
        //		{
        //			PropertyInfo propertyInfo = PropertyUtility.GetPropertyInfo(expression);
        //			string name = propertyInfo.Name;
        //			viewState.DeregisterPersistentState(name);
        //		}
        //
        //		protected void RegisterPersistentState<T>(
        //			string stateKey, Func<T> getFunc, Action<T> setAction)
        //		{
        //			viewState.RegisterState(
        //				stateKey, getFunc, setAction, ApplicationStateType.Persistent);
        //		}


        void ReadStateAttributes()
        {
            var properties = GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                object[] attributes = propertyInfo.GetCustomAttributes(typeof(StatefulAttribute), true);

                if (attributes.Length <= 0)
                {
                    continue;
                }

                StatefulAttribute attribute = (StatefulAttribute)attributes[0];
                var persistenceType = attribute.StateType;

                if (!propertyInfo.CanRead || !propertyInfo.CanWrite)
                {
                    throw new InvalidOperationException(string.Format("Property {0} must have a getter and a setter.", propertyInfo.Name));
                }

                /* Prevents access to internal closure warning. */
                PropertyInfo info = propertyInfo;

                viewState.RegisterState(
                    string.Format("{0}.{1}", GetType().FullName, propertyInfo.Name),
                    () => info.GetValue(this, null),
                    obj => info.SetValue(this, obj, null),
                    persistenceType,
                    propertyInfo.PropertyType);
            }
        }

        #endregion
    }
}
