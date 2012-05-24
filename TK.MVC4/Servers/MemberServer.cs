using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TK.Model.Servers;
using TK.MVC4.Models;
using TK.Model;

namespace TK.MVC4.Servers
{
    public class MemberServer
    {
        private MemberDataServer memberDataServer = null;
        private EventDataServer eventDataServer = null;

        public MemberServer() {
            memberDataServer = new MemberDataServer();
            eventDataServer = new EventDataServer();
        }

        public List<MemberVM> GetMembers() {
            try {
                List<Member> members = memberDataServer.Members.OrderBy(m => m.SortOrder).ToList();
                return (from m in members
                        select MemberVM.FromMember(m)).ToList();
            }
            catch {
            }
            return new List<MemberVM>();
        }
        public MemberVM GetMember(long id) {
            try {
                return MemberVM.FromMember(memberDataServer.Members.Where(m => m.Id == id).SingleOrDefault());
            }
            catch {
            }
            return null;
        }
        public MemberVM AddMember(MemberVM member, out string error) {
            return MemberVM.FromMember( memberDataServer.AddMember(member.ToMember(), out error) );
        }

        public MemberVM GetMemberAndScores(long id) {
            Member member = null;
            MemberVM retval = null;
            try {
                member = memberDataServer.Members.Include("MemberEventScores").Where(m => m.Id == id).SingleOrDefault();
            }
            catch {
            }
            if (member != null) {
                retval = MemberVM.FromMember(member);
                int sumTotal = 0;
                int sumThisYear = 0;
                int sumLastYear = 0;
                int eventsTotal = member.MemberEventScores.Count();
                int eventsThisYear = 0;
                int eventsLastYear = 0;

                foreach (var s in member.MemberEventScores) {
                    s.Event = s.Event == null ? eventDataServer.GetEvent(s.EventId) : s.Event;
                    sumTotal += s.Score;
                    if (s.Event.Date.Year == DateTime.Now.Year) {
                        sumThisYear += s.Score;
                        eventsThisYear++;
                    }
                    else if (s.Event.Date.Year == DateTime.Now.Year - 1) {
                        sumLastYear += s.Score;
                        eventsLastYear++;
                    }
                }
                retval.AvgThisYear = eventsThisYear > 0 ? (float)sumThisYear / (float)eventsThisYear : 0;
                retval.EventsThisYear = eventsThisYear;
                retval.AvgLastYear = eventsLastYear > 0 ? (float)sumLastYear / (float)eventsLastYear : 0;
                retval.EventsLastYear = eventsLastYear;
                retval.AvgTotal = eventsTotal > 0 ? (float)sumTotal / (float)eventsTotal : 0;
            }
            return retval;
        }
    }
}