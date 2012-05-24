using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TK.Model.Servers
{
    public class ScoreServer
    {
        private TKEntities entities = null;
        public ScoreServer() {
            entities = new TKEntities();
        }

        public MemberEventScore AddMemberEventScore( MemberEventScore mes, out string error ) {
            error = null;
            MemberEventScore retval = null;
            if (mes.Id > 0) {
                error = "Can not add a member score that has already been added";
            }
            else {
                try {
                    mes.CreatedAt = DateTime.Now;
                    mes.LastUpdated = DateTime.Now;
                    entities.AddToMemberEventScores(mes);
                    entities.SaveChanges();
                    retval = mes;
                }
                catch (Exception x) {
                    error = x.Message + (x.InnerException != null ? "Inner exception: " + x.InnerException.Message : "");
                    retval = null;
                }
            }
            return retval;
        }
    }
}
