using GalaSoft.MvvmLight;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using System.Net;
using System;
using System.IO;
using TK.ViewModels;

using System.Linq;

using Newtonsoft.Json;
using System.Threading;
using System.ComponentModel;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;


namespace TK.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private static string baseUrl = "http://localhost:63306/api/";

        Dictionary<string, string> urls = new Dictionary<string, string> {
            { "Member", baseUrl + "Member" },
            { "EventType", baseUrl + "EventType" },
            { "Location", baseUrl + "Location" },
            { "Event", baseUrl + "Events" },
        };
        Dictionary<string,WaitHandle> waitForUrls = null;

        private Dictionary<long, EventTypeVM> eventTypesById = null;
        private Dictionary<long, LocationVM> locationsById = null;
        public Dictionary<long, MemberVM> MembersById = null;
        public Dictionary<long, EventVM> EventsById = null;

        private Dictionary<long, List<MemberEventScoreVM>> eventScoresByUser = null;
        private Dictionary<long, List<EventVM>> eventsByLocation = null;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel() {
            if (IsInDesignMode) {
                _events = new List<EventVM> {
                    new EventVM { Name = "Crossing Jordan", Score = 1.1F },
                    new EventVM { Name = "Moving heaven and earth", Score = 5.5F },
                };
            }
            else {
                // Code runs "for real"
            }

            // Get the data
            waitForUrls = new Dictionary<string, WaitHandle>();
            foreach (var key in urls.Keys) {
                waitForUrls.Add(key, new AutoResetEvent(false) );
            }

            GetEntities("EventType", GetEventTypes_OpenReadCompleted);
            GetEntities("Location", GetLocations_OpenReadCompleted);
            GetEntities("Member", GetMembers_OpenReadCompleted);
            GetEntities("Event", GetEvents_OpenReadCompleted);

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(ReCalculate);
            bw.RunWorkerAsync();
        }

        void ReCalculate(object sender, DoWorkEventArgs e) {
            // Wait for all of the Read functions to complete
            foreach (var mutex in waitForUrls.Values) {
                mutex.WaitOne();
            }

            // Recalculate totals
            eventsByLocation = new Dictionary<long,List<EventVM>>();
            foreach (var evt in Events) {
                if (!eventsByLocation.ContainsKey(evt.LocationId) ) {
                    eventsByLocation.Add(evt.LocationId, new List<EventVM>());
                }
                eventsByLocation[evt.LocationId].Add(evt);
            }
            foreach (var loc in Locations) {
                loc.Score  = eventsByLocation[loc.Id].Average(l => l.Score);
            }

            eventScoresByUser = new Dictionary<long, List<MemberEventScoreVM>>();
            foreach (var evt in Events) {
                foreach (var mes in evt.MemberEventScores) {
                    if (!eventScoresByUser.ContainsKey(mes.MemberId)) {
                        eventScoresByUser.Add(mes.MemberId, new List<MemberEventScoreVM>());
                    }
                    mes.Member = MembersById[mes.MemberId];
                    mes.Event = EventsById[mes.EventId];
                    mes.Date = EventsById[mes.EventId].Date;
                    eventScoresByUser[mes.MemberId].Add(mes);
                }
            }
            foreach (var member in Members) {
                foreach (var mes in member.MemberEventScores) {
                    mes.Member = MembersById[mes.MemberId];
                    mes.Event = EventsById[mes.EventId];
                }
                member.MemberEventScores = member.MemberEventScores.OrderByDescending(m => m.Event.Date).ToList();

                if (eventScoresByUser.ContainsKey(member.Id)) {
                    IEnumerable<MemberEventScoreVM> evtsThisYear = eventScoresByUser[member.Id].Where(es => es.Date.Value.Year == DateTime.Now.Year);
                    var evtsLastYear = eventScoresByUser[member.Id].Where(es => es.Date.Value.Year == DateTime.Now.Year - 1);
                    var evtsTotal = eventScoresByUser[member.Id];

                    member.AvgLastYear = average(evtsLastYear);
                    member.AvgThisYear = average(evtsThisYear);
                    member.AvgTotal = average(evtsTotal);

                    member.EventsTotal = count(evtsTotal);
                    member.EventsLastYear = count(evtsLastYear);
                    member.EventsThisYear = count(evtsThisYear);
                }
            }
        }

        private float average(IEnumerable<MemberEventScoreVM> eventScores) {
            try { return (float)eventScores.Average(es => es.Score); } catch { }
            return 0F;
        }

        private int count(IEnumerable<MemberEventScoreVM> eventScores) {
            try { return eventScores.Count(); } catch { }
            return 0;
        }

        private void GetEntities(string entityName, OpenReadCompletedEventHandler orcDelegate) {
            Uri uri = new Uri(urls[entityName], UriKind.Absolute);
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += orcDelegate;
            webClient.OpenReadAsync(uri, entityName);
        }

        void GetEvents_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e) {
            if (e.Error == null) {
                StreamReader sr = new StreamReader(e.Result);
                string s = sr.ReadToEnd();
                var evts = JsonConvert.DeserializeObject<EventVM[]>(s);
                List<EventVM> evtList = new List<EventVM>();
                EventsById = new Dictionary<long, EventVM>();
                foreach (var evt in evts) {
                    EventsById.Add(evt.Id, evt);
                    evtList.Add(evt);
                }
                Events = evtList;
            }
            ((AutoResetEvent)waitForUrls[e.UserState as string]).Set();
        }
        void GetEventTypes_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e) {
            if (e.Error == null) {
                StreamReader sr = new StreamReader(e.Result);
                string s = sr.ReadToEnd();
                var ets = JsonConvert.DeserializeObject<EventTypeVM[]>(s);
                List<EventTypeVM> etList = new List<EventTypeVM>();
                eventTypesById = new Dictionary<long, EventTypeVM>();
                foreach (var et in ets) {
                    eventTypesById.Add(et.Id, et);
                    etList.Add(et);
                }
                EventTypes = etList;
            }
            ((AutoResetEvent)waitForUrls[e.UserState as string]).Set();
        }
        void GetLocations_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e) {
            if (e.Error == null) {
                StreamReader sr = new StreamReader(e.Result);
                string s = sr.ReadToEnd();
                var locs = JsonConvert.DeserializeObject<LocationVM[]>(s);
                List<LocationVM> locList = new List<LocationVM>();
                locationsById = new Dictionary<long, LocationVM>();
                foreach (var loc in locs) {
                    locationsById.Add(loc.Id, loc);
                    locList.Add(loc);
                }
                Locations = locList;
            }
            ((AutoResetEvent)waitForUrls[e.UserState as string]).Set();
        }
        private void GetMembers_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e) {
            if (e.Error == null) {
                StreamReader sr = new StreamReader(e.Result);
                string s = sr.ReadToEnd();
                e.Result.Seek(0, 0);
                var members = JsonConvert.DeserializeObject<MemberVM[]>(s);
                var membersList = new List<MemberVM>();
                MembersById = new Dictionary<long, MemberVM>();
                foreach (var member in members) {
                    MembersById.Add(member.Id, member);
                    membersList.Add(member);
                }
                Members = membersList;
            }
            ((AutoResetEvent)waitForUrls[e.UserState as string]).Set();
        }


        public RelayCommand ButtonCommand { get; set; }


        /// <summary>
        /// Gets the Events property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string EventsPropertyName = "Events";
        private List<EventVM> _events = null;
        public List<EventVM> Events {
            get { return _events; }
            set {
                if (_events == value) {
                    return;
                }
                var oldValue = _events;
                _events = value;
                RaisePropertyChanged(EventsPropertyName, oldValue, value, true);
            }
        }


        /// <summary>
        /// Gets the SelectedEvent property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string SelectedEventPropertyName = "SelectedEvent";
        private EventVM _selectedEvent = null;
        public EventVM SelectedEvent {
            get { return _selectedEvent; }
            set {
                if (_selectedEvent == value) {
                    return;
                }
                var oldValue = _selectedEvent;
                _selectedEvent = value;
                RaisePropertyChanged(SelectedEventPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// Gets the Members property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string MembersPropertyName = "Members";
        private List<MemberVM> _members = null;
        public List<MemberVM> Members {
            get { return _members; }
            set {
                if (_members == value) {
                    return;
                }
                var oldValue = _members;
                _members = value;
                RaisePropertyChanged(MembersPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// Gets the ApplicationTitle property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string ApplicationTitlePropertyName = "ApplicationTitle";
        private string _applicationTitle = "torsdagskino";
        public string ApplicationTitle {
            get { return _applicationTitle; }
            set {
                if (_applicationTitle == value) {
                    return;
                }
                _applicationTitle = value;
                RaisePropertyChanged(ApplicationTitlePropertyName);
            }
        }

        /// <summary>
        /// Gets the FirstPageName property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string FirstPageNamePropertyName = "FirstPageName";
        private string _firstPageName = "oversikt";
        public string FirstPageName {
            get { return _firstPageName; }
            set {
                if (_firstPageName == value) {
                    return;
                }
                _firstPageName = value;
                RaisePropertyChanged(FirstPageNamePropertyName);
            }
        }

        /// <summary>
        /// Gets the LocationsPageName property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string LocationsPageNamePropertyName = "LocationsPageName";
        private string _locationsPageName = "lokasjoner";
        public string LocationsPageName {
            get { return _locationsPageName; }
            set {
                if (_locationsPageName == value) {
                    return;
                }
                var oldValue = _locationsPageName;
                _locationsPageName = value;
                RaisePropertyChanged(LocationsPageNamePropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// Gets the StatsPageName property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string StatsPageNamePropertyName = "StatsPageName";
        private string _statsPageName = "statistikk";
        public string StatsPageName {
            get { return _statsPageName; }
            set {
                if (_statsPageName == value) {
                    return;
                }
                _statsPageName = value;
                RaisePropertyChanged(StatsPageNamePropertyName);
            }
        }

        /// <summary>
        /// Gets the MembersPageName property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string MembersPageNamePropertyName = "MembersPageName";
        private string _membersPageName = "medlemmer";
        public string MembersPageName {
            get { return _membersPageName; }
            set {
                if (_membersPageName == value) {
                    return;
                }
                _membersPageName = value;
                RaisePropertyChanged(MembersPageNamePropertyName);
            }
        }

        /// <summary>
        /// Gets the SettingsPageName property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string SettingsPageNamePropertyName = "SettingsPageName";
        private string _settingsPageName = "innstillinger";
        public string SettingsPageName {
            get { return _settingsPageName; }
            set {
                if (_settingsPageName == value) {
                    return;
                }
                _settingsPageName = value;
                RaisePropertyChanged(SettingsPageNamePropertyName);
            }
        }

        /// <summary>
        /// Gets the Welcome property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string WelcomePropertyName = "Welcome";
        private string _welcome = "Welcome to MVVM Light";
        public string Welcome {
            get { return _welcome; }
            set {
                if (_welcome == value) {
                    return;
                }
                _welcome = value;
                RaisePropertyChanged(WelcomePropertyName);
            }
        }

        /// <summary>
        /// Gets the EventTypes property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string EventTypesPropertyName = "EventTypes";
        private List<EventTypeVM> _eventTypes = null;
        public List<EventTypeVM> EventTypes {
            get { return _eventTypes; }
            set {
                if (_eventTypes == value) {
                    return;
                }
                var oldValue = _eventTypes;
                _eventTypes = value;
                RaisePropertyChanged(EventTypesPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// Gets the Locations property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string LocationsPropertyName = "Locations";
        private List<LocationVM> _locations = null;
        public List<LocationVM> Locations {
            get { return _locations; }
            set {
                if (_locations == value) {
                    return;
                }
                var oldValue = _locations;
                _locations = value;
                RaisePropertyChanged(LocationsPropertyName, oldValue, value, true);
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}