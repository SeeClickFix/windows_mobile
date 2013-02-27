using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.Infrastructure;
using SeeClickFix.WP8.SeeClickFixAPI;
using SeeClickFix.WP8.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Windows.Phone.Speech.Recognition;

namespace SeeClickFix.WP8.ViewModel
{
    public class NewCommentViewModel : BaseViewModelState
    {
        string photoFileName;

        IssueHistoryItemType commentType;
        [Stateful(ApplicationStateType.Transient)]
        public IssueHistoryItemType CommentType
        {
            get { return this.commentType; }
            set
            {
                if (this.commentType != value)
                {
                    this.commentType = value;
                    this.RaisePropertyChanged("CommentType");
                }
            }
        }

        //public event Action<Exception> CommentSent = null;

        public RelayCommand SendCommentCommand { get; private set; }
        public ICommand AttachCommentCommand { get; private set; }
        public ICommand SpeakCommentCommand { get; private set; }

        public Issue Issue
        {
            get;
            private set;
        }

        string commentText;
        [Stateful(ApplicationStateType.Transient)]
        public string CommentText
        {
            get { return this.commentText; }
            set
            {
                if (this.commentText != value)
                {
                    this.commentText = value;
                    this.RaisePropertyChanged("CommentText");
                    this.SendCommentCommand.RaiseCanExecuteChanged();
                }
            }
        }

        string commentName;
        [Stateful(ApplicationStateType.Transient)]
        public string CommentName
        {
            get
            {
                return this.commentName;
            }
            set
            {
                if (this.commentName != value)
                {
                    this.commentName = value;
                    this.RaisePropertyChanged("CommentName");
                }
            }
        }

        string commentEmail;
        [Stateful(ApplicationStateType.Transient)]
        public string CommentEmail
        {
            get { return this.commentEmail; }
            set
            {
                if (this.commentEmail != value)
                {
                    this.commentEmail = value;
                    this.RaisePropertyChanged("CommentEmail");
                    this.SendCommentCommand.RaiseCanExecuteChanged();
                }
            }
        }

        //public IssueStatus? SelectedIssueStatus
        //{
        //    get;
        //    private set;
        //}

        //public IEnumerable<IssueStatus> IssueStatusOptions { get; private set; }

        [Stateful(ApplicationStateType.Transient)]
        public string YoutubeVideoUrl
        {
            get;
            set;
        }

        Stream photo;
        public Stream Photo
        {
            get { return this.photo; }
            set
            {
                this.photo = value;
                this.RaisePropertyChanged("Photo");
            }
        }

        //public static void SetNewCommentType(IssueHistoryItemType itemType)
        //{
        //    Messenger.Default.Send<IssueHistoryItemType>(itemType, "SetNewCommentType");
        //}

        public NewCommentViewModel()
        {
            //this.RegisterStatefulProperty(ApplicationStateType.Transient, () => this.CommentType);
            //this.RegisterStatefulProperty(ApplicationStateType.Transient, () => this.CommentText);
            //this.RegisterStatefulProperty(ApplicationStateType.Transient, () => this.CommentName);
            //this.RegisterStatefulProperty(ApplicationStateType.Transient, () => this.CommentEmail);

            var viewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
            this.Issue = viewModel.IssueList.SelectedIssue;

            this.SendCommentCommand = new RelayCommand(this.SendComment, this.CanSendComment);
            this.AttachCommentCommand = new RelayCommand(this.AttachComment);
            this.SpeakCommentCommand = new RelayCommand(this.SpeakComment);

            this.commentType = IssueHistoryItemType.Comment;

            var userProfile = UserProfileService.Inst.UserProfile;
            this.CommentName = userProfile.Name;
            this.commentEmail = userProfile.Email;

            //Messenger.Default.Register<UserLogin>(this, Messages.RefreshUserLogin, (userLogin) =>
            //{
            //    this.OnLoginProfileChanged();
            //});

            this.SubcribeToUserProfileChanges();

            //Messenger.Default.Register<IssueHistoryItemType>(this, "SetNewCommentType", (itemType) =>
            //    {
            //        this.CommentType = itemType;
            //        this.RaisePropertyChanged("CommentType");
            //    });

            // this.UpdateIssueStatusOptions();
        }

        //void UpdateIssueStatusOptions()
        //{
        //    var statusOptions = Enum.GetValues(typeof(IssueStatus)).Cast<IssueStatus>().ToList();

        //    // nobody can archive it
        //    statusOptions.Remove(SeeClickFixAPI.IssueStatus.Archived);

        //    // TODO: user profiles with this right can do this
        //    statusOptions.Remove(SeeClickFixAPI.IssueStatus.Acknowledged);

        //    if (this.Issue.Status == SeeClickFixAPI.IssueStatus.Closed)
        //    {
        //        // issue is closed
        //        // remove close option
        //        statusOptions.Remove(IssueStatus.Closed);
        //        statusOptions.Remove(IssueStatus.Acknowledged);
        //    }
        //    else if (this.Issue.Status == SeeClickFixAPI.IssueStatus.Open)
        //    {
        //        // issue is opened
        //        // remove open option
        //        statusOptions.Remove(IssueStatus.Open);
        //    }

        //    this.IssueStatusOptions = statusOptions;
        //}

        //IssueHistoryItemType ItemStatusToItemType()
        //{
        //    switch (this.SelectedIssueStatus)
        //    {
        //        case SeeClickFixAPI.IssueStatus.Open:
        //            return IssueHistoryItemType.Reopened;
        //        case SeeClickFixAPI.IssueStatus.Closed:
        //            return IssueHistoryItemType.Closed;
        //        case SeeClickFixAPI.IssueStatus.Acknowledged:
        //            return IssueHistoryItemType.Acknowledged;
        //        case null:
        //            return IssueHistoryItemType.Comment;
        //        default:
        //            throw new ArgumentException("this.SelectedIssueStatus");
        //    }
        //}

        bool CanSendComment()
        {
            return !this.IsBusy && !string.IsNullOrWhiteSpace(this.CommentText) && !string.IsNullOrWhiteSpace(this.CommentEmail) && EmailValidator.IsValid(this.CommentEmail);
        }

        async void SendComment()
        {
            try
            {
                if (EmailValidator.IsValid(this.CommentEmail))
                {
                    this.IsBusy = true;

                    var response = await SCFDataService.AddComment(
                        new Comment(this.Issue.Id, this.CommentText, this.CommentEmail)
                        {
                            Name = this.CommentName,
                            YoutubeURL = this.YoutubeVideoUrl,
                            Photo = this.Photo,
                            PhotoName = this.photoFileName,
                            ItemType = this.CommentType
                            //ItemType = this.ItemStatusToItemType()
                        });

                    UserProfileService.Inst.UserProfile.Name = this.CommentName;
                    UserProfileService.Inst.UserProfile.Email = this.CommentEmail;
                    UserProfileService.Inst.Save();

                    this.IsBusy = false;

                    if (this.CommentType == IssueHistoryItemType.Comment)
                    {
                        Messenger.Default.Send<string>(string.Empty, Messages.RefreshComments);
                    }
                    else
                    {
                        Messenger.Default.Send<int>(this.Issue.Id, Messages.RefreshIssue);
                    }
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("Enter a valid email", "SEECLICKFIX", MessageBoxButton.OK);
                }
            }

            catch (Exception ex)
            {
                this.IsBusy = false;
                MessageBox.Show("There was an error sending your comment", "SEECLICKFIX", MessageBoxButton.OK);
            }


            this.IsBusy = false;
            //if (string.IsNullOrWhiteSpace(response.Error))
            //{
            //    Messenger.Default.Send<string>(string.Empty, "RefreshComments");
            //    NavigationService.GoBack();
            //}
            //else
            //{
            //    MessageBox.Show("There was an error sending your comment.", "SEECLICKFIX", MessageBoxButton.OK);
            //}


            //if (this.CommentSent != null)
            //{
            //    this.CommentSent(response.Error != null ? new Exception(response.Error) : null);
            //}
        }

        async void AttachComment()
        {
            var photoChooserTask = new PhotoChooserTask()
            {
                ShowCamera = true
            };

            var result = await photoChooserTask.ShowAsync();
            if (result.TaskResult == TaskResult.OK)
            {
                this.Photo = result.ChosenPhoto;
                this.photoFileName = Path.GetFileName(result.OriginalFileName);
            }
        }

        async void SpeakComment()
        {
            // Create an instance of SpeechRecognizerUI.
            SpeechRecognizerUI recoWithUI = new SpeechRecognizerUI();

            // Start recognition (load the dictation grammar by default).
            SpeechRecognitionUIResult recoResult = await recoWithUI.RecognizeWithUIAsync();

            if (recoResult.ResultStatus == SpeechRecognitionUIStatus.Succeeded)
            {
                this.CommentText = recoResult.RecognitionResult.Text;
            }

            // Do something with the recognition result.
            //MessageBox.Show(string.Format("You said {0}.", recoResult.RecognitionResult.Text));
        }

        protected override void OnLoginProfileChanged()
        {
            var userProfile = UserProfileService.Inst.UserProfile;
            this.CommentName = userProfile.Name;
            this.CommentEmail = userProfile.Email;
        }

        protected override void OnBusyChanged()
        {
            this.SendCommentCommand.RaiseCanExecuteChanged();
        }
    }

    // http://stackoverflow.com/questions/14621101/issue-with-async-await-method-along-with-choosers-and-launchers

    public static class ExtensionMethods
    {
        public static Task<TTaskEventArgs> ShowAsync<TTaskEventArgs>(this ChooserBase<TTaskEventArgs> chooser)
            where TTaskEventArgs : TaskEventArgs
        {
            var taskCompletionSource = new TaskCompletionSource<TTaskEventArgs>();

            EventHandler<TTaskEventArgs> completed = null;

            completed = (s, e) =>
            {
                chooser.Completed -= completed;
                taskCompletionSource.SetResult(e);
            };

            chooser.Completed += completed;
            chooser.Show();

            return taskCompletionSource.Task;
        }
    }
}
