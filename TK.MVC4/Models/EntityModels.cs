using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

#if(SILVERLIGHT)
#else
using System.Web;
using TK.Model;
using TK.Model.Servers;
#endif


#if(SILVERLIGHT)
namespace TK.ViewModels
#else
namespace TK.MVC4.Models
#endif
{
    [DataContract]
    public class EntityVM
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public DateTime? LastUpdated { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class MemberVM : EntityVM
    {
        [DataMember]
        public string Nickname { get; set; }
        [DataMember]
        public string Lastname { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public DateTime? Fodselsdato { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public List<MemberEventScoreVM> MemberEventScores { get; set; }

        public int EventsLastYear { get; set; }
        public int EventsThisYear { get; set; }
        public int EventsTotal { get; set; }
        public float AvgTotal { get; set; }
        public float AvgThisYear { get; set; }
        public float AvgLastYear { get; set; }

        public string FullName {
            get {
                return FirstName + (MiddleName != null ? " " + MiddleName : "") + " " + Lastname;
            }
        }

#if(!SILVERLIGHT)
        public static MemberVM FromMember(Member member) {
            if (member == null) { return null; }
            List<MemberEventScoreVM> mesList = new List<MemberEventScoreVM>();
            if (member.MemberEventScores != null) {
                mesList = (from mes in member.MemberEventScores
                           select MemberEventScoreVM.FromMemberEventScore(mes)).ToList();
            }
            return new MemberVM {
                Active = member.Active,
                CreatedAt = member.CreatedAt,
                Email = member.Email,
                FirstName = member.FirstName,
                Fodselsdato = member.Fodselsdato,
                Id = member.Id,
                Lastname = member.Lastname,
                LastUpdated = member.LastUpdated,
                MiddleName = member.MiddleName,
                Nickname = member.Nickname,
                Phone = member.Phone,
                SortOrder = member.SortOrder,
                MemberEventScores = mesList,
            };
        }
        public Member ToMember() {
            return new Member {
                Active = this.Active,
                CreatedAt = this.CreatedAt,
                Email = this.Email,
                FirstName = this.FirstName,
                Fodselsdato = this.Fodselsdato,
                Id = this.Id,
                Lastname = this.Lastname,
                LastUpdated = this.LastUpdated,
                MiddleName = this.MiddleName,
                Nickname = this.Nickname,
                Phone = this.Phone,
                SortOrder = this.SortOrder,
            };
        }
        public static MemberVM FromCSVLine(string line) {
            if (line == null) { return null; }
            string[] words = line.Split(';');

            string fornavn = words[0];
            string mellomnavn = null;
            string etternavn = words[1];
            DateTime fodt = DateTime.Parse(words[2]);
            string alias = words[3];
            string email = words[4];
            int sortering = int.Parse(words[5]);
            if (words[0].Contains(' ')) {
                string[] forMellomnavn = fornavn.Split(' ');
                fornavn = forMellomnavn[0];
                mellomnavn = forMellomnavn[1];
            }

            return new MemberVM {
                Active = true,
                CreatedAt = DateTime.Now,
                Email = email,
                FirstName = fornavn,
                Fodselsdato = fodt,
                Id = 0,
                Lastname = etternavn,
                LastUpdated = DateTime.Now,
                MiddleName = mellomnavn,
                Nickname = alias,
                Phone = null,
                SortOrder = sortering,
            };
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class EventVM : EntityVM
    {
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public EventTypeVM EventType { get; set; }
        [DataMember]
        public long EventTypeId { get; set; }
        [DataMember]
        public string FilmwebId { get; set; }
        [DataMember]
        public string IMDBId { get; set; }
        [DataMember]
        public LocationVM Location { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<MemberEventScoreVM> MemberEventScores { get; set; }
        [DataMember]
        public float Score { get; set; }

#if(!SILVERLIGHT)
        public static EventVM FromDataObject(Event evt) {
            EventVM retval = null;
            if (evt != null) {
                retval = new EventVM {
                    CreatedAt = evt.CreatedAt,
                    Date = evt.Date,
                    EventType = EventTypeVM.FromDataObject(evt.EventType),
                    EventTypeId = evt.EventTypeId,
                    FilmwebId = evt.FilmwebId,
                    Id = evt.Id,
                    IMDBId = evt.IMDBId,
                    LastUpdated = evt.LastUpdated,
                    LocationId = evt.LocationId,
                    Location = LocationVM.FromDataObject(evt.Location),
                    Name = evt.Name,
                };

                List<MemberEventScore> memEvtsScores = evt.MemberEventScores.ToList();
                if (memEvtsScores != null && memEvtsScores.Count > 0) {
                    retval.MemberEventScores = (from mes in memEvtsScores
                                                select MemberEventScoreVM.FromMemberEventScore(mes)).ToList();
                }
            }
            if (retval.MemberEventScores != null && retval.MemberEventScores.Count > 0) {
                int sum = retval.MemberEventScores.Sum(mes => mes.Score);
                retval.Score = (float)sum / (float)retval.MemberEventScores.Count;
            }
            return retval;
        }
        public static EventVM FromCSVLine(string line) {
            if (line == null) { return null; }
            string[] words = line.Split(';');

            DateTime when = DateTime.Parse(words[0]);
            string name = words[1];
            string sal = words[2];
            string imdblink = words[3];
            string filmwebLink = words[4];

            imdblink = Regex.Replace(imdblink, "http://www.imdb.com/title/(.*)/", "$1");
            filmwebLink = Regex.Replace(filmwebLink, "http://www.filmweb.no/(.*)/article(.*).ece", "$1:$2");
            filmwebLink = Regex.Replace(filmwebLink, "http://www2.filmweb.no/film/article.jhtml\\?articleID=(.*)", "articleId:$1" );

            return new EventVM {
                CreatedAt = DateTime.Now,
                Date = when,
                FilmwebId = filmwebLink,
                IMDBId = imdblink,
                LastUpdated = DateTime.Now,
                Location = null,
                LocationId = (new LocationDataServer()).GetLocationIdByName(sal),
                Name = name,
                Id = 0,
                EventTypeId = (new EventTypeDataServer()).GetEventTypeIdByName("Film"),
            };
        }
        public Event ToEvent() {
            return new Event {
                CreatedAt = this.CreatedAt,
                Date = this.Date,
                EventTypeId = this.EventTypeId,
                FilmwebId = this.FilmwebId,
                Id = this.Id,
                IMDBId = this.IMDBId,
                LastUpdated = this.LastUpdated,
                LocationId = this.LocationId,
                Name = this.Name,
            };
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class LocationVM : EntityVM
    {
        [DataMember]
        public int? Capacity { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public float? Score { get; set; }

#if(!SILVERLIGHT)
        public static LocationVM FromDataObject(Location loc) {
            if (loc == null) { return null; }
            return new LocationVM {
                Capacity = loc.Capacity,
                CreatedAt = loc.CreatedAt,
                Id = loc.Id,
                LastUpdated = loc.LastUpdated,
                Name = loc.Name,
                Score = loc.Score,
            };
        }
        public Location ToDataObject() {
            return new Location {
                Capacity = this.Capacity,
                CreatedAt = this.CreatedAt,
                Id = this.Id,
                LastUpdated = this.LastUpdated,
                Name = this.Name,
                Score = (int)this.Score,
            };
        }
        public static LocationVM FromCSVLine(string line) {
            string[] words = line.Split(';');
            return new LocationVM {
                Name = words[0],
                Capacity = int.Parse(words[1]),
                CreatedAt = DateTime.Now,
                LastUpdated = DateTime.Now,
            };
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class MemberEventScoreVM : EntityVM
    {
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public long EventId { get; set; }
        [DataMember]
        public long MemberId { get; set; }
        [DataMember]
        public int Score { get; set; }

        public DateTime? Date { get; set; }
        public MemberVM Member { get; set; }
        public EventVM Event { get; set; }

#if(!SILVERLIGHT)
        public static MemberEventScoreVM FromMemberEventScore(MemberEventScore mes) {
            if (mes == null) { return null; }
            return new MemberEventScoreVM {
                Comment = mes.Comment,
                CreatedAt = mes.CreatedAt,
                EventId = mes.EventId,
                Id = mes.Id,
                LastUpdated = mes.LastUpdated,
                MemberId = mes.MemberId,
                Score = mes.Score,
            };
        }
        public MemberEventScore ToDataObject() {
            return new MemberEventScore {
                Comment = this.Comment,
                CreatedAt = this.CreatedAt,
                EventId = this.EventId,
                Id = this.Id,
                LastUpdated = this.LastUpdated != null ? this.LastUpdated.Value : DateTime.MinValue,
                MemberId = this.MemberId,
                Score = this.Score,
            };
        }
        public static MemberEventScoreVM FromCSVLine(string line) {
            if (line == null) { return null; }
            string[] words = line.Split(';');

            DateTime when = DateTime.Parse(words[0]);
            string name = words[1];
            int score = int.Parse(words[2]);

            Member who = (new MemberDataServer()).Members.Where(m => m.Nickname.ToUpper() == name.ToUpper()).SingleOrDefault();
            Event evt = (new EventDataServer()).GetEventByDate(when);

            return new MemberEventScoreVM {
                EventId = evt.Id,
                Id = 0,
                MemberId = who.Id,
                Score = score,
            };
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class EventTypeVM
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime Createdat { get; set; }
        [DataMember]
        public DateTime? LastUpdated { get; set; }

#if(!SILVERLIGHT)
        public static EventTypeVM FromDataObject(EventType et) {
            EventTypeVM retval = null;
            if (et != null) {
                retval = new EventTypeVM {
                    Id = et.Id,
                    Name = et.Name,
                    Createdat = et.Createdat,
                    LastUpdated = et.LastUpdated,
                };
            }
            return retval;
        }
#endif
    }
}