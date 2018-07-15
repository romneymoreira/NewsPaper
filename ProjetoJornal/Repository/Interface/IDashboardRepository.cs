using ProjetoJornal.Areas.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Repository.Interface
{
    public interface IDashboardRepository
    {
        DashboardModel Dashboard();
    }
}