using GalaSoft.MvvmLight.Command;
using SeeClickFix.WP8.Infrastructure;
using SeeClickFix.WP8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.ViewModel
{
    public class SettingsViewModel : BaseViewModelState
    {
        bool useLocationServices;

        [Stateful(ApplicationStateType.Transient)]
        public bool UseLocationServices
        {
            get { return this.useLocationServices; }

            set
            {
                if (this.useLocationServices != value)
                {
                    this.useLocationServices = value;
                    this.Save();
                }
            }
        }

        public RelayCommand SaveCommand { get; private set; }

        public SettingsViewModel()
        {
            this.SaveCommand = new RelayCommand(this.Save);
            this.UseLocationServices = AppSettingsService.Inst.Settings.UseLocationServices;
        }

        void Save()
        {
            AppSettingsService.Inst.Settings.UseLocationServices = this.UseLocationServices;
            AppSettingsService.Inst.Save();
        }
    }
}
