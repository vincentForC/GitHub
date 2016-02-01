﻿// ***********************************************************************
// Assembly         : OpenAuth.Domain
// Author           : yubaolee
// Created          : 10-25-2015
//
// Last Modified By : yubaolee
// Last Modified On : 10-25-2015
// ***********************************************************************
// <copyright file="IRepository.cs" company="www.cnblogs.com/yubaolee">
//     Copyright (c) www.cnblogs.com/yubaolee. All rights reserved.
// </copyright>
// <summary>仓储接口</summary>
// ***********************************************************************

using System;
using System.Linq;
using System.Linq.Expressions;

namespace OpenAuth.Domain.Interface
{
    public interface IRepository<T> where T : class
    {
        T FindSingle(Expression<Func<T, bool>> exp = null);

        IQueryable<T> Find(Expression<Func<T, bool>> exp = null);

        IQueryable<T> Find(int pageindex = 1, int pagesize = 10, string orderby = "",
            Expression<Func<T, bool>> exp = null);

        int GetCount(Expression<Func<T, bool>> exp = null);

        void Add(T entity);

        /// <summary>
        /// 更新一个实体的所有属性
        /// </summary>
        void Update(T entity);

        void Delete(T entity);

        /// <summary>
        /// 批量更新
        /// </summary>
        void Update(Expression<Func<T, bool>> exp, T entity);

        /// <summary>
        /// 批量删除
        /// </summary>
        void Delete(Expression<Func<T, bool>> exp);

        void Save();
    }
}