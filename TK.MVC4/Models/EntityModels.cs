using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TK.Model;
using TK.Model.Servers;
using System.Text.RegularExpressions;

namespace TK.MVC4.Models
{
    public class EntityVM
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MemberVM : EntityVM
    {
        public string Nickname { get; set; }
        public string Lastname { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public DateTime? Fodselsdato { get; set; }
        public string Phone { get; set; }
        public int SortOrder { get; set; }
        public bool Active { get; set; }
        public IQueryable<MemberEventScore> MemberEventScores { get; set; }

        public int EventsLastYear { get; set; }
        public int EventsThisYear { get; set; }
        public float AvgTotal { get; set; }
        public float AvgThisYear { get; set; }
        public float AvgLastYear { get; set; }

        public static MemberVM FromMember(Member member) {
            if (member == null) { return null; }
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
                MemberEventScores = member.MemberEventScores != null ? member.MemberEventScores.AsQueryable() : null,
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class EventVM : EntityVM
    {
        public DateTime Date { get; set; }
        public EventType EventType { get; set; }
        public long EventTypeId { get; set; }
        public string FilmwebId { get; set; }
        public string IMDBId { get; set; }
        public Location Location { get; set; }
        public long LocationId { get; set; }
        public string Name { get; set; }

        public static EventVM FromEvent(Event evt) {
            if (evt == null) { return null; }

            return new EventVM {
                CreatedAt = evt.CreatedAt,
                Date = evt.Date,
                EventType = evt.EventType,
                EventTypeId = evt.EventTypeId,
                FilmwebId = evt.FilmwebId,
                Id = evt.Id,
                IMDBId = evt.IMDBId,
                LastUpdated = evt.LastUpdated,
                Location = evt.Location,
                LocationId = evt.LocationId,
                Name = evt.Name,
            };
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
                LocationId = (new LocationServer()).GetLocationIdByName(sal),
                Name = name,
                Id = 0,
                EventTypeId = (new EventTypeServer()).GetEventTypeIdByName("Film"),
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class LocationVM : EntityVM
    {
        public static LocationVM FromLocation(Location loc) {
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
        public Location ToLocation() {
            return new Location {
                Capacity = this.Capacity,
                CreatedAt = this.CreatedAt,
                Id = this.Id,
                LastUpdated = this.LastUpdated,
                Name = this.Name,
                Score = this.Score,
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

        public int? Capacity { get; set; }
        public string Name { get; set; }
        public int? Score { get; set; }
    }

    public class MemberEventScoreVM : EntityVM
    {
        public static MemberEventScoreVM FromMemberEventScore(MemberEventScore mes) {
            if (mes == null) { return null; }
            return new MemberEventScoreVM {
                Comment = mes.Comment,
                CreatedAt = mes.CreatedAt,
                Event = mes.Event,
                EventId = mes.EventId,
                Id = mes.Id,
                LastUpdated = mes.LastUpdated,
                Member = mes.Member,
                MemberId = mes.MemberId,
                Score = mes.Score,
            };
        }
        public MemberEventScore ToDataObject() {
            return new MemberEventScore {
                Comment = this.Comment,
                CreatedAt = this.CreatedAt,
                Event = this.Event,
                EventId = this.EventId,
                Id = this.Id,
                LastUpdated = this.LastUpdated != null ? this.LastUpdated.Value : DateTime.MinValue,
                Member = this.Member,
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
                Event = null,
                EventId = evt.Id,
                Id = 0,
                Member = null,
                MemberId = who.Id,
                Score = score,
            };
        }

        public string Comment { get; set; }
        public Event Event { get; set; }
        public long EventId { get; set; }
        public Member Member { get; set; }
        public long MemberId { get; set; }
        public int Score { get; set; }
    }
}