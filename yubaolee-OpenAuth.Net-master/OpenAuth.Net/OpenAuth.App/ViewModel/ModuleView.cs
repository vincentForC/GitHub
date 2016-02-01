﻿using Infrastructure;
using OpenAuth.Domain;
using System.Collections.Generic;

namespace OpenAuth.App.ViewModel
{
    public class ModuleView
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <returns></returns>
        public int Id { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// 主页面URL
        /// </summary>
        /// <returns></returns>
        public string Url { get; set; }

        /// <summary>
        /// 父节点流水号
        /// </summary>
        /// <returns></returns>
        public int ParentId { get; set; }

        /// <summary>
        /// 节点图标文件名称
        /// </summary>
        /// <returns></returns>
        public string IconName { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<ModuleView> Childern = new List<ModuleView>();

        /// <summary>
        /// 模块中的元素
        /// </summary>
        public List<ModuleElement> Elements = new List<ModuleElement>();

        public static implicit operator ModuleView(Module module)
        {
            return module.MapTo<ModuleView>();
        }

        public static implicit operator Module(ModuleView view)
        {
            return view.MapTo<Module>();
        }
    }
}