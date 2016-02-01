﻿using OpenAuth.App.ViewModel;
using OpenAuth.Domain;
using OpenAuth.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenAuth.App
{
    public class UserManagerApp
    {
        private IUserRepository _repository;
        private IOrgRepository _orgRepository;
        private IRelevanceRepository _relevanceRepository;

        public UserManagerApp(IUserRepository repository,
            IOrgRepository orgRepository,
            IRelevanceRepository relevanceRepository)
        {
            _repository = repository;
            _orgRepository = orgRepository;
            _relevanceRepository = relevanceRepository;
        }

        public int GetUserCntInOrg(int orgId)
        {
            if (orgId == 0)
            {
                return _repository.Find(null).Count();
            }
            else
            {
                return _repository.GetUserCntInOrgs(GetSubOrgIds(orgId));
            }
        }

        /// <summary>
        /// 加载一个部门及子部门全部用户
        /// </summary>
        public dynamic Load(int orgId, int pageindex, int pagesize)
        {
            if (pageindex < 1) pageindex = 1;  //TODO:如果列表为空新增加一个用户后，前端会传一个0过来，奇怪？？
            IEnumerable<User> users;
            int total = 0;
            if (orgId == 0)
            {
                users = _repository.LoadUsers(pageindex, pagesize);
                total = _repository.GetCount();
            }
            else
            {
                users = _repository.LoadInOrgs(pageindex, pagesize,GetSubOrgIds(orgId));
                total = _repository.GetUserCntInOrgs(orgId);
            }
            var userviews = new List<UserView>();
            foreach (var user in users)
            {
                UserView uv = user;
                uv.Organizations = string.Join(",", _orgRepository.LoadByUser(user.Id).Select(u => u.Name).ToList());
                userviews.Add(uv);
            }

            return new 
            {
                total = total,
                list = userviews,
                pageCurrent = pageindex
            };
        }

        /// <summary>
        /// 获取当前组织的所有下级组织
        /// </summary>
        private int[] GetSubOrgIds(int orgId)
        {
            var org = _orgRepository.FindSingle(u => u.Id == orgId);
            var orgs = _orgRepository.Find(u => u.CascadeId.Contains(org.CascadeId)).Select(u => u.Id).ToArray();
            return orgs;
        }

        public UserView Find(int id)
        {
            var user = _repository.FindSingle(u => u.Id == id);
            if (user == null) return new UserView();

            UserView view = user;
            foreach (var org in _orgRepository.LoadByUser(id))
            {
                view.Organizations += "," + org.Name;
                view.OrganizationIds += "," + org.Id;
            }
            view.OrganizationIds = view.OrganizationIds.TrimStart(',');
            view.Organizations = view.Organizations.TrimStart(',');
            return view;
        }

        public void Delete(int id)
        {
            _repository.Delete(u =>u.Id == id);
            _relevanceRepository.DeleteBy("UserOrg", id);
            _relevanceRepository.DeleteBy("UserModule", id);
            _relevanceRepository.DeleteBy("UserRole", id);
        }

        public void AddOrUpdate(UserView view)
        {
            User user = view;
            if (user.Id == 0)
            {
                user.CreateTime = DateTime.Now;
                user.Password = user.Account; //初始密码与账号相同
                _repository.Add(user);
                view.Id = user.Id;   //要把保存后的ID存入view
            }
            else
            {
                _repository.Update(user);
            }
            int[] orgIds = view.OrganizationIds.Split(',').Select(id => int.Parse(id)).ToArray();

            _relevanceRepository.DeleteBy("UserOrg", user.Id);
            _relevanceRepository.AddRelevance("UserOrg", orgIds.ToLookup(u =>user.Id));
        }

    }
}