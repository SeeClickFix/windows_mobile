using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.ViewModel
{
    public class LoginRegisterViewModel : BaseViewModelState
    {
        public LoginViewModel Login { get; private set; }
        public RegisterViewModel Register { get; private set; }

        public RelayCommand LoginCommand { get; private set; }
        public RelayCommand RegisterCommand { get; private set; }
        
        public LoginRegisterViewModel()
        {
            this.Login = new LoginViewModel();
            this.Register = new RegisterViewModel();

            this.LoginCommand = this.Login.LoginCommand;
            this.RegisterCommand = this.Register.RegisterCommand;
        }

        public override void LoadState(IDictionary<string, object> persistentStateDictionary, IDictionary<string, object> transientStateDictionary, bool shouldLoadTransientState)
        {
            this.Login.LoadState(persistentStateDictionary, transientStateDictionary, shouldLoadTransientState);
            this.Register.LoadState(persistentStateDictionary, transientStateDictionary, shouldLoadTransientState);
        }

        public override void SaveState(IDictionary<string, object> persistentStateDictionary, IDictionary<string, object> transientStateDictionary)
        {
            this.Login.SaveState(persistentStateDictionary, transientStateDictionary);
            this.Register.SaveState(persistentStateDictionary, transientStateDictionary);
        }

        public override void ClearState(IDictionary<string, object> persistentStateDictionary, IDictionary<string, object> transientStateDictionary)
        {
            this.Login.ClearState(persistentStateDictionary, transientStateDictionary);
            this.Register.ClearState(persistentStateDictionary, transientStateDictionary);
        }
    }
}
