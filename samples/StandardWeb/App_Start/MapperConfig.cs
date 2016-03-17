using System;
using AutoMapper;
using Orzoo.AspNet.Infrastructure;
using Orzoo.AspNet.Mvc;
using Orzoo.Core.Extensions;
using Orzoo.Core.Utility;
using StandardWeb.Models.Maps;

namespace StandardWeb
{
    public class MapperConfig
    {
        #region Methods

        #region Public Methods

        public static void Configure()
        {
             DynamicMapperConfig.Configure();
        }

        #endregion

        #endregion
    }
}