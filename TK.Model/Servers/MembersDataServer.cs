using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace TK.Model.Servers
{
    public class MemberDataServer
    {
        private TKEntities entities = null;
        public MemberDataServer() {
            entities = new TKEntities();
        }

        public ObjectSet<Member> Members {
            get {
                return entities.Members;
            }
        }

        public bool HasMember(Member member) {
            try {
                return entities.Members.Where(m => m.Nickname == member.Nickname).Count() > 0;
            }
            catch {
            }
            return false;
        }
        public Member AddMember(Member member, out string error) {
            error = null;
            Member retval = null;
            if (member.Id > 0 || HasMember(member)) {
                error = "Can not add a member that has already been added";
            }
            else {
                try {
                    entities.AddToMembers(member);
                    entities.SaveChanges();
                    retval = member;
                }
                catch (Exception x) {
                    error = x.Message + (x.InnerException != null ? "Inner exception: " + x.InnerException.Message : "");
                    retval = null;
                }
            }
            return retval;
        }

        public Member GetMemberAndScores(int memberId) {
            try {
                return entities.Members.Include("MemberEventScores").Where(m => m.Id == memberId).SingleOrDefault();
            }
            catch {
            }
            return null;
        }
    }
}
