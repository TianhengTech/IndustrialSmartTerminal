#include "historydata.h"
#include "precompiled.h"
#include <QxMemLeak.h>

QX_REGISTER_CPP_SMARTTERMINAL(historydata)
//ORM映射类
namespace qx{
    template <> void register_class(QxClass<historydata>&t)
    {
        t.setName("historydata");
        t.id(&historydata::id,"idhistorydata");
        t.data(&historydata::json,"json_string");
        t.data(&historydata::time,"storetime");
    }
}
