using System.Collections.Generic;

namespace SeeClickFix.WP8.Infrastructure
{
	public interface IStatePreservation
	{
		void LoadState(IDictionary<string, object> persistentStateDictionary,
					    IDictionary<string, object> transientStateDictionary,
                        bool loadTransientStateRequired);

		void SaveState(IDictionary<string, object> persistentStateDictionary,
						IDictionary<string, object> transientStateDictionary);

        void ClearState(IDictionary<string, object> persistentStateDictionary,
                        IDictionary<string, object> transientStateDictionary);
	}
}
