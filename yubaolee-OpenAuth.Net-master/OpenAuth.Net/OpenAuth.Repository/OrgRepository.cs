﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAuth.Domain;
using OpenAuth.Domain.Interface;

namespace OpenAuth.Repository
{
    public class OrgRepository : BaseRepository<Org>, IOrgRepository
    {
        public IEnumerable<Org> LoadOrgs()
        {
            return Find();
        }

        public IEnumerable<Org> LoadByUser(int userId)
        {
            var result = from userorg in Context.Relevances
                join org in Context.Orgs on userorg.SecondId equals org.Id
                where userorg.FirstId == userId && userorg.Key =="UserOrg"
                select org;
            return result;

        }

        public IEnumerable<Org> GetSubOrgs(int orgId)
        {
            string cascadeId = "0.";
            if (orgId != 0)
            {
                var org = FindSingle(u => u.Id == orgId);
                if (org == null)
                    throw new Exception("未能找到指定对象信息");
                cascadeId = org.CascadeId;
            }

            return Find(u => u.CascadeId.Contains(cascadeId) && u.Id != orgId);
        }
    }
}
