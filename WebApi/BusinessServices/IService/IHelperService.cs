using System;
using System.Collections.Generic;
using BusinessEntities;

namespace BusinessServices
{
    /// <summary>
    /// Helper Service Contract
    /// </summary>
    public interface IHelperService
    {
        StaticDataEntity GetStaticData();
        DateTime GetDate(DateTime dt,bool time=false);
    }
}
