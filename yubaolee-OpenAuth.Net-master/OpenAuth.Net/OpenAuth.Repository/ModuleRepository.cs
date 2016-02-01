﻿using System.Collections.Generic;
using System.Linq;
using OpenAuth.Domain;
using OpenAuth.Domain.Interface;

namespace OpenAuth.Repository
{
    public class ModuleRepository :BaseRepository<Module>, IModuleRepository
    {
        public IEnumerable<Module> LoadModules(int pageindex, int pagesize)
        {
            return Context.Modules.OrderBy(u => u.Id).Skip((pageindex - 1) * pagesize).Take(pagesize);
        }

        public int GetRoleCntInOrgs(params int[] orgIds)
        {
            return LoadInOrgs(orgIds).Count();
        }

        public int GetModuleCntInOrgs(params int[] orgIds)
        {
            return LoadInOrgs(orgIds).Count();
        }


        public IEnumerable<Module> LoadInOrgs(int pageindex, int pagesize, params int[] orgIds)
        {
            return LoadInOrgs(orgIds).OrderBy(u => u.Id).Skip((pageindex - 1) * pagesize).Take(pagesize);
        }

        public void Delete(int id)
        {
            Delete(u =>u.Id == id);
        }

        public IEnumerable<Module> LoadInOrgs(params int[] orgId)
        {
            var result = from role in Context.Modules.Where(u => orgId.Contains(u.ParentId)) select role;
                        
            return result;

        }
    }
}
