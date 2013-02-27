#region File and License Information
/*
<File>
	<Copyright>Copyright © 2009, Daniel Vaughan. All rights reserved.</Copyright>
	<License>
	This file is part of Calcium.

	Calcium is free software: you can redistribute it and/or modify
	it under the terms of the GNU Lesser General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Calcium is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Lesser General Public License for more details.

	You should have received a copy of the GNU Lesser General Public License
	along with Calcium.  If not, see http://www.gnu.org/licenses/.
	</License>
	<Owner Name="Daniel Vaughan" Email="dbvaughan@gmail.com"/>
	<CreationDate>2011-02-11 10:35:26Z</CreationDate>
</File>
*/
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;

using SeeClickFix.WP8.Infrastructure.Serialization;

namespace SeeClickFix.WP8.Infrastructure
{
    public enum ApplicationStateType
    {
        Persistent,
        Transient
    }

    public class ViewState
    {
        readonly object transientStateLock = new object();
        readonly Dictionary<string, IStateAccessor> transientState = new Dictionary<string, IStateAccessor>();
        readonly object persistentStateLock = new object();
        readonly Dictionary<string, IStateAccessor> persistentState = new Dictionary<string, IStateAccessor>();

        public void RegisterState<T>(
            string stateKey,
            Func<T> getterFunc,
            Action<T> setterAction,
            ApplicationStateType stateType, 
            Type t = null)
        {
            ArgumentValidator.AssertNotNull(stateKey, "propertyName");
            ArgumentValidator.AssertNotNull(getterFunc, "propertyGetterFunc");
            ArgumentValidator.AssertNotNull(setterAction, "propertySetterAction");

            if (stateType == ApplicationStateType.Persistent)
            {
                lock (persistentStateLock)
                {
                    persistentState[stateKey] = new Accessor<T>(getterFunc, setterAction, t);
                }
            }
            else
            {
                lock (transientStateLock)
                {
                    transientState[stateKey] = new Accessor<T>(getterFunc, setterAction, t);
                }
            }
        }

        public void DeregisterState(string stateKey, ApplicationStateType? applicationStateType)
        {
            ArgumentValidator.AssertNotNull(stateKey, "stateKey");

            if (applicationStateType.HasValue)
            {
                if (applicationStateType == ApplicationStateType.Persistent)
                {
                    DeregisterPersistentState(stateKey);
                }
                else
                {
                    DeregisterTransientState(stateKey);
                }
            }
            else
            {
                DeregisterPersistentState(stateKey);
                DeregisterTransientState(stateKey);
            }
        }

        public void SaveTransientState(IDictionary<string, object> stateDictionary)
        {
            this.SaveState(stateDictionary, transientState, transientStateLock);
        }

        public void SavePersistentState(IDictionary<string, object> stateDictionary)
        {
            this.SaveState(stateDictionary, persistentState, persistentStateLock);
        }

        public void LoadTransientState(IDictionary<string, object> stateDictionary)
        {
            this.LoadState(stateDictionary, transientState, transientStateLock);
        }

        public void LoadPersistentState(IDictionary<string, object> stateDictionary)
        {
            this.LoadState(stateDictionary, persistentState, persistentStateLock);
        }

        //public void DeregisterAllStates()
        //{
        //    this.DeregisterAllStates(ApplicationStateType.Persistent);
        //    this.DeregisterAllStates(ApplicationStateType.Transient);
        //}

        //public void DeregisterAllStates(ApplicationStateType applicationStateType)
        //{
        //    switch (applicationStateType)
        //    {
        //        case ApplicationStateType.Transient:
        //            this.DeregisterAllStates(this.transientState, this.transientStateLock);
        //            break;
        //        case ApplicationStateType.Persistent:
        //            this.DeregisterAllStates(this.persistentState, this.persistentStateLock);
        //            break;
        //    }
        //}

        //public void DeregisterAndRemoveAllTransientStates()
        //{
        //    this.DeregisterAndRemoveAllStates(this.persistentState);
        //}

        //public void DeregisterAndRemoveAllPersistentStates()
        //{
        //}

        //public void DeregisterAndRemoveAllStates(IDictionary<string, object> stateDictionary, )
        //{

        //}

        public void DeregisterAndRemoveAllPersistentStates(IDictionary<string, object> stateDictionary)
        {
            this.DeregisterAndRemoveAllStates(stateDictionary, this.persistentState, this.persistentStateLock);
        }

        public void DeregisterAndRemoveAllTransientStates(IDictionary<string, object> stateDictionary)
        {
            this.DeregisterAndRemoveAllStates(stateDictionary, this.transientState, this.transientStateLock);
        }

        void DeregisterAndRemoveAllStates(IDictionary<string, object> stateDictionary, Dictionary<string, IStateAccessor> accessors, object propertiesLock)
        {
            lock (propertiesLock)
            {
                foreach (KeyValuePair<string, IStateAccessor> pair in accessors)
                {
                    string stateKey = pair.Key;
                    stateDictionary.Remove(stateKey);
                }
            }
        }

        //void DeregisterAllStates(Dictionary<string, IStateAccessor> accessors, object propertiesLock)
        //{
        //    lock (propertiesLock)
        //    {
        //        accessors.Clear();
        //    }
        //}

        void DeregisterTransientState(string stateKey)
        {
            ArgumentValidator.AssertNotNull(stateKey, "stateKey");

            lock (transientStateLock)
            {
                transientState.Remove(stateKey);
            }
        }

        void DeregisterPersistentState(string stateKey)
        {
            ArgumentValidator.AssertNotNull(stateKey, "stateKey");

            lock (persistentStateLock)
            {
                persistentState.Remove(stateKey);
            }
        }

        //protected byte[] Serialize(object value)
        //{
        //    // JsonConvert.DeserializeObject<T>(response.Content);
        //    // Newtonsoft.Json.JsonSerializer.Create
        //    byte[] state = Serializer.Write(value);
        //    return state;
        //}

        protected object Serialize(object value, Type t)
        {
            return Serializer.Write(value, t);
        }

        //protected T Deserialize<T>(byte[] data) where T : class
        //{
        //    T result = Serializer.Read<T>(data);
        //    return result;
        //}

        //protected T Deserialize<T>(byte[] data) where T : class
        //{
        //    T result = Serializer.Read<T>(data);
        //    return result;
        //}

        protected object Deserialize(object data, Type t)
        {
            return Serializer.Read(data, t);
        }

        void SaveState(IDictionary<string, object> stateDictionary, Dictionary<string, IStateAccessor> accessors, object propertiesLock)
		{
			lock (propertiesLock)
			{
				foreach (KeyValuePair<string, IStateAccessor> pair in accessors)
				{
					string stateKey = pair.Key;
					IStateAccessor accessor = pair.Value;

					object accessorValue = accessor.Value;
					if (accessorValue == null)
					{
						stateDictionary.Remove(stateKey);
						continue;
					}

                    object data;
					try
					{
                        data = Serialize(accessorValue, accessor.AccessorType);
					}
					catch (Exception ex)
					{
						stateDictionary[pair.Key] = null;
						Debug.Assert(false, "Unable to serialize state value. " + ex);
						continue;
					}

                    stateDictionary[stateKey] = data;
				}
			}
		}

        void LoadState(IDictionary<string, object> stateDictionary, Dictionary<string, IStateAccessor> accessors, object propertiesLock)
        {
            lock (propertiesLock)
            {
                foreach (KeyValuePair<string, IStateAccessor> pair in accessors)
                {
                    object stateValue;
                    string stateKey = pair.Key;
                    IStateAccessor accessor = pair.Value;

                    if (!stateDictionary.TryGetValue(stateKey, out stateValue))
                    {
                        continue;
                    }

                    object data = stateValue;
                    //if (bytes == null)
                    //{
                    //    Debug.Assert(false, "state value is not a byte[]");
                    //    continue;
                    //}

                    object deserializedValue;
                    try
                    {
                        deserializedValue = Deserialize(data, accessor.AccessorType);
                    }
                    catch (Exception ex)
                    {
                        string message = "Unable to deserialize bytes. " + ex;
                        Console.WriteLine(message);
                        Debug.Assert(false, message);
                        continue;
                    }

                    if (deserializedValue == null)
                    {
                        const string message = "Deserialized object should not be null.";
                        Console.WriteLine(message);
                        Debug.Assert(false, message);
                        continue;
                    }

                    try
                    {
                        accessor.Value = deserializedValue;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to set state value. " + ex);
                        continue;
                    }
                }
            }
        }

        class Accessor<T> : IStateAccessor
        {
            readonly Func<T> getter;
            readonly Action<T> setter;

            public Accessor(Func<T> getter, Action<T> setter, Type t)
            {
                this.getter = getter;
                this.setter = setter;
                this.AccessorType = t ?? typeof(T);
            }

            public Type AccessorType
            {
                get;
                private set;
            }

            public object Value
            {
                get
                {
                    return getter();
                }
                set
                {
                    setter((T)value);
                }
            }
        }

        interface IStateAccessor
        {
            object Value { get; set; }

            Type AccessorType { get; }
        }
    }
}
