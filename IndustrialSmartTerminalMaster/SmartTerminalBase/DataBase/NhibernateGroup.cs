using System;
using FluentNHibernate.Mapping;
namespace SmartTerminalBase.DataBase
{
    //Your mapping class should be listed below
    //***********************************************

    class historydata
    {
        public virtual int idhistorydata {set;get; }
        public virtual string json_string { set; get; }
        public virtual DateTime storetime { set; get; }
    }

    class historydataMapping:ClassMap<historydata>
    {
        public historydataMapping()
        {
            Table("historydata");
            Id<int>("idhistorydata").GeneratedBy.Identity();
           
            Map(m => m.json_string).Nullable();
            Map(m => m.storetime).Nullable();

        }
    }
}
