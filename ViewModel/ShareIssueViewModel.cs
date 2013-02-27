using GalaSoft.MvvmLight.Command;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.ViewModel
{
    public class ShareIssueViewModel : BaseViewModel
    {
        public Issue Issue
        {
            get;
            private set;
        }

        public IEnumerable<ShareServiceType> ShareServices
        {
            get;
            private set;
        }

        public RelayCommand<ShareServiceType> ShareCommand
        {
            get;
            private set;
        }

        public ShareIssueViewModel()
        {
            this.Issue = ViewModelLocator.Instance.Main.IssueList.SelectedIssue;
            this.ShareServices = Enum.GetValues(typeof(ShareServiceType)).Cast<ShareServiceType>();
            this.ShareCommand = new RelayCommand<ShareServiceType>(this.ShareIssue);
        }

        public void ShareIssue(ShareServiceType service)
        {
            ShareService.Inst.Share(this.Issue, service);
        }
    }
}
